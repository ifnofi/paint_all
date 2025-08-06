
using System.IO;
using System;
using UnityEditor;
using UnityEngine;

namespace LFramework
{
    public partial class EditorUtil
    {
        /// <summary>
        /// 打开路径的文件夹
        /// </summary>
        /// <param name="path">路径</param>
        public static void OpenInFolder(string path)
        {
            if (path[0] == '/')
            {
                Application.OpenURL("file://" + path);
            }
            else
            {
                Application.OpenURL("file:///" + path);
            }
        }

        public static void MoveTo(string fileName, string v)
        {
            File.Move(fileName,v);
        }
#if UNITY_EDITOR
        /// <summary>
        /// 导出 UnityPackage 包
        /// </summary>
        /// <param name="assetPathName">资源路径</param>
        /// <param name="packageName">包名</param>
        public static void ExportPackage(string assetPathName, string packageName)
        {
            AssetDatabase.ExportPackage(assetPathName, packageName, ExportPackageOptions.Recurse);
        }
        public static void ExportPackage(string[] assetPathNames, string packageName)
        {
            AssetDatabase.ExportPackage(assetPathNames, packageName, ExportPackageOptions.Recurse);
        }
        /// <summary>
        /// 复用 MenuItem 按钮
        /// </summary>
        /// <param name="menuItem"></param>
        public static void CallMenuItem(string menuItem)
        {
            EditorApplication.ExecuteMenuItem(menuItem);
        }
        #endif
    }
}