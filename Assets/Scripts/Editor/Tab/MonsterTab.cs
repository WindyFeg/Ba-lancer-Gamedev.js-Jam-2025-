using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.Tab
{
    public class MonsterTab
    {
        private int _selectedMonsterIndex = 0;
        private List<MonsterData> _monsters = new List<MonsterData>();
        
        // Track the previous values of stats to detect changes
        private Dictionary<StatType, int> _previousStatValues = new Dictionary<StatType, int>();

        // File path for saving/loading
        private const string SaveFilePath = "Assets/Resources/MonsterData.json";

        public MonsterTab()
        {
            // Try to load existing data first
            if (!LoadMonsterData())
            {
                // Initialize with 1 monster by default if no data was loaded
                _monsters.Add(new MonsterData("Monster 1"));
            }
            
            // Initialize previous stat values
            InitializePreviousStatValues();
        }
        
        private void InitializePreviousStatValues()
        {
            if (_monsters.Count > 0)
            {
                _previousStatValues.Clear();
                MonsterData monster = _monsters[_selectedMonsterIndex];
                _previousStatValues[StatType.ATK] = monster.Attack;
                _previousStatValues[StatType.HP] = monster.Hp;
                _previousStatValues[StatType.DEF] = monster.Def;
                _previousStatValues[StatType.SPEED] = monster.Speed;
                _previousStatValues[StatType.ATKSPEED] = monster.AttackSpeed;
                _previousStatValues[StatType.RANGE] = monster.Range;
            }
        }

        public void Draw()
        {
            EditorGUILayout.LabelField("Monster Design", EditorStyles.boldLabel);

            // Save and Load buttons at the top
            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            if (GUILayout.Button("Save Data", GUILayout.Height(25)))
            {
                SaveMonsterData();
                EditorUtility.DisplayDialog("Save Complete", "Monster data saved successfully!", "OK");
            }

            if (GUILayout.Button("Load Data", GUILayout.Height(25)))
            {
                if (LoadMonsterData())
                {
                    InitializePreviousStatValues();
                    EditorUtility.DisplayDialog("Load Complete", "Monster data loaded successfully!", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Load Failed", "No saved monster data found or error loading data.",
                        "OK");
                }
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            // === Left panel: sub-tabs and Add button ===
            EditorGUILayout.BeginVertical(GUILayout.Width(150));

            for (int i = 0; i < _monsters.Count; i++)
            {
                GUIStyle tabStyle = new GUIStyle(GUI.skin.button);
                if (i == _selectedMonsterIndex)
                    tabStyle.normal.textColor = Color.white;

                if (GUILayout.Button(_monsters[i].Name, tabStyle, GUILayout.Height(30)))
                {
                    _selectedMonsterIndex = i;
                    InitializePreviousStatValues();
                }
            }

            GUILayout.Space(25);

            if (GUILayout.Button("Add monster", GUILayout.Height(30)))
            {
                _monsters.Add(new MonsterData($"Monster {_monsters.Count + 1}"));
                _selectedMonsterIndex = _monsters.Count - 1;
                InitializePreviousStatValues();
            }

            if (_monsters.Count > 0 && GUILayout.Button("Remove monster", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog("Confirm Delete",
                        $"Are you sure you want to delete '{_monsters[_selectedMonsterIndex].Name}'?", "Yes", "No"))
                {
                    _monsters.RemoveAt(_selectedMonsterIndex);
                    _selectedMonsterIndex = Mathf.Clamp(_selectedMonsterIndex, 0, _monsters.Count - 1);
                    InitializePreviousStatValues();
                }
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // === Right panel: monster data ===
            EditorGUILayout.BeginVertical();

            if (_monsters.Count > 0)
            {
                DrawMonster(_monsters[_selectedMonsterIndex]);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawMonster(MonsterData monster)
        {
            monster.Name = EditorGUILayout.TextField("Name", monster.Name);

            // Draw the basic stats and detect changes
            bool attackChanged = DrawStatBar("Attack", ref monster.Attack, Color.red, ref monster.AttackMin, ref monster.AttackMax);
            bool hpChanged = DrawStatBar("HP", ref monster.Hp, Color.gray, ref monster.HpMin, ref monster.HpMax);
            bool speedChanged = DrawStatBar("Speed", ref monster.Speed, Color.red, ref monster.SpeedMin, ref monster.SpeedMax);
            bool defChanged = DrawStatBar("Def", ref monster.Def, Color.gray, ref monster.DefMin, ref monster.DefMax);
            bool atkSpeedChanged = DrawStatBar("AtkSpeed", ref monster.AttackSpeed, Color.gray, ref monster.AttackSpeedMin, ref monster.AttackSpeedMax);
            bool rangeChanged = DrawStatBar("Range", ref monster.Range, Color.gray, ref monster.RangeMin, ref monster.RangeMax);

            // Process related stat changes if any primary stat changed
            if (attackChanged) ProcessStatChange(monster, StatType.ATK);
            if (hpChanged) ProcessStatChange(monster, StatType.HP);
            if (speedChanged) ProcessStatChange(monster, StatType.SPEED);
            if (defChanged) ProcessStatChange(monster, StatType.DEF);
            if (atkSpeedChanged) ProcessStatChange(monster, StatType.ATKSPEED);
            if (rangeChanged) ProcessStatChange(monster, StatType.RANGE);

            // Update previous values for next frame
            _previousStatValues[StatType.ATK] = monster.Attack;
            _previousStatValues[StatType.HP] = monster.Hp;
            _previousStatValues[StatType.DEF] = monster.Def;
            _previousStatValues[StatType.SPEED] = monster.Speed;
            _previousStatValues[StatType.ATKSPEED] = monster.AttackSpeed;
            _previousStatValues[StatType.RANGE] = monster.Range;

            GUILayout.Space(20);
            EditorGUILayout.LabelField("Main stat - [Related stats]", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Change the main stat to adjust related stats but reverse.");
            DrawStatConfigurations(monster);
        }

        private bool DrawStatBar(string label, ref int value, Color barColor, ref int minValue, ref int maxValue)
        {
            EditorGUILayout.BeginHorizontal();

            // Label with current int value
            EditorGUILayout.LabelField($"{label}: {value}", GUILayout.Width(100));

            // Store the old value to detect changes
            int oldValue = value;

            // Slider background and interaction
            Rect sliderRect = GUILayoutUtility.GetRect(150, 20);
            EditorGUI.DrawRect(new Rect(sliderRect.x, sliderRect.y, sliderRect.width, sliderRect.height),
                new Color(0.2f, 0.2f, 0.2f));
            value = (int)Mathf.Clamp(GUI.HorizontalSlider(sliderRect, value, minValue, maxValue), minValue, maxValue);

            // Min and Max as int fields
            minValue = EditorGUILayout.IntField(minValue, GUILayout.Width(40));
            maxValue = EditorGUILayout.IntField(maxValue, GUILayout.Width(40));

            EditorGUILayout.EndHorizontal();

            // Return true if the value changed
            return value != oldValue;
        }

        private void ProcessStatChange(MonsterData monster, StatType changedStat)
        {
            // Find the difference between current and previous value
            int previousValue = _previousStatValues[changedStat];
            int currentValue = GetStatValue(monster, changedStat);
            int difference = currentValue - previousValue;
            
            // If no change or no configurations, nothing to do
            if (difference == 0 || monster.StatConfig.Count == 0)
                return;

            // Check if the stat is increasing
            bool isIncreasing = difference > 0;

            // Process stat configurations where the changed stat is the primary stat
            foreach (var config in monster.StatConfig)
            {
                if (config.PrimaryStat == changedStat)
                {
                    // If any related stat is at minimum and we're trying to increase the primary stat,
                    // revert the change and exit
                    if (isIncreasing && config.RelatedStats.Any(relatedStat => IsStatAtMinimum(monster, relatedStat)))
                    {
                        // Revert the primary stat change
                        AdjustStatValue(monster, changedStat, -difference);
                        return;
                    }
                    
                    // This primary stat changed, so adjust all related stats
                    foreach (var relatedStat in config.RelatedStats)
                    {
                        // Calculate the inverse effect - when primary increases, related decreases
                        int relatedStatDelta = -difference;
                        
                        // Apply the change to the related stat
                        AdjustStatValue(monster, relatedStat, relatedStatDelta);
                    }
                }
            }
        }

        private bool IsStatAtMinimum(MonsterData monster, StatType statType)
        {
            switch (statType)
            {
                case StatType.ATK: return monster.Attack <= monster.AttackMin;
                case StatType.HP: return monster.Hp <= monster.HpMin;
                case StatType.DEF: return monster.Def <= monster.DefMin;
                case StatType.SPEED: return monster.Speed <= monster.SpeedMin;
                case StatType.ATKSPEED: return monster.AttackSpeed <= monster.AttackSpeedMin;
                case StatType.RANGE: return monster.Range <= monster.RangeMin;
                default: return false;
            }
        }

        private int GetStatValue(MonsterData monster, StatType statType)
        {
            switch (statType)
            {
                case StatType.ATK: return monster.Attack;
                case StatType.HP: return monster.Hp;
                case StatType.DEF: return monster.Def;
                case StatType.SPEED: return monster.Speed;
                case StatType.ATKSPEED: return monster.AttackSpeed;
                case StatType.RANGE: return monster.Range;
                default: return 0;
            }
        }

        private void AdjustStatValue(MonsterData monster, StatType statType, int delta)
        {
            switch (statType)
            {
                case StatType.ATK:
                    monster.Attack = Mathf.Clamp(monster.Attack + delta, monster.AttackMin, monster.AttackMax);
                    _previousStatValues[StatType.ATK] = monster.Attack;
                    break;
                case StatType.HP:
                    monster.Hp = Mathf.Clamp(monster.Hp + delta, monster.HpMin, monster.HpMax);
                    _previousStatValues[StatType.HP] = monster.Hp;
                    break;
                case StatType.DEF:
                    monster.Def = Mathf.Clamp(monster.Def + delta, monster.DefMin, monster.DefMax);
                    _previousStatValues[StatType.DEF] = monster.Def;
                    break;
                case StatType.SPEED:
                    monster.Speed = Mathf.Clamp(monster.Speed + delta, monster.SpeedMin, monster.SpeedMax);
                    _previousStatValues[StatType.SPEED] = monster.Speed;
                    break;
                case StatType.ATKSPEED:
                    monster.AttackSpeed = Mathf.Clamp(monster.AttackSpeed + delta, monster.AttackSpeedMin, monster.AttackSpeedMax);
                    _previousStatValues[StatType.ATKSPEED] = monster.AttackSpeed;
                    break;
                case StatType.RANGE:
                    monster.Range = Mathf.Clamp(monster.Range + delta, monster.RangeMin, monster.RangeMax);
                    _previousStatValues[StatType.RANGE] = monster.Range;
                    break;
            }
        }

        private void DrawStatConfigurations(MonsterData monster)
        {
            // Draw each stat configuration row
            for (int i = 0; i < monster.StatConfig.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                // Draw the green stat dropdown
                StatConfiguration statConfig = monster.StatConfig[i];
                StatType oldPrimaryStat = statConfig.PrimaryStat;
                statConfig.PrimaryStat = (StatType)EditorGUILayout.EnumPopup(statConfig.PrimaryStat, GUILayout.Width(80));
                
                // Draw each red stat in this row
                for (int j = 0; j < statConfig.RelatedStats.Count; j++)
                {
                    statConfig.RelatedStats[j] = (StatType)EditorGUILayout.EnumPopup(statConfig.RelatedStats[j], GUILayout.Width(80));
                }

                // Add red stat button
                if (DrawButton("+", new Color(1f, 0.5f, 0.5f), GUILayout.Width(30)))
                {
                    statConfig.RelatedStats.Add(StatType.ATK);
                }

                // Remove this configuration row button
                if (DrawButton("×", Color.red, GUILayout.Width(20)) && monster.StatConfig.Count > 1)
                {
                    monster.StatConfig.RemoveAt(i);
                    i--; // Adjust index after removal
                    continue;
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            // Add a new green stat row button
            EditorGUILayout.BeginHorizontal();
            if (DrawButton("+", Color.green, GUILayout.Width(80)))
            {
                monster.StatConfig.Add(new StatConfiguration());
            }

            EditorGUILayout.EndHorizontal();
        }

        private bool DrawButton(string label, Color color, GUILayoutOption layout)
        {
            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            bool clicked = GUILayout.Button(label, layout, GUILayout.Height(20));
            GUI.backgroundColor = oldColor;
            return clicked;
        }

        // === JSON Saving and Loading ===
        private void SaveMonsterData()
        {
            // Create directory if it doesn't exist
            string directory = Path.GetDirectoryName(SaveFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Serialize the monster data to JSON
            MonstersData saveData = new MonstersData(_monsters);
            string json = JsonUtility.ToJson(saveData, true);

            // Write to file
            File.WriteAllText(SaveFilePath, json);

            // Refresh the asset database to show the file in the editor
            AssetDatabase.Refresh();
        }

        private bool LoadMonsterData()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    string json = File.ReadAllText(SaveFilePath);
                    MonstersData loadedData = JsonUtility.FromJson<MonstersData>(json);

                    if (loadedData != null && loadedData.Monsters != null && loadedData.Monsters.Count > 0)
                    {
                        _monsters = loadedData.Monsters;
                        _selectedMonsterIndex = Mathf.Clamp(_selectedMonsterIndex, 0, _monsters.Count - 1);
                        return true;
                    }
                }

                return false;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error loading monster data: " + e.Message);
                return false;
            }
        }
    }
}