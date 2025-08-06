using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using LFramework;
using UnityEngine;

public class PostBuildScript
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        // // 获取构建输出文件夹路径
        // string buildDirectory = Path.GetDirectoryName(pathToBuiltProject);
        //
        //
        // var files = Path.Combine(Application.dataPath, "LFramework\\Core\\Editor\\Resource").GetFilePathsByDirPath(".bat", ".vbs");
        //
        // foreach (var file in files)
        // {
        //     var fileName = file.GetFileName();
        //     var customFilePath = Path.Combine(buildDirectory, fileName);
        //     if (!customFilePath.IsExistFile())
        //     {
        //         File.Copy(file, customFilePath);
        //         Debug.Log("已创建：" + customFilePath);
        //     }
        // }
    }
}