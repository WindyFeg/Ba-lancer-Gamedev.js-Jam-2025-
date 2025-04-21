using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Base
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private int fakeAtk;
        [SerializeField] private int fakeHp;
        [SerializeField] private int fakeDef;
        [SerializeField] private int fakeAtkSpeed;
        [SerializeField] private int fakeSpeed;
        [SerializeField] private int fakeRange;
        
        [Header("Sliders")]
        [SerializeField] private Slider attackSlider;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider defSlider;
        [SerializeField] private Slider atkSpeedSlider;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private Slider rangeSlider;
        
        public PlayerStats stats { get; private set; } = new PlayerStats(); 
        
        private Dictionary<StatType, float> lastValidStatValues = new();
        private void Start()
        {
            lastValidStatValues[StatType.ATK] = stats.AttackDamage;
            lastValidStatValues[StatType.HP] = stats.MaxHealth;
            lastValidStatValues[StatType.DEF] = stats.Armor;
            lastValidStatValues[StatType.ATKSPEED] = stats.AttackSpeed;
            lastValidStatValues[StatType.SPEED] = stats.Speed;
            lastValidStatValues[StatType.RANGE] = stats.Range;
            
            stats.OnAttackDamageChanged += (oldVal, newVal) =>
            {
                attackSlider.value = newVal; // optional to reset UI
            };

            stats.OnMaxHealthChanged += (oldVal, newVal) =>
            {
                hpSlider.value = newVal;
            };
            
            stats.OnArmorChanged += (oldVal, newVal) =>
            {
                defSlider.value = newVal;
            };
            stats.OnAttackSpeedChanged += (oldVal, newVal) =>
            {
                atkSpeedSlider.value = newVal;
            };
            stats.OnSpeedChanged += (oldVal, newVal) =>
            {
                speedSlider.value = newVal;
            };
            stats.OnRangeChanged += (oldVal, newVal) =>
            {
                rangeSlider.value = newVal;
            };
            
            // Set initial values
            stats.AttackDamage = fakeAtk;
            stats.MaxHealth = fakeHp;
            stats.Armor = fakeDef;
            attackSlider.value = fakeAtk;
            hpSlider.value = fakeHp;
            defSlider.value = fakeDef;
        }
        private void Awake()
        {
            // Set up Base Stats - [Linked Stats]
            stats.linkedStats = new List<StatConfig>
            {
                new StatConfig
                {
                    BaseStat = StatType.ATK,
                    LinkedStat = new[] { StatType.HP },
                    Ratio = 1f
                },
                new StatConfig
                {
                    BaseStat = StatType.HP,
                    LinkedStat = new[] { StatType.DEF },
                    Ratio = 1f
                }
            };
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
            if (stats.CanChangeStat(stat, newValue))
            {
                stats.ApplyStatChange(stat, newValue);
                lastValidStatValues[stat] = newValue; // Update last valid value
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
    }
}