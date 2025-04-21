using System;
using UnityEngine;

public abstract class EntityStatsBase : MonoBehaviour
{
    public string EntityName { get; set; }

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

    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

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

    // Change Stats Event
    public event Action<float, float> OnMaxHealthChanged;
    public event Action<float, float> OnAttackDamageChanged;
    public event Action<float, float> OnAttackSpeedChanged;
    public event Action<float, float> OnArmorChanged;
}