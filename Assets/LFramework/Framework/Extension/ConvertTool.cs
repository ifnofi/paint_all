using System;
using UnityEngine;

namespace LFramework
{
    public static class ConvertTool
    {
        /// <summary>
        /// float 转 int32
        /// </summary>
        /// <param name="fl"></param>
        /// <returns></returns>
        public static int ToInt(this float fl)
        {
            try
            {
                return (int)fl;
            }
            catch (Exception e)
            {
                Debug.Log("error:" + e.Message);
                return -1;
            }
        }


        /// <summary>
        /// str 转 float 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            try
            {
                return float.Parse(str);
            }
            catch (Exception e)
            {
                Debug.Log("error:" + e.Message);
                return -1;
            }
        }

        /// <summary>
        /// hex 的 string 转 int  ten
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        public static int HexToInt(this string strHex)
        {
            try
            {
                return int.Parse(strHex, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception e)
            {
                Debug.Log("error:" + e.Message);
                return -1;
            }
        }

        /// <summary>
        /// str 转 bool 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBool(this string str)
        {
            try
            {
                return str == "1" ||
                    str == "true" ||
                    str == "True" ||
                    str == "TRUE" ||
                    str == "yes" ||
                    str == "Yes" ||
                    str == "YES" ||
                    str == "on" ||
                    str == "On" ||
                    str == "ON" ||
                    str == "是";
            }
            catch (Exception e)
            {
                Debug.Log("error:" + e.Message);
                return false;
            }
        }


        /// <summary>
        /// str 转 int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception e)
            {
                Debug.Log("error:" + e.Message);
                return -1;
            }
        }

        /// <summary>
        /// str 转 double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            try
            {
                return double.Parse(str);
            }
            catch (Exception e)
            {
                Debug.Log("error:" + e.Message);
                return -1;
            }
        }
    }
}