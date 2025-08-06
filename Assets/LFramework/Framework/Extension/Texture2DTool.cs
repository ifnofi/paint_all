using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace LFramework
{
    public static class Texture2DTool
    {
        public static IEnumerator GetTexture(this string path, Action<Texture2D> callback)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[fs.Length];
                var read = fs.Read(buffer, 0, buffer.Length);
                var texture = new Texture2D(2, 2);
                texture.LoadImage(buffer);
                texture.name = path.GetFileName();
                callback?.Invoke(texture);
            }

            yield break;
        }


        /// <summary>
        /// 重置分辨率   效率低一些
        /// </summary>
        public static Texture2D ScaleTexture(Texture2D source, float targetWidth, float targetHeight)
        {
            targetHeight = targetWidth * (float)source.height / (float)source.width;
            Texture2D result = new Texture2D((int)targetWidth, (int)targetHeight, source.format, false);

            float incX = (1.0f / targetWidth);
            float incY = (1.0f / targetHeight);

            for (int i = 0; i < result.height; ++i)
            {
                for (int j = 0; j < result.width; ++j)
                {
                    Color newColor = source.GetPixelBilinear
                        ((float)j / (float)result.width, (float)i / (float)result.height);
                    result.SetPixel(j, i, newColor);
                }
            }

            result.Apply();
            return result;
        }


        /// <summary>
        /// 重置大小  等比缩放  效率高一些
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Texture2D ResizeTexture(this Texture2D source, int width)
        {
            if (source.width < width)
            {
                return source;
            }

            if (source != null)
            {
                var height = (int)(width / (float)source.width * source.height);
                // 创建临时的RenderTexture
                RenderTexture renderTex = RenderTexture.GetTemporary
                    (width, height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
                // 复制source的纹理到RenderTexture里
                Graphics.Blit(source, renderTex);
                // 开启当前RenderTexture激活状态
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = renderTex;
                // 创建修改后的纹理，保持与源纹理相同压缩格式
                Texture2D resizedTexture = new Texture2D(width, height, source.format, false);
                // 读取像素到创建的纹理中
                resizedTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                // 应用修改到GPU上
                resizedTexture.Apply();
                // 停止当前RenderTexture工作
                RenderTexture.active = previous;
                // 释放内存
                RenderTexture.ReleaseTemporary(renderTex);
                return resizedTexture;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 裁切掉透明区域
        /// </summary>
        /// <param name="source">图片</param>
        /// <returns></returns>
        public static Texture2D CutTransparencyArea(this Texture2D source)
        {
            var h = source.height;
            var w = source.width;

            // 从左到右  从上到下
            var left = source.width;
            var right = source.width;
            var up = source.height;
            var down = source.height;


            for (int y = 0; y < h; y++)
            {
                var temp = 0;
                for (int x = 0; x < w; x++)
                {
                    if (source.GetPixel(x, y).a > 0)
                    {
                        break;
                    }

                    temp++;
                }

                if (temp < left)
                {
                    left = temp;
                }
            }

            for (int y = 0; y < h; y++)
            {
                var temp = 0;
                for (int x = w - 1; x > 0; x--)
                {
                    if (source.GetPixel(x, y).a > 0)
                    {
                        break;
                    }

                    temp++;
                }

                if (temp < right)
                {
                    right = temp;
                }
            }

            for (int x = 0; x < w; x++)
            {
                var temp = 0;
                for (int y = h - 1; y > 0; y--)
                {
                    if (source.GetPixel(x, y).a > 0)
                    {
                        break;
                    }

                    temp++;
                }

                if (temp < up)
                {
                    up = temp;
                }
            }

            for (int x = 0; x < w; x++)
            {
                var temp = 0;
                for (int y = 0; y < h; y++)
                {
                    if (source.GetPixel(x, y).a > 0)
                    {
                        break;
                    }

                    temp++;
                }

                if (temp < down)
                {
                    down = temp;
                }
            }

            var newH = h - up - down;
            var newW = w - left - right;
            var t2d = new Texture2D(newW, newH);
            for (int j = 0; j < newH; j++)
            {
                for (int i = 0; i < newW; i++)
                {
                    var color = source.GetPixel(i + left, j + down);
                    if (color.a > 0) { }

                    t2d.SetPixel(i, j, color);
                }
            }

            // var bytes = t2d.EncodeToPNG();
            // using (var file = new FileStream
            //     (Application.streamingAssetsPath + "1.png", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            // {
            //     file.Write(bytes, 0, bytes.Length);
            // }
            // AssetDatabase.Refresh();

            return t2d;
        }
    }
}