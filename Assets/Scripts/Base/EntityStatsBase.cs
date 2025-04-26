using System;
using UnityEngine;

[Serializable]
public class EntityStatsBase : MonoBehaviour
{
    public string EntityName { get; set; }

    [SerializeField]
    private int _level = 1;
    public int Level
    {
        get => _level;
        set
        {
            if (value < 1)
            {
                _level = 1;
            }
            else
            {
                _level = value;
            }
        }
    }
    [SerializeField]

    private float _maxHealth;
    public float MaxHealth
    {
        get => _maxHealth;
        set
        {
            if (value < 0)
            {
                value = 1;
            }
            OnMaxHealthChanged?.Invoke(_maxHealth, value);
            _maxHealth = value;
        }
    }
    [SerializeField]

    private float _currentHealth;
    public float CurrentHealth
    {   
        get => _currentHealth;
        set
        {
            if (value < 0)
            {
                value = -1;
            }
            if (value > MaxHealth)
            {
                value = MaxHealth;
            }
            OnMaxHealthChanged?.Invoke(_currentHealth, value);
            _currentHealth = value;
        }
    }
    [SerializeField]

    private float _attackDamage;
    public float AttackDamage
    {
        get => _attackDamage;
        set
        {
            if (value < 0)
            {
                value = 1;
            }
            OnAttackDamageChanged?.Invoke(_attackDamage, value);
            _attackDamage = value;
        }
    }
    [SerializeField]

    private float _attackSpeed;
    public float AttackSpeed
    {
        get => _attackSpeed;
        set
        {
            if (value < 0)
            {
                value = 1;
            }
            OnAttackSpeedChanged?.Invoke(_attackSpeed, value);
            _attackSpeed = value;
        }
    }
    [SerializeField]

    private float _armor;
    public float Armor
    {
        get => _armor;
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            OnArmorChanged?.Invoke(_armor, value);
            _armor = value;
        }
    }
    [SerializeField]
    
    private float _speed;
    public float Speed
    {
        get => _speed;
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            _speed = value;
        }
    }
    [SerializeField]
    
    private float _range;
    public float Range
    {
        get => _range;
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            _range = value;
        }
    }

    // Change Stats Event
    public event Action<float, float> OnMaxHealthChanged;
    public event Action<float, float> OnAttackDamageChanged;
    public event Action<float, float> OnAttackSpeedChanged;
    public event Action<float, float> OnArmorChanged;
    public event Action<float, float> OnSpeedChanged;
    public event Action<float, float> OnRangeChanged;
}