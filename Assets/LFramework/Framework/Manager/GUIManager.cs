using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace LFramework
{
    public enum UILayer
    {
        Bg,
        Common,
        Top,
    }

    public class GUIManager
    {
        private static Dictionary<string, GameObject> _panelsDic = new Dictionary<string, GameObject>();
        
        private static GameObject _mPrivateUIRoot;

        public static GameObject UIRoot
        {
            get
            {
                if (_mPrivateUIRoot != null) return _mPrivateUIRoot;
                var uiRoot = Resources.Load<GameObject>("Prefabs/UIRoot");
                _mPrivateUIRoot = Object.Instantiate(uiRoot);
                _mPrivateUIRoot.name = "UIRoot";
                return _mPrivateUIRoot;
            }
        }
        public static GameObject LoadPanel(string panelName, UILayer layer)
        {
            var panel = Resources.Load<GameObject>("Prefabs/Panels/" + panelName);
            var panelObj = Object.Instantiate(panel);
            panelObj.name = panelName;
            switch (layer)
            {
                case UILayer.Bg:
                    panelObj.transform.SetParent(UIRoot.transform.Find("Bg"));
                    break;
                case UILayer.Common:
                    panelObj.transform.SetParent(UIRoot.transform.Find("Common"));
                    break;
                case UILayer.Top:
                    panelObj.transform.SetParent(UIRoot.transform.Find("Top"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }

            var rectTransform = panelObj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localScale = Vector3.one;

            if (_panelsDic.ContainsKey(panelName))
            {
                _panelsDic[panelName] = panelObj;
            }
            else
            {
                _panelsDic.Add(panelName, panelObj);
            }
            
            return panelObj;
        }
        
        public static void SetResolution(float width, float height, float matchWidthOrHeight)
        {
            var canvasScaler = UIRoot.GetComponent<CanvasScaler>();
            canvasScaler.referenceResolution = new Vector2(width, height);
            canvasScaler.matchWidthOrHeight = matchWidthOrHeight;
        }

        
        
        public static void UnloadPanel(string panelName)
        {
            if (!_panelsDic.ContainsKey(panelName)) return;
            Object.Destroy(_panelsDic[panelName]);
            _panelsDic.Remove(panelName);
        }
    }
}