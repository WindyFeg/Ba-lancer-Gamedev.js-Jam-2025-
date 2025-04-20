using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameDefine
{
    public abstract class SingletonScriptObject<T> : ScriptableObject where T : ScriptableObject
    {
#if UNITY_EDITOR


        private SerializedObject _s;
        public SerializedObject SerializedObject { get { if (_s == null) _s = new SerializedObject(Ins); return _s; } }


#endif


        private static T _instance = null;
        public static T Ins
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.Load($"GameAssets/{typeof(T).Name}") as T;
#if UNITY_EDITOR
                    if (!_instance) CreateAsset();
#endif
                    if (_instance) CallInitMethod();
                }
                return _instance;
            }
        }


        public static void Reload()
        {
            _instance = Resources.Load($"GameAssets/{typeof(T).Name}") as T;
            if (_instance) CallInitMethod();
        }


        private static void CallInitMethod()
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            _instance.GetType().GetMethod("Init", flag)?.Invoke(_instance, null);
        }


#if UNITY_EDITOR
        private const string RESOURCE_DIR = "Assets/_Game/Resources/GameAssets/";
        public static void CreateAsset()
        {
            if (!Directory.Exists(RESOURCE_DIR))
                Directory.CreateDirectory(RESOURCE_DIR);


            var filepath = RESOURCE_DIR + typeof(T).Name + ".asset";
            if (File.Exists(filepath))
            {
                try
                {
                    EditorUtility.FocusProjectWindow();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error: {e}");
                }
                T _instance = Resources.Load($"GameAssets/{typeof(T).Name}") as T;
                Selection.activeObject = _instance;
                return;
            }


            if (!_instance)
            {
                ScriptableObject asset = CreateInstance(typeof(T));
                AssetDatabase.CreateAsset(asset, filepath);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;


                Reload();
            }
        }
#endif
    }
}



