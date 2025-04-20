// using System.Collections.Generic;
// using System.IO;
// using UnityEditor;
// using UnityEngine;
//
// namespace Editor.Tab
// {
//     public class LevelDesignTab
//     {
//         private int _selectedMonsterIndex = 0;
//         private List<MonsterData> _monsters = new List<MonsterData>();
//
//         // Track the previous values of stats to detect changes
//         private Dictionary<ObjectStatType, float> _previousStatValues = new Dictionary<ObjectStatType, float>();
//         
//         // File path for saving/loading
//         private const string SaveFilePath = "Assets/Resources/ObjectData.json";
//
//         public LevelDesignTab()
//         {
//             // Initialize previous stat values
//             InitializePreviousStatValues();
//         }
//
//         private void InitializePreviousStatValues()
//         {
//             
//         }
//
//         public void Draw()
//         {
//             EditorGUILayout.LabelField("Level Design", EditorStyles.boldLabel);
//
//             // Save and Load buttons at the top
//             EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
//             if (GUILayout.Button("Save Data", GUILayout.Height(25)))
//             {
//                 SaveObjectData();
//                 EditorUtility.DisplayDialog("Save Complete", "Level data saved successfully!", "OK");
//             }
//
//             if (GUILayout.Button("Load Data", GUILayout.Height(25)))
//             {
//                 if (LoadObjectData())
//                 {
//                     InitializePreviousStatValues();
//                     EditorUtility.DisplayDialog("Load Complete", "Level data loaded successfully!", "OK");
//                 }
//                 else
//                 {
//                     EditorUtility.DisplayDialog("Load Failed", "No saved level data found or error loading data.",
//                         "OK");
//                 }
//             }
//
//             EditorGUILayout.EndHorizontal();
//
//             GUILayout.Space(10);
//
//             EditorGUILayout.BeginHorizontal();
//
//             // === Left panel: sub-tabs and Add button ===
//             EditorGUILayout.BeginVertical(GUILayout.Width(150));
//
//             for (int i = 0; i < _monsters.Count; i++)
//             {
//                 GUIStyle tabStyle = new GUIStyle(GUI.skin.button);
//                 if (i == _selectedMonsterIndex)
//                     tabStyle.normal.textColor = Color.white;
//
//                 if (GUILayout.Button(_monsters[i].Name, tabStyle, GUILayout.Height(30)))
//                 {
//                     _selectedMonsterIndex = i;
//                     InitializePreviousStatValues();
//                 }
//             }
//
//             GUILayout.Space(25);
//
//             if (GUILayout.Button("Add object", GUILayout.Height(30)))
//             {
//                 _monsters.Add(new MonsterData($"Monster {_monsters.Count + 1}"));
//                 _selectedMonsterIndex = _monsters.Count - 1;
//                 InitializePreviousStatValues();
//             }
//
//             if (_monsters.Count > 0 && GUILayout.Button("Remove object", GUILayout.Height(30)))
//             {
//                 if (EditorUtility.DisplayDialog("Confirm Delete",
//                         $"Are you sure you want to delete '{_monsters[_selectedMonsterIndex].Name}'?", "Yes", "No"))
//                 {
//                     _monsters.RemoveAt(_selectedMonsterIndex);
//                     _selectedMonsterIndex = Mathf.Clamp(_selectedMonsterIndex, 0, _monsters.Count - 1);
//                     InitializePreviousStatValues();
//                 }
//             }
//
//             EditorGUILayout.EndVertical();
//
//             GUILayout.Space(10);
//
//             // === Right panel: object data ===
//             EditorGUILayout.BeginVertical();
//
//             if (_monsters.Count > 0)
//             {
//             }
//
//             EditorGUILayout.EndVertical();
//             EditorGUILayout.EndHorizontal();
//         }
//
//         private void DrawObject(ObjectData obj)
//         {
//             obj.Name = EditorGUILayout.TextField("Name", obj.Name);
//
//             // Draw the basic stats and detect changes
//             bool hpChanged = DrawIntStatBar("HP", ref obj.Hp, Color.green, ref obj.HpMin, ref obj.HpMax);
//             bool sizeChanged = DrawFloatStatBar("Size", ref obj.Size, Color.blue, ref obj.SizeMin, ref obj.SizeMax);
//
//             // Process related stat changes if any primary stat changed
//             if (hpChanged) ProcessStatChange(obj, ObjectStatType.HP);
//             if (sizeChanged) ProcessStatChange(obj, ObjectStatType.SIZE);
//
//             // Update previous values for next frame
//             _previousStatValues[ObjectStatType.HP] = obj.Hp;
//             _previousStatValues[ObjectStatType.SIZE] = obj.Size;
//
//             GUILayout.Space(20);
//             EditorGUILayout.LabelField("Main stat - [Related stats]", EditorStyles.boldLabel);
//             DrawStatConfigurations(obj);
//         }
//
//         private bool DrawFloatStatBar(string label, ref float value, Color barColor, ref float minValue,
//             ref float maxValue)
//         {
//             EditorGUILayout.BeginHorizontal();
//
//             // Label with current float value (formatted to 2 decimal places)
//             EditorGUILayout.LabelField($"{label}: {value:F2}", GUILayout.Width(100));
//
//             // Store the old value to detect changes
//             float oldValue = value;
//
//             // Slider background and interaction
//             Rect sliderRect = GUILayoutUtility.GetRect(150, 20);
//             EditorGUI.DrawRect(new Rect(sliderRect.x, sliderRect.y, sliderRect.width, sliderRect.height),
//                 new Color(0.2f, 0.2f, 0.2f));
//             value = Mathf.Clamp(GUI.HorizontalSlider(sliderRect, value, minValue, maxValue), minValue, maxValue);
//
//             // Min and Max as float fields (with 2 decimal precision in display)
//             minValue = EditorGUILayout.FloatField(minValue, GUILayout.Width(40));
//             maxValue = EditorGUILayout.FloatField(maxValue, GUILayout.Width(40));
//
//             EditorGUILayout.EndHorizontal();
//
//             // Return true if the value changed significantly (using small epsilon for float comparison)
//             return Mathf.Abs(value - oldValue) > 0.001f;
//         }
//
//         private void ProcessStatChange(ObjectData obj, ObjectStatType changedStat)
//         {
//             // Find the difference between current and previous value
//             float previousValue = _previousStatValues[changedStat];
//             float currentValue = GetStatValue(obj, changedStat);
//             float difference = currentValue - previousValue;
//
//             // If no change or no configurations, nothing to do
//             if (Mathf.Abs(difference) < 0.001f || obj.StatConfig.Count == 0)
//                 return;
//
//             // Check if the stat is increasing
//             bool isIncreasing = difference > 0;
//
//             // Process stat configurations where the changed stat is the primary stat
//             foreach (var config in obj.StatConfig)
//             {
//                 if (config.PrimaryStat == changedStat)
//                 {
//                     // If any related stat is at minimum and we're trying to increase the primary stat,
//                     // revert the change and exit
//                     bool anyRelatedStatAtMin = false;
//                     foreach (var relatedStat in config.RelatedStats)
//                     {
//                         if (IsStatAtMinimum(obj, relatedStat))
//                         {
//                             anyRelatedStatAtMin = true;
//                             break;
//                         }
//                     }
//
//                     if (isIncreasing && anyRelatedStatAtMin)
//                     {
//                         // Revert the primary stat change
//                         AdjustStatValue(obj, changedStat, -difference);
//                         return;
//                     }
//
//                     // This primary stat changed, so adjust all related stats
//                     foreach (var relatedStat in config.RelatedStats)
//                     {
//                         // Calculate the inverse effect - when primary increases, related decreases
//                         float relatedStatDelta = -difference;
//
//                         // Apply the change to the related stat
//                         AdjustStatValue(obj, relatedStat, relatedStatDelta);
//                     }
//                 }
//             }
//         }
//
//         private bool IsStatAtMinimum(ObjectData obj, ObjectStatType statType)
//         {
//             switch (statType)
//             {
//                 case ObjectStatType.HP: return obj.Hp <= obj.HpMin;
//                 case ObjectStatType.SIZE: return obj.Size <= obj.SizeMin;
//                 default: return false;
//             }
//         }
//
//         private float GetStatValue(ObjectData obj, ObjectStatType statType)
//         {
//             switch (statType)
//             {
//                 case ObjectStatType.HP: return obj.Hp;
//                 case ObjectStatType.SIZE: return obj.Size;
//                 default: return 0;
//             }
//         }
//
//         private void AdjustStatValue(ObjectData obj, ObjectStatType statType, float delta)
//         {
//             switch (statType)
//             {
//                 case ObjectStatType.HP:
//                     obj.Hp = Mathf.Clamp(Mathf.RoundToInt(obj.Hp + delta), obj.HpMin, obj.HpMax);
//                     _previousStatValues[ObjectStatType.HP] = obj.Hp;
//                     break;
//                 case ObjectStatType.SIZE:
//                     obj.Size = Mathf.Clamp(obj.Size + delta, obj.SizeMin, obj.SizeMax);
//                     _previousStatValues[ObjectStatType.SIZE] = obj.Size;
//                     break;
//             }
//         }
//
//         private void DrawStatConfigurations(ObjectData obj)
//         {
//             // Draw each stat configuration row
//             for (int i = 0; i < obj.StatConfig.Count; i++)
//             {
//                 EditorGUILayout.BeginHorizontal();
//
//                 // Draw the green stat dropdown
//                 ObjectStatConfiguration statConfig = obj.StatConfig[i];
//                 ObjectStatType oldPrimaryStat = statConfig.PrimaryStat;
//                 statConfig.PrimaryStat =
//                     (ObjectStatType)EditorGUILayout.EnumPopup(statConfig.PrimaryStat, GUILayout.Width(80));
//
//                 // Draw each red stat in this row
//                 for (int j = 0; j < statConfig.RelatedStats.Count; j++)
//                 {
//                     statConfig.RelatedStats[j] =
//                         (ObjectStatType)EditorGUILayout.EnumPopup(statConfig.RelatedStats[j], GUILayout.Width(80));
//                 }
//
//                 // Add red stat button
//                 if (DrawButton("+", new Color(1f, 0.5f, 0.5f), GUILayout.Width(30)))
//                 {
//                     statConfig.RelatedStats.Add(ObjectStatType.HP);
//                 }
//
//                 // Remove this configuration row button
//                 if (DrawButton("×", Color.red, GUILayout.Width(20)) && obj.StatConfig.Count > 1)
//                 {
//                     obj.StatConfig.RemoveAt(i);
//                     i--; // Adjust index after removal
//                     continue;
//                 }
//
//                 EditorGUILayout.EndHorizontal();
//                 GUILayout.Space(5);
//             }
//
//             // Add a new green stat row button
//             EditorGUILayout.BeginHorizontal();
//             if (DrawButton("+", Color.green, GUILayout.Width(80)))
//             {
//                 obj.StatConfig.Add(new ObjectStatConfiguration());
//             }
//
//             EditorGUILayout.EndHorizontal();
//         }
//
//         private bool DrawButton(string label, Color color, GUILayoutOption layout)
//         {
//             Color oldColor = GUI.backgroundColor;
//             GUI.backgroundColor = color;
//             bool clicked = GUILayout.Button(label, layout, GUILayout.Height(20));
//             GUI.backgroundColor = oldColor;
//             return clicked;
//         }
//
//         // === JSON Saving and Loading ===
//         private void SaveObjectData()
//         {
//             // Create directory if it doesn't exist
//             string directory = Path.GetDirectoryName(SaveFilePath);
//             if (!Directory.Exists(directory))
//             {
//                 Directory.CreateDirectory(directory);
//             }
//
//             // Serialize the object data to JSON
//             ObjectsData saveData = new ObjectsData(_objects);
//             string json = JsonUtility.ToJson(saveData, true);
//
//             // Write to file
//             File.WriteAllText(SaveFilePath, json);
//
//             // Refresh the asset database to show the file in the editor
//             AssetDatabase.Refresh();
//         }
//
//         private bool LoadObjectData()
//         {
//             try
//             {
//                 if (File.Exists(SaveFilePath))
//                 {
//                     string json = File.ReadAllText(SaveFilePath);
//                     ObjectsData loadedData = JsonUtility.FromJson<ObjectsData>(json);
//
//                     if (loadedData != null && loadedData.Objects != null && loadedData.Objects.Count > 0)
//                     {
//                         _objects = loadedData.Objects;
//                         _selectedMonsterIndex = Mathf.Clamp(_selectedMonsterIndex, 0, _objects.Count - 1);
//                         return true;
//                     }
//                 }
//
//                 return false;
//             }
//             catch (System.Exception e)
//             {
//                 Debug.LogError("Error loading object data: " + e.Message);
//                 return false;
//             }
//         }
//     }
//     
//     // Define the ObjectsData class for JSON serialization
//     [System.Serializable]
//     public class ObjectsData
//     {
//         public List<ObjectData> Objects;
//         
//         public ObjectsData(List<ObjectData> objects)
//         {
//             Objects = objects;
//         }
//     }
// }