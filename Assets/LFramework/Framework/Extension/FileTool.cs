using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UniTask_Installed
using Cysharp.Threading.Tasks;
#endif
using UnityEngine;
using UnityEngine.Networking;

namespace LFramework
{
    public static class FileTool
    {
        public static void SaveFile(byte[] fileData, string filePath)
        {
            if (!Directory.Exists(filePath.GetDirectoryName()))
            {
                Directory.CreateDirectory(filePath.GetDirectoryName());
            }

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            File.WriteAllBytes(filePath, fileData);
        }

        /// <summary>
        /// 获得路径下的所有图片路径  jpg png jpeg 三种类型的
        /// </summary>
        /// <param name="path">要判断的路径</param>
        /// <param name="whenNullCreate">当路径不存在是是否自动创建</param>
        /// <returns></returns>
        public static List<string> GetImagePathsByDirPath(this string path, bool whenNullCreate = false)
        {
            var isExist = path.IsExistDirectory();
            if (whenNullCreate && !isExist)
            {
                path.CreateDirectoryByPath();
            }

            return path.GetFilePathsByDirPath(".png", ".jpg", ".jpeg");
        }

        /// <summary>
        /// 获得路径下的所有视频路径  mp4 m4v avi 三种类型的
        /// </summary>
        /// <param name="path">要判断的路径</param>
        /// <param name="whenNullCreate">当路径不存在是是否自动创建</param>
        /// <returns></returns>
        public static List<string> GetVideoPathsByDirPath(this string path, bool whenNullCreate = false)
        {
            var isExist = path.IsExistDirectory();
            if (whenNullCreate && !isExist)
            {
                path.CreateDirectoryByPath();
            }

            return path.GetFilePathsByDirPath(".mp4", ".m4v", ".avi");
        }

        /// <summary>
        /// 获得路径下的所有指定文件
        /// </summary>
        /// <param name="path">要判断的路径</param>
        /// <param name="end1">以 end1 为结尾的</param>
        /// <param name="endwith">可变参数</param>
        /// <returns></returns>
        public static List<string> GetFilePathsByDirPath(this string path, string end1, params string[] endwith)
        {
            var files = Directory.GetFiles(path).Where
            (
                x =>
                {
                    var conform = x.ToLower().EndsWith(end1);
                    foreach (var s in endwith)
                    {
                        conform = x.ToLower().EndsWith(s) || conform;
                    }

                    return conform;
                }
            ).ToList();

            return files;
        }

        public static List<string> GetAllDir(this string path)
        {
            return Directory.GetDirectories(path).ToList();
        }


        public static IEnumerator GetTexture2D(string url, Action<Texture2D, byte[]> onGet)
        {
            if (url.StartsWith("/storage"))
            {
                url = "file://" + url;
            }

            var www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("加载:" + url);
                var t2d = ((DownloadHandlerTexture)www.downloadHandler).texture;
                onGet?.Invoke(t2d, t2d.EncodeToPNG());
            }
        }

        public static IEnumerator GetAudioClip(string url, Action<AudioClip> onGet)
        {
            var isMp3 = url.ToLower().EndsWith(".mp3");
            if (url.StartsWith("/storage"))
            {
                url = "file://" + url;
            }

            var www = UnityWebRequestMultimedia.GetAudioClip(url, isMp3 ? AudioType.MPEG : AudioType.WAV);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("加载:" + url);
                onGet?.Invoke(((DownloadHandlerAudioClip)www.downloadHandler).audioClip);
            }
        }


#if UniTask_Installed
        public static async UniTask<Texture2D> GetTexture2DAsync(string url)
        {
            if (url.StartsWith("/storage"))
            {
                url = "file://" + url;
            }

            var www = UnityWebRequestTexture.GetTexture(url);
            await www.SendWebRequest().ToUniTask();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                return null;
            }

            Debug.Log("加载:" + url);
            return ((DownloadHandlerTexture)www.downloadHandler).texture;
        }

        public static async UniTask<AudioClip> GetAudioClipAsync(string url)
        {
            var isMp3 = url.ToLower().EndsWith(".mp3");
            if (url.StartsWith("/storage"))
            {
                url = "file://" + url;
            }

            var www = UnityWebRequestMultimedia.GetAudioClip(url, isMp3 ? AudioType.MPEG : AudioType.WAV);
            await www.SendWebRequest().ToUniTask();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                return null;
            }

            Debug.Log("加载:" + url);
            return ((DownloadHandlerAudioClip)www.downloadHandler).audioClip;
        }
#endif
    }
}