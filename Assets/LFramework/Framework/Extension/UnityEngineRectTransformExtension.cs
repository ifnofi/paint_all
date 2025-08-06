using UnityEngine;

namespace LFramework
{
    public static class UnityEngineRectTransformExtension 
    {
        public static RectTransform AnchoredPositionY(this RectTransform selfRectTrans, float anchoredPositionY)
        {
            var anchorPos = selfRectTrans.anchoredPosition;
            anchorPos.y = anchoredPositionY;
            selfRectTrans.anchoredPosition = anchorPos;
            return selfRectTrans;
        }
        
        public static RectTransform AnchoredPositionX(this RectTransform selfRectTrans, float anchoredPositionX)
        {
            var anchorPos = selfRectTrans.anchoredPosition;
            anchorPos.x = anchoredPositionX;
            selfRectTrans.anchoredPosition = anchorPos;
            return selfRectTrans;
        }
    }
}