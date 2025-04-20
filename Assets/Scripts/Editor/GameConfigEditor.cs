using GameDefine;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameConfig))]
    public class GameConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor")) GameSettingWindow.ShowWindow();
        }
    }
}