using UnityEngine;

namespace LFramework
{
    public static class UnityEngineColorExtension
    {
        /// <summary>
        /// HTML string(#000000) 转 Color
        /// var color = ""#C5563CFF"".HtmlStringToColor();
        /// Debug.Log(color)
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static Color HtmlStringToColor(this string htmlString)
        {
            var parseSucceed = ColorUtility.TryParseHtmlString(htmlString, out var retColor);
            return parseSucceed ? retColor : Color.black;
        }
    }
}