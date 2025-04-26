using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Base
{
    public class PlayerBehaviour : PlayerStats
    {
        [SerializeField] private PlayerAttackRange playerAttackRange;
        [Header("Sliders")]
        [SerializeField] public Slider attackSlider;
        [SerializeField] public Slider hpSlider;
        [SerializeField] public Slider defSlider;
        [SerializeField] public Slider atkSpeedSlider;
        [SerializeField] public Slider speedSlider;
        [SerializeField] public Slider rangeSlider;
        
        [Header("Stats Value")]
        [SerializeField] public TextMeshProUGUI attackSliderText;
        [SerializeField] public TextMeshProUGUI hpSliderText;
        [SerializeField] public TextMeshProUGUI defSliderText;
        [SerializeField] public TextMeshProUGUI atkSpeedSliderText;
        [SerializeField] public TextMeshProUGUI speedSliderText;
        [SerializeField] public TextMeshProUGUI rangeSliderText;

        private Dictionary<StatType, float> lastValidStatValues = new();

        private void Awake()
        {
            InitialRandomStats();
            if (attackSlider == null)
            {
                // =)) enemy
                RandomEnemyStats();
                return;
            }
            else
            {
                RandomPlayerStats();
            }
            ForceUIUpdate();
            SetUpListeners();
        }

        private void Start()
        {
            if (attackSlider == null) return;
            lastValidStatValues[StatType.ATK] = AttackDamage;
            lastValidStatValues[StatType.HP] = MaxHealth;
            lastValidStatValues[StatType.DEF] = Armor;
            lastValidStatValues[StatType.ATKSPEED] = AttackSpeed;
            lastValidStatValues[StatType.SPEED] = Speed;
            lastValidStatValues[StatType.RANGE] = Range;

            OnAttackDamageChanged += (oldVal, newVal) =>
            {
                attackSlider.value = newVal; // optional to reset UI
            };

            OnMaxHealthChanged += (oldVal, newVal) => { hpSlider.value = newVal; };

            OnArmorChanged += (oldVal, newVal) => { defSlider.value = newVal; };
            OnAttackSpeedChanged += (oldVal, newVal) => { atkSpeedSlider.value = newVal; };
            OnSpeedChanged += (oldVal, newVal) => { speedSlider.value = newVal; };
            OnRangeChanged += (oldVal, newVal) => { rangeSlider.value = newVal; };
        }

        // Listeners for the sliders to call OnStatSliderChanged when their values change.
        public void SetUpListeners()
        {
            attackSlider.onValueChanged.AddListener(value => OnStatSliderChanged(StatType.ATK, value));
            hpSlider.onValueChanged.AddListener(value => OnStatSliderChanged(StatType.HP, value));
            defSlider.onValueChanged.AddListener(value => OnStatSliderChanged(StatType.DEF, value));
            atkSpeedSlider.onValueChanged.AddListener(value => OnStatSliderChanged(StatType.ATKSPEED, value));
            speedSlider.onValueChanged.AddListener(value => OnStatSliderChanged(StatType.SPEED, value));
            rangeSlider.onValueChanged.AddListener(value => OnStatSliderChanged(StatType.RANGE, value));
        }

        public void OnStatSliderChanged(StatType stat, float newValue)
        {
            if (CanChangeStat(stat, newValue))
            {
                ApplyStatChange(stat, newValue);
                lastValidStatValues[stat] = newValue; // Update last valid value
                if (stat == StatType.RANGE && playerAttackRange != null)
                {
                    playerAttackRange.SetAttackRange(newValue);
                }

                Debug.Log($"Stat {stat} changed to {newValue}");
            }
            else
            {
                // Revert slider back to last valid value
                switch (stat)
                {
                    case StatType.ATK:
                        attackSlider.SetValueWithoutNotify(lastValidStatValues[stat]);
                        break;
                    case StatType.HP:
                        hpSlider.SetValueWithoutNotify(lastValidStatValues[stat]);
                        break;
                    case StatType.DEF:
                        defSlider.SetValueWithoutNotify(lastValidStatValues[stat]);
                        break;
                    case StatType.ATKSPEED:
                        atkSpeedSlider.SetValueWithoutNotify(lastValidStatValues[stat]);
                        break;
                    case StatType.SPEED:
                        speedSlider.SetValueWithoutNotify(lastValidStatValues[stat]);
                        break;
                    case StatType.RANGE:
                        rangeSlider.SetValueWithoutNotify(lastValidStatValues[stat]);
                        break;
                }
            }
            
            ForceUIUpdate();
        }

        private void ForceUIUpdate()
        {
            attackSlider.value = AttackDamage;
            hpSlider.value = MaxHealth;
            defSlider.value = Armor;
            atkSpeedSlider.value = AttackSpeed;
            speedSlider.value = Speed;
            rangeSlider.value = Range;

            attackSliderText.text = AttackDamage + "";
            hpSliderText.text = MaxHealth + "";
            defSliderText.text = Armor + "";
            atkSpeedSliderText.text = AttackSpeed + "";
            speedSliderText.text = Speed + "";
            rangeSliderText.text = Range + "";
            
            if (playerAttackRange != null)
            {
                playerAttackRange.SetAttackRange(Range);
            }
        }
        public void InitialRandomStats()
        {
            List<int> numbers = new List<int> { 0, 1, 2, 3, 4, 5 };

            // Shuffle
            for (int i = 0; i < numbers.Count; i++)
            {
                int rand = Random.Range(0, numbers.Count);
                (numbers[i], numbers[rand]) = (numbers[rand], numbers[i]);
            }

            // Put into Queue
            Queue<int> queue = new Queue<int>(numbers);
            linkedStats = new List<StatConfig>();

            // Dequeue in pairs
            while (queue.Count > 0)
            {
                int a = queue.Dequeue();
                int b = queue.Dequeue();
                linkedStats.Add(new StatConfig
                {
                    BaseStat = (StatType)a,
                    LinkedStat = new[] { (StatType)b },
                    Ratio = 1f
                });
                
                linkedStats.Add(new StatConfig
                {
                    BaseStat = (StatType)b,
                    LinkedStat = new[] { (StatType)a },
                    Ratio = 1f
                });
            }
        }

        private void RandomPlayerStats()
        {
            AttackDamage = Random.Range(4, 6);
            MaxHealth = Random.Range(4, 6);
            Armor = Random.Range(4, 6);
            AttackSpeed = Random.Range(4, 6);
            Speed = Random.Range(4, 6);
            Range = Random.Range(4, 6);
        }
        
        private void RandomEnemyStats()
        {
            AttackDamage = Random.Range(4, 8);
            MaxHealth = Random.Range(4, 8);
            Armor = Random.Range(4, 8);
            AttackSpeed = Random.Range(4, 8);
            Speed = Random.Range(4, 8);
            Range = Random.Range(4, 8);
        }
    }
}