using System.Collections.Generic;
using Game;
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

        private Dictionary<StatType, float> lastValidStatValues = new();
        private void Start()
        {
            if (attackSlider == null) return;
            InitialRandomStats();

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

            OnMaxHealthChanged += (oldVal, newVal) =>
            {
                hpSlider.value = newVal;
            };

            OnArmorChanged += (oldVal, newVal) =>
            {
                defSlider.value = newVal;
            };
            OnAttackSpeedChanged += (oldVal, newVal) =>
            {
                atkSpeedSlider.value = newVal;
            };
            OnSpeedChanged += (oldVal, newVal) =>
            {
                speedSlider.value = newVal;
            };
            OnRangeChanged += (oldVal, newVal) =>
            {
                rangeSlider.value = newVal;
            };

            // Set initial values
            AttackDamage = this.AttackDamage;
            MaxHealth = this.MaxHealth;
            Armor = this.Armor;
            attackSlider.value = this.AttackDamage;
            hpSlider.value = this.MaxHealth;
            defSlider.value = this.Armor;
        }
        private void Awake()
        {
            // Set up Base Stats - [Linked Stats]
            if (attackSlider == null) return;
            
            InitialRandomStats();
            SetUpListeners();
        }

        // Listeners for the sliders to call OnStatSliderChanged when their values change.
        private void SetUpListeners()
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
        }

        public void InitialRandomStats()
        {
            linkedStats = new List<StatConfig>
            {
                new StatConfig
                {
                    BaseStat = StatType.ATK,
                    LinkedStat = new[] { (StatType)Random.Range(1, 6) },
                    Ratio = 1f
                },
            };
            SetUpListeners();
        }
    }
}