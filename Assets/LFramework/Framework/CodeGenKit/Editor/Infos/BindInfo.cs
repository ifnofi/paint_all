

#if UNITY_EDITOR
using System;

namespace LFramework
{
    /// <summary>
    /// 存储一些Mark相关的信息
    /// </summary>
    [Serializable]
    public class BindInfo
    {
        public string TypeName;

        public string PathToRoot;

        public IBindOld BindScript;
        
        public string MemberName;
    }

    [Serializable]
    public class BindInfoGroup
    {
        
    }
}
#endif