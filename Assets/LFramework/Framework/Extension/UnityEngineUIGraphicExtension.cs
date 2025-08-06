using UnityEngine.UI;

namespace LFramework
{
    public static class UnityEngineUIGraphicExtension
    {

        /// <summary>
        /// image.ColorAlpha(1.0f);
        /// rawImage.ColorAlpha(1.0f);
        /// </summary>
        /// <param name="selfGraphic"></param>
        /// <param name="alpha"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ColorAlpha<T>(this T selfGraphic, float alpha) where T : Graphic
        {
            var color = selfGraphic.color;
            color.a = alpha;
            selfGraphic.color = color;
            return selfGraphic;
        }

        public static Image FillAmount(this Image selfImage, float fillAmount)
        {
            selfImage.fillAmount = fillAmount;
            return selfImage;
        }
    }
}