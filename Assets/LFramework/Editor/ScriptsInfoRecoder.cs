/************************************************************
    文件: ScriptsInfoRecoder.cs
	作者: XWL
    邮箱: 2263007881@qq.com
    日期: NULL
	功能: 记录脚本信息
*************************************************************/

using System;
using System.IO;

namespace LFramework
{
    public class ScriptsInfoRecoder : UnityEditor.AssetModificationProcessor
    {
        private static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                string str = File.ReadAllText(path);

                str = str.Replace("// Start is called before the first frame update\nvoid Start()", "private void Start()");
                str = str.Replace("// Update is called once per frame\nvoid Update()", "private void Update()");
                

                File.WriteAllText(path, str);
                // Debug.CLog(str);
            }
        }

        // private static void OnWillCreateAsset(string path)
        // {
        //     path = path.Replace(".meta", "");
        //     if (path.EndsWith(".cs"))
        //     {
        //         string str = File.ReadAllText(path);
        //         str = str.Replace("#CreateAuthor#", Environment.UserName).Replace
        //         (
        //             "#CreateTime#",
        //             string.Concat
        //             (
        //                 DateTime.Now.Year, "/", DateTime.Now.Month, "/", DateTime.Now.Day, " ", DateTime.Now.Hour, ":",
        //                 DateTime.Now.Minute, ":", DateTime.Now.Second
        //             )
        //         );
        //         File.WriteAllText(path, str);
        //     }
        // }
    }
}