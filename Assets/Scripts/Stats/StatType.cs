// === Enum definition for Stats ===

using System;

[Serializable]
public enum StatType
{
    ATK,
    HP,
    DEF,
    SPEED,
    ATKSPEED,
    RANGE,
}

// Define the ObjectStatType enum
[Serializable]
public enum ObjectStatType
{
    HP,
    SIZE,
    // Add other object-specific stats here
}