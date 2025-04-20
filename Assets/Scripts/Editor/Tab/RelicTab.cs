using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.Tab
{
    public class RelicTab
    {
        private int _selectedRelicIndex = 0;
        private List<RelicData> _relics = new List<RelicData>();

        // Track the previous values of stats to detect changes
        private Dictionary<StatType, float> _previousStatValues = new Dictionary<StatType, float>();

        // File path for saving/loading
        private const string SaveFilePath = "Assets/Resources/RelicData.json";

        public RelicTab()
        {
            // Try to load existing data first
            if (!LoadObjectData())
            {
                // Initialize with 1 object by default if no data was loaded
                _relics.Add(new RelicData("Object 1"));
            }

            // Initialize previous stat values
            InitializePreviousStatValues();
        }

        private void InitializePreviousStatValues()
        {
            if (_relics.Count > 0)
            {
                _previousStatValues.Clear();
                RelicData relic = _relics[_selectedRelicIndex];
                _previousStatValues[StatType.ATK] = relic.Attack;
                _previousStatValues[StatType.HP] = relic.Hp;
                _previousStatValues[StatType.DEF] = relic.Def;
                _previousStatValues[StatType.SPEED] = relic.Speed;
                _previousStatValues[StatType.ATKSPEED] = relic.AttackSpeed;
                _previousStatValues[StatType.RANGE] = relic.Range;
            }
        }

        public void Draw()
        {
            EditorGUILayout.LabelField("Relic Design", EditorStyles.boldLabel);

            // Save and Load buttons at the top
            EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
            if (GUILayout.Button("Save Data", GUILayout.Height(25)))
            {
                SaveObjectData();
                EditorUtility.DisplayDialog("Save Complete", "Relic data saved successfully!", "OK");
            }

            if (GUILayout.Button("Load Data", GUILayout.Height(25)))
            {
                if (LoadObjectData())
                {
                    InitializePreviousStatValues();
                    EditorUtility.DisplayDialog("Load Complete", "Relic data loaded successfully!", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Load Failed", "No saved relic data found or error loading data.",
                        "OK");
                }
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            // === Left panel: sub-tabs and Add button ===
            EditorGUILayout.BeginVertical(GUILayout.Width(150));

            for (int i = 0; i < _relics.Count; i++)
            {
                GUIStyle tabStyle = new GUIStyle(GUI.skin.button);
                if (i == _selectedRelicIndex)
                    tabStyle.normal.textColor = Color.white;

                if (GUILayout.Button(_relics[i].Name, tabStyle, GUILayout.Height(30)))
                {
                    _selectedRelicIndex = i;
                    InitializePreviousStatValues();
                }
            }

            GUILayout.Space(25);

            if (GUILayout.Button("Add relic", GUILayout.Height(30)))
            {
                _relics.Add(new RelicData($"Relic {_relics.Count + 1}"));
                _selectedRelicIndex = _relics.Count - 1;
                InitializePreviousStatValues();
            }

            if (_relics.Count > 0 && GUILayout.Button("Remove relic", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog("Confirm Delete",
                        $"Are you sure you want to delete '{_relics[_selectedRelicIndex].Name}'?", "Yes", "No"))
                {
                    _relics.RemoveAt(_selectedRelicIndex);
                    _selectedRelicIndex = Mathf.Clamp(_selectedRelicIndex, 0, _relics.Count - 1);
                    InitializePreviousStatValues();
                }
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // === Right panel: object data ===
            EditorGUILayout.BeginVertical();

            if (_relics.Count > 0)
            {
                DrawRelics(_relics[_selectedRelicIndex]);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawRelics(RelicData relic)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Relic Data", EditorStyles.boldLabel);
            // Draw the name field
            relic.Name = EditorGUILayout.TextField("Name", relic.Name);

            // Draw each stat configuration row
            for (int i = 0; i < relic.StatConfig.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                    // Draw the green stat dropdown
                    RelicStatConfiguration statConfig = relic.StatConfig[i];
                    StatType oldPrimaryStat = statConfig.PrimaryStat;
                    statConfig.PrimaryStat = (StatType)EditorGUILayout.EnumPopup(statConfig.PrimaryStat, GUILayout.Width(80));
                    
                    // Draw input field for the green stat value
                    statConfig.Value = EditorGUILayout.IntField(statConfig.Value, GUILayout.Width(40));

                    // Remove this configuration row button
                    if (DrawButton("×", Color.red, GUILayout.Width(20)) && relic.StatConfig.Count > 1)
                    {
                        relic.StatConfig.RemoveAt(i);
                        i--; // Adjust index after removal
                        continue;
                    }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);
            }
            
            if (DrawButton("+", Color.green, GUILayout.Width(80)))
            {
                relic.StatConfig.Add(new RelicStatConfiguration());
            }
            GUILayout.Space(20);
            EditorGUILayout.EndVertical();
        }


        private void DrawStatConfigurations(RelicData relic)
        {
            // Draw each stat configuration row
            for (int i = 0; i < relic.StatConfig.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                // Draw the green stat dropdown
                RelicStatConfiguration statConfig = relic.StatConfig[i];
                StatType oldPrimaryStat = statConfig.PrimaryStat;
                statConfig.PrimaryStat =
                    (StatType)EditorGUILayout.EnumPopup(statConfig.PrimaryStat, GUILayout.Width(80));

                // Draw input field for the green stat value
                float newStatValue = EditorGUILayout.FloatField(statConfig.PrimaryStat.ToString(), 0f);
                if (newStatValue != _previousStatValues[statConfig.PrimaryStat])
                {
                    // Update the stat value in the relic data
                    switch (statConfig.PrimaryStat)
                    {
                        case StatType.ATK:
                            relic.Attack = (int)newStatValue;
                            break;
                        case StatType.HP:
                            relic.Hp = (int)newStatValue;
                            break;
                        case StatType.DEF:
                            relic.Def = (int)newStatValue;
                            break;
                        case StatType.SPEED:
                            relic.Speed = (int)newStatValue;
                            break;
                        case StatType.ATKSPEED:
                            relic.AttackSpeed = (int)newStatValue;
                            break;
                        case StatType.RANGE:
                            relic.Range = (int)newStatValue;
                            break;
                    }

                    // Update the previous stat value
                    _previousStatValues[statConfig.PrimaryStat] = newStatValue;
                }

                // Remove this configuration row button
                if (DrawButton("×", Color.red, GUILayout.Width(20)) && relic.StatConfig.Count > 1)
                {
                    relic.StatConfig.RemoveAt(i);
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
                relic.StatConfig.Add(new RelicStatConfiguration());
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
        private void SaveObjectData()
        {
            // Create directory if it doesn't exist
            string directory = Path.GetDirectoryName(SaveFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Serialize the object data to JSON
            RelicsData saveData = new RelicsData(_relics);
            string json = JsonUtility.ToJson(saveData, true);

            // Write to file
            File.WriteAllText(SaveFilePath, json);

            // Refresh the asset database to show the file in the editor
            AssetDatabase.Refresh();
        }

        private bool LoadObjectData()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    string json = File.ReadAllText(SaveFilePath);
                    RelicsData loadedData = JsonUtility.FromJson<RelicsData>(json);

                    if (loadedData != null && loadedData.Relics != null && loadedData.Relics.Count > 0)
                    {
                        _relics = loadedData.Relics;
                        _selectedRelicIndex = Mathf.Clamp(_selectedRelicIndex, 0, _relics.Count - 1);
                        return true;
                    }
                }

                return false;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error loading object data: " + e.Message);
                return false;
            }
        }
    }
    
    // Define the RelicsData class for JSON serialization
    [System.Serializable]
    public class RelicsData
    {
        public List<RelicData> Relics;
        
        public RelicsData(List<RelicData> Relics)
        {
            Relics = Relics;
        }
    }
}