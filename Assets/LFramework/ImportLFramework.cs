#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

public class ImportLFramework
{
    [MenuItem("LFramework/导入插件", false, 0)]
    private static void MenuClicked()
    {
        DownloadAndOpenFile();
    }

    private static async void DownloadAndOpenFile()
    {
        var req = UnityWebRequest.Get("http://192.168.3.106");
        var operation = req.SendWebRequest();

        while (!operation.isDone)
        {
            EditorUtility.DisplayProgressBar("LFramework", "Downloading...", req.downloadProgress);
            await Task.Delay(100);
        }

        EditorUtility.ClearProgressBar();

        #region 兼容

#if UNITY_6000_0_OR_NEWER || UNITY_6000 || UNITY_6000_0_38 || UNITY_6000_1_OR_NEWER || UNITY_6000_0
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("下载失败: " + req.error);
            return;
        }
#elif UNITY_2023_1_OR_NEWER
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("下载失败: " + req.error);
            return;
        }
#elif UNITY_2022_1_OR_NEWER
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("下载失败: " + req.error);
            return;
        }
#elif UNITY_2021_1_OR_NEWER
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("下载失败: " + req.error);
            return;
        }
#elif UNITY_2020_1_OR_NEWER
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("下载失败: " + req.error);
            return;
        }
#elif UNITY_2019_4_OR_NEWER
        if (req.error != null)
        {
            Debug.LogError("下载失败: " + req.error);
            return;
        }
#endif

        #endregion

        var data = req.downloadHandler.data;
        var fileName = req.GetResponseHeader("Content-Disposition");
        fileName = ParseFileNameFromContentDisposition(fileName) ?? "LFrameworkTempFile";
        Debug.Log(fileName);
        var tempPath = Path.Combine(Application.temporaryCachePath, fileName);
        File.WriteAllBytes(tempPath, data);

        try
        {
            Process.Start(tempPath);
        }
        catch (Exception e)
        {
            Debug.LogError("无法打开文件: " + e.Message);
        }
    }

    private static string ParseFileNameFromContentDisposition(string contentDisposition)
    {
        const string fileNameKey = "filename=";
        if (string.IsNullOrEmpty(contentDisposition)) return null;
        int fileNameIndex = contentDisposition.IndexOf(fileNameKey, StringComparison.Ordinal);
        if (fileNameIndex >= 0)
        {
            int startIndex = fileNameIndex + fileNameKey.Length;
            string fileName = contentDisposition.Substring(startIndex).Trim('\"');
            return fileName;
        }

        return null;
    }
}
#endif