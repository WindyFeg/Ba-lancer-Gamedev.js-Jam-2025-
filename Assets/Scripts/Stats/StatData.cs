using System.Collections.Generic;
using UnityEngine;

// ----- Monster -----

[System.Serializable]
public class MonstersData
{
    public List<MonsterData> Monsters;
            
    public MonstersData(List<MonsterData> monsters)
    {
        Monsters = monsters;
    }
}
        
[System.Serializable]
public class MonsterData
{
    public string Name;

    public int Attack = 5;
    public int AttackMin = 1, AttackMax = 10;

    public int Hp = 5;
    public int HpMin = 1, HpMax = 10;

    public int Speed = 5;
    public int SpeedMin = 1, SpeedMax = 10;

    public int Def = 5;
    public int DefMin = 1, DefMax = 10;
    
    public int AttackSpeed = 1;
    public int AttackSpeedMin = 1, AttackSpeedMax = 10;
    
    public int Range = 5;
    public int RangeMin = 1, RangeMax = 10;
            
    // Custom stat configurations (green-red relationships)
    public List<StatConfiguration> StatConfig = new List<StatConfiguration>();

    public MonsterData(string name)
    {
        Name = name;
    }
}

[System.Serializable]
public class StatConfiguration
{
    // The green stat (left side)
    public StatType PrimaryStat = StatType.ATK;
            
    // The list of red stats (right side)
    public List<StatType> RelatedStats = new List<StatType> { StatType.HP };
}

// ----- Objects -----

[System.Serializable]
public class ObjectData
{
    public string Name;
        
    // Stats
    public int Hp;
    public float Size;
        
    // Min-Max values for stats
    public int HpMin = 1;
    public int HpMax = 10;
    public float SizeMin = 0.1f;
    public float SizeMax = 10.0f;
        
    // Stat configurations (relationships between stats)
    public List<ObjectStatConfiguration> StatConfig = new List<ObjectStatConfiguration>();
        
    public ObjectData(string name)
    {
        Name = name;
        Hp = 5;
        Size = 1.0f;
    }
}
// Define the ObjectStatConfiguration class
[System.Serializable]
public class ObjectStatConfiguration
{
    public ObjectStatType PrimaryStat = ObjectStatType.HP;
    public List<ObjectStatType> RelatedStats = new List<ObjectStatType> { ObjectStatType.SIZE };
}

// ----- Relics -----

[System.Serializable]
public class RelicData
{
    public string Name;
    
    public int Attack;
    public int Hp;
    public int Speed;
    public int Def;
    public int AttackSpeed;
    public int Range;
    
    public List<RelicStatConfiguration> StatConfig = new List<RelicStatConfiguration>();
    
    public RelicData(string name)
    {
        Name = name;
    }
}

[System.Serializable]
public class RelicStatConfiguration
{
    public StatType PrimaryStat = StatType.ATK;
    public int Value = 1;
}