using UnityEngine;
using ZXing;
using ZXing.QrCode;

namespace LFramework
{
    public static class QrTool
    {
        
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="textForEncoding"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static Color32[] Encode(string textForEncoding, int width, int height)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height, Width = width
                }
            };
            return writer.Write(textForEncoding);
        }


        /// <summary>
        /// 路径转二维码
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Texture2D creatER(this string uri)
        {
            Texture2D encoded = new Texture2D(256, 256);
            if (uri != null)
            {
                var color32 = Encode(uri, encoded.width, encoded.height);
                encoded.SetPixels32(color32);
                encoded.Apply();
                if (encoded != null)
                {
                    return encoded;
                }
                else
                {
                    Debug.Log("creatER Error");
                    return new Texture2D(1, 1);
                }
            }
            else
            {
                return new Texture2D(1, 1);
            }
        }

    }
}