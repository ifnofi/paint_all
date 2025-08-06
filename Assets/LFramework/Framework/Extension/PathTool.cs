using System;
using UnityEngine;

namespace LFramework
{
    public static class PathTool
    {
        public static string Path
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.OSXEditor:
                        break;
                    case RuntimePlatform.WindowsPlayer:
                        break;
                    case RuntimePlatform.WindowsEditor:
                        break;
                    case RuntimePlatform.Android:
                        return Application.persistentDataPath;
                        break;
                    case RuntimePlatform.WebGLPlayer:
                        break;
                }

                return Application.streamingAssetsPath;
            }
        }

        public static string DataPath => Environment.CurrentDirectory;

        public static string ConfigPath => Path.Combine("Config.txt");

        /// <summary>
        /// PC 端用
        /// </summary>
        public static string ConfigDataPath => DataPath.Combine("Config.txt");

        /// <summary>
        /// 检测路径合法性
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CheckPathValidity(this string path)
        {
            if (path != "")
            {
                var regex = new System.Text.RegularExpressions.Regex(@"^([a-zA-Z]:\\)?[^\/\:\*\?\""\<\>\|\,]*$");
                var m = regex.Match(path);
                if (!m.Success)
                {
                    UnityEngine.Debug.Log("路径不合法");
                    return false;
                }

                regex = new System.Text.RegularExpressions.Regex(@"^[^\/\:\*\?\""\<\>\|\,]+$");
                m = regex.Match(path);
                if (!m.Success)
                {
                    UnityEngine.Debug.Log("请勿在文件名中包含\\ / : * ？ \" < > |等字符，请重新输入有效文件名！");
                    return false;
                }
            }

            UnityEngine.Debug.Log("路径为 空");
            return false;
        }


        /// <summary>
        /// 组合两个路径  不必考虑  /  多不多
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string Combine(this string path1, string path2)
        {
            path2 = path2.Replace('\\', '/');
            if (path2.StartsWith("/"))
            {
                path2 = path2.Substring(1);
            }

            return System.IO.Path.Combine(path1, path2);
        }

        public static bool IsExistFile(this string path)
        {
            return System.IO.File.Exists(path);
        }

        public static bool IsExistDirectory(this string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public static void CreateFileByPath(this string path)
        {
            System.IO.File.Create(path).Close();
        }

        public static void CreateDirectoryByPath(this string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        public static void DeleteFileByPath(this string path)
        {
            System.IO.File.Delete(path);
        }

        /// <summary>
        /// 删除文件夹  需保证文件夹内部为空
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteDirectoryByPath(this string path)
        {
            System.IO.Directory.Delete(path);
        }

        /// <summary>
        /// 获取文件夹名  实际用起来是获得上一级目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDirectoryName(this string path)
        {
            return System.IO.Path.GetDirectoryName(path);
        }

        /// <summary>
        /// 文件夹的路径使用 得到文件夹的名子
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        // public static string GetFileName(this string path)
        // {
        //     return System.IO.Path.GetFileName(path);
        // }

        public static string GetFileNameWithoutExtension(this string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }

        public static string GetExtension(this string path)
        {
            return System.IO.Path.GetExtension(path);
        }
    }
}