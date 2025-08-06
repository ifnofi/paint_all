

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace LFramework
{
    public enum CodeGenTaskStatus
    {
        Search,
        Gen,
        Compile,
        Complete
    }

    public enum GameObjectFrom
    {
        Scene,
        Prefab
    }
    
    [System.Serializable]
    public class CodeGenTask
    {
        public bool ShowLog = false;
        
        // state
        public CodeGenTaskStatus Status;

        // input
        public GameObject GameObject;
        public GameObjectFrom From = GameObjectFrom.Scene;
        

        // search
        public List<StringPair> NameToFullName = new List<StringPair>();
        public List<BindInfo> BindInfos = new List<BindInfo>();
        
        // info
        public string ScriptsFolder;
        public string ClassName;
        public string Namespace;
        
        // result
        public string MainCode;
        public string DesignerCode;
    }

    [System.Serializable]
    public class StringPair
    {
        public StringPair(string key, string value)
        {
            Key = key;
            Value = value;
        }
        
        public string Key;
        public string Value;
    }
}
#endif