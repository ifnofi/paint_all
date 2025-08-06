#if UNITY_EDITOR


using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class ImportLFramework
{
    [MenuItem("LFramework/导入插件", false, 0)]
    private static void MenuClicked()
    {
        // var url = "\\\\Ureljwchh9gr6c1\\文龙共享\\LFramework";
        // var files = Directory.GetFiles(url).ToList();
        // if (files.Count <= 0)
        // {
        //     Debug.Log("没有找到插件");
        //     return;
        // }
        //
        // files.Reverse();
        var req = UnityWebRequest.Get("http://192.168.3.106");

        req.SendWebRequest();

        while (!req.isDone)
        {
            // 等待请求完成
            EditorUtility.DisplayProgressBar("LFramework","Downloading..." , req.downloadProgress);
            // Debug.Log("Downloading..." + req.downloadProgress * 100 + "%");
        }
        
        EditorUtility.ClearProgressBar();

        var data = req.downloadHandler.data;
        var fileName = req.GetResponseHeader("Content-Disposition");
        fileName = ParseFileNameFromContentDisposition(fileName);
        Debug.Log(fileName);
        var tempPath = Path.Combine(Application.temporaryCachePath, fileName);
        using (var fileStream = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            fileStream.Write(data, 0, data.Length);
        }
        
        Process.Start(tempPath);
    }

    private static string ParseFileNameFromContentDisposition(string contentDisposition)
    {
        const string fileNameKey = "filename=";
        int fileNameIndex = contentDisposition.IndexOf(fileNameKey, StringComparison.Ordinal);
        if (fileNameIndex >= 0)
        {
            // 提取文件名
            int startIndex = fileNameIndex + fileNameKey.Length;
            string fileName = contentDisposition.Substring(startIndex).Trim('\"');
            return fileName;
        }

        return null;
    }
}

#endif