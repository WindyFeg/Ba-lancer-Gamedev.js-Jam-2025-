using System.Collections.Generic;
using UnityEngine;

namespace GameDefine
{
    public class GameConfig : SingletonScriptObject<GameConfig>
    {
        // Static instance that can be accessed from anywhere
        private static GameConfig _instance;
        public static GameConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<GameConfig>("GameConfig");
                    if (_instance == null)
                    {
                        Debug.LogError("GameConfig not found in Resources folder!");
                    }
                }
                return _instance;
            }
        }

        public List<MonsterData> monsters = new();
        public List<ObjectData> objects = new();
        // public List<LevelData> levels = new();
      
        // Called when the script instance is being loaded
        protected virtual void OnEnable()
        {
            _instance = this;
        }
      
        //--------------------- Monster ---------------------
        public MonsterData FindMonster(string name) { return monsters.Find(m => m.Name == name); }
      
        public ObjectData FindObject(string name) { return objects.Find(o => o.Name == name); }
      
        // public LevelData FindLevel(int level) { return levels.Find(l => l.Level == level); }
    }
}