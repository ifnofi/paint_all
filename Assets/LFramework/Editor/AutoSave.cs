/****************************************************
    文件：AutoSave.cs
    作者：XWL
    邮箱:  <2263007881@qq.com>
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LFramework
{
    [InitializeOnLoad]
    public class AutoSave
    {
        // Static constructor that gets called when unity fires up.
        static AutoSave()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                // If we're about to run the scene...
                if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
                {
                    // Save the scene and the assets.
                    Debug.Log("自动保存所有打开的场景... " + state);
                    EditorSceneManager.SaveOpenScenes();
                    AssetDatabase.SaveAssets();
                }
            };
        }
        
        /// <summary>
        /// 编译结束之后
        /// </summary>
        // [UnityEditor.Callbacks.DidReloadScripts]
        [InitializeOnLoadMethod]
        public static void AddScript()
        {
            // 补充宏定义
            var currentTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (currentTarget == BuildTargetGroup.Unknown) return;
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            // Debug.Log(definesString);
            var old = definesString;
            var dg = CheckDG();
            var tmp = CheckTMP();

            if (dg && !definesString.Contains("DG_Installed"))
            {
                definesString += ";DG_Installed";
            }

            if (tmp && !definesString.Contains(""))
            {
                definesString += ";TMP_Installed";
            }


            if (definesString.Contains("DG_Installed") && !dg)
            {
                definesString = definesString.Replace("DG_Installed", "");
            }

            if (definesString.Contains("TMP_Installed") && !tmp)
            {
                definesString = definesString.Replace("TMP_Installed", "");
            }

            // Debug.Log(definesString);
            if (old == definesString)
            {
                return;
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
        }

        private static bool CheckDG()
        {
            try
            {
                return IsExistFileInDirectoryPath("DOTweenPro.dll", Application.dataPath + "/LFramework") ||
                       IsExistFileInDirectoryPath("DOTweenPro.dll", Application.dataPath + "/Plugins");
            }
            catch (Exception e)
            {
                Debug.Log("error:" + e.Message);
            }

            return false;
        }

        private static bool CheckTMP()
        {
            try
            {
                var pa = Application.dataPath + "/../Packages/manifest.json";
                var txt = File.ReadAllText(pa);

                return txt.Contains("com.unity.textmeshpro");
            }
            catch (Exception e)
            {
                Debug.Log("error:" + e.Message);
            }

            return false;
        }


        private static bool IsExistFileInDirectoryPath(string fileNameKeepExtension, string directoryPath)
        {
            var result = false;
            var dirs = directoryPath.GetAllDir();
            if (dirs.Count > 0)
            {
                foreach (var dir in dirs)
                {
                    result = result || IsExistFileInDirectoryPath(fileNameKeepExtension, dir);
                }
            }

            return result || Directory.Exists(directoryPath) && Directory.GetFiles(directoryPath).Any(x => x.EndsWith(fileNameKeepExtension));
        }
    }
}