using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

namespace LFramework
{
    public static partial class OtherExtension
    {
        /// <summary>
        /// 重映射 当前数值 到 新的区间
        /// </summary>
        /// <param name="oXY">当前要映射的值</param>
        /// <param name="oMin">老区间</param>
        /// <param name="oMax">老区间</param>
        /// <param name="nMin">新区间</param>
        /// <param name="nMax">新区间</param>
        /// <returns>新的映射值</returns>
        public static float ReMapping(this float oXY, float oMin, float oMax, float nMin, float nMax)
        {
            oXY = Mathf.Min(oXY, oMax);
            return (nMax - nMin) / (oMax - oMin) * (oXY - oMin) + nMin;
        }


        public static bool IsEmail(this string email)
        {
            Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");

            //w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样  	
            return RegEmail.Match(email).Success;
        }

        public static string RemoveAllDigits(this string input)
        {
            // 使用正则表达式移除所有数字
            string output = Regex.Replace(input, @"\d", "");
            return output;
        }
    }
}