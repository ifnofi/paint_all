using System.IO;
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LFramework
{
    public class Exporter
    {
#if UNITY_EDITOR
        [MenuItem("LFramework/导出 UnityPackage %#e")]
        private static void MenuClicked()
        {
            var assetPathNames = new []
            {
                "Assets/LFramework",
                "Assets/Plugins",
            };
            var fileName = GeneratePackageName();
            EditorUtil.ExportPackage(assetPathNames, fileName);
            EditorUtil.MoveTo(fileName, "\\\\Ureljwchh9gr6c1/文龙共享/LFramework/files/" + fileName);
            EditorUtil.OpenInFolder("\\\\Ureljwchh9gr6c1/文龙共享/LFramework/files");
            Debug.Log("导出完成，文件名：" + fileName);
        }
        // [MenuItem("LFramework/2.导出 UnityPackage %w")]
        // private static void MenuClicked1()
        // {
        //     var assetPathName = "Assets/LFramework";
        //     var fileName = GeneratePackageName();
        //     EditorUtil.ExportPackage(assetPathName, fileName);
        //     EditorUtil.OpenInFolder(Application.dataPath + "/../");
        //     Debug.Log("导出完成，文件名：" + fileName);
        // }

        // [MenuItem("GameObject/Creat Manager", false, -20)]
        // public static void CreateGameObject()
        // {
        //     if (Object.FindObjectOfType(typeof(GameManager)))
        //     {
        //         return;
        //     }
        //
        //     GameObject go = new GameObject("GameManager");
        //     go.AddComponent<GameManager>();
        // }


        [MenuItem("LFramework/快速结构 #e")]
        private static void MenuClicked2()
        {
            var assetPathName = new[]
            {
                "Assets/Scripts", "Assets/StreamingAssets", "Assets/UI/Fonts", "Assets/Plugins", "Assets/Resources",
            };
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);
            foreach (var item in assetPathName)
            {
                Directory.CreateDirectory(item);
            }

            if (!File.Exists("Assets/StreamingAssets/Config.txt"))
            {
                File.Create("Assets/StreamingAssets/Config.txt").Close();
            }

            // if (File.Exists("Assets/LFramework/Framework/Logic/GameManager.cs"))
            // {
            //     File.Move("Assets/LFramework/Framework/Logic/GameManager.cs", "Assets/Scripts/GameManager.cs");
            // }

            AssetDatabase.Refresh();
            // var fileName = GeneratePackageName();
            // EditorUtil.ExportPackage(assetPathName, fileName);
            // EditorUtil.MoveTo(fileName, "D:/1XWL/素材/Unity资源/" + fileName);
            // EditorUtil.OpenInFolder("D:/1XWL/素材/Unity资源/");
            // Debug.Log("导出完成，文件名：" + fileName);
        }

#endif


        /// <summary>
        /// 获得 UnityPackage 包名
        /// </summary>
        /// <returns></returns>
        private static string GeneratePackageName()
        {
            return "LFramework_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".unitypackage";
        }
    }
}