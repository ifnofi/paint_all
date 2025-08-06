using System;
using System.Collections;
using LFramework;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class T2dData
{
    public Texture2D t2d;
    public string path;
    public Action<byte[]> callback = null;

    public IEnumerator Load()
    {
        yield return FileTool.GetTexture2D(path, (t, datas) =>
        {
            this.t2d.LoadImage(datas);
            callback?.Invoke(datas);
        });
    }
}

public static class T2dDataTool
{
    public static T2dData CreateT2dData(this string path, Action<byte[]> callback = null)
    {
        return new T2dData() { path = path, t2d = new Texture2D(2, 2), callback = callback };
    }

    public static bool IsLoadComplete(this T2dData t2dData)
    {
        return t2dData.t2d != null && t2dData.t2d.width > 2;
    }

    public static byte[] GetData(this T2dData t2dData)
    {
        if (!t2dData.IsLoadComplete())
        {
            Debug.Log("没有加载完成:" + t2dData.path);
        }

        return t2dData.t2d.EncodeToPNG();
    }
}