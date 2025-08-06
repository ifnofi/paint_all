

#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LFramework
{
    [System.Serializable]
    public class OtherBind
    {
        public string MemberName;
        public Object Object;
    }

    public class OtherBindComparer : IComparer<OtherBind>
    {
        public int Compare(OtherBind a, OtherBind b)
        {
            return string.Compare(a.MemberName, b.MemberName, StringComparison.Ordinal);
        }
    }

    [RequireComponent(typeof(ViewController))]
    public class OtherBinds : MonoBehaviour
    {
        public List<OtherBind> Binds = new List<OtherBind>();
    }


}