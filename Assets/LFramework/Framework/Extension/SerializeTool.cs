using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace LFramework
{
    public static class SerializeTool
    {
        /// <summary>
        /// 序列化 以 T  为类型
        /// </summary>
        /// <typeparam name="T">序列化的类型</typeparam>
        /// <param name="obj">传递过来的参数</param>
        /// <returns>序列化之后的</returns>
        public static byte[] Serialize<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                try
                {
                    //序列化
                    var bf = new BinaryFormatter();
                    bf.Serialize(ms, obj);
                    ms.Seek(0, SeekOrigin.Begin);
                    return Compress(ms.ToArray());
                }
                catch (SerializationException e)
                {
                    UnityEngine.Debug.LogError("Failed to serialize. Reason: " + e.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(this byte[] bytes)
        {
            using (var ms = new MemoryStream(DeCompress(bytes)))
            {
                try
                {
                    var bf = new BinaryFormatter();
                    var msg = (T)bf.Deserialize(ms);
                    return msg;
                }
                catch (SerializationException e)
                {
                    UnityEngine.Debug.LogError("Failed to deserialize. Reason: " + e.Message + " bytesLen:" + bytes.Length);
                    return default;
                }
            }
        }

        /// <summary>
        /// 数据压缩
        /// </summary>
        /// <param name="input">压缩源</param>
        /// <returns>压缩后</returns>
        private static byte[] Compress(byte[] input)
        {
            using (var outMS = new MemoryStream())
            {
                using (var gzs = new GZipStream(outMS, CompressionMode.Compress, true))
                {
                    gzs.Write(input, 0, input.Length);
                    gzs.Close();
                    return outMS.ToArray();
                }
            }
        }

        /// <summary>
        /// 数据解压
        /// </summary>
        /// <param name="input">解压源</param>
        /// <returns>解压后</returns>
        private static byte[] DeCompress(byte[] input)
        {
            using (var inputMS = new MemoryStream(input))
            {
                using (var outMS = new MemoryStream())
                {
                    using (var gzs = new GZipStream(inputMS, CompressionMode.Decompress))
                    {
                        byte[] bytes = new byte[1024];
                        int len = 0;
                        while ((len = gzs.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            outMS.Write(bytes, 0, len);
                        }

                        //高版本可用
                        // gzs.CopyTo(outMS);
                        gzs.Close();
                        return outMS.ToArray();
                    }
                }
            }
        }
    }
}