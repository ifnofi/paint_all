using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    public class SetScriptingDefine
    {
        public static void Set()
        {
            // 补充宏定义
            var currentTarget = EditorUserBuildSettings.selectedBuildTargetGroup;

            if (currentTarget == BuildTargetGroup.Unknown) return;
            Debug.Log("检查宏定义... DG_Installed");
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            if (definesString.Contains("DG_Installed"))
            {
                Debug.Log("宏定义 DG_Installed 存在");
                return;
            }

            Debug.Log("宏定义 DG_Installed 不存在");
            definesString += ";DG_Installed";
            Debug.Log("宏定义 DG_Installed 已添加");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
        }

        /// <summary>
        /// 编译结束之后出发
        /// </summary>
        [UnityEditor.Callbacks.DidReloadScripts]
        public static void DidReloadScripts()
        {
            Set();
        }
    }
}