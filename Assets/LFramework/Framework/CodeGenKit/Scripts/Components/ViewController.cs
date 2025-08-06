using System;
using System.Collections;
#if DG_Installed
using DG.Tweening;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace LFramework
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ViewController : MonoBehaviour, IBindGroup
    {
        [HideInInspector] public string Namespace = string.Empty;

        [HideInInspector] public string ScriptName;

        [HideInInspector] public string ScriptsFolder = string.Empty;

        [HideInInspector] public bool GeneratePrefab = false;

        [HideInInspector] public string PrefabFolder = string.Empty;

        [HideInInspector] public string ArchitectureFullTypeName = string.Empty;

        [HideInInspector] public string ViewControllerFullTypeName = string.Empty;

        public string TemplateName => nameof(ViewController);

        [Tooltip("是否在打开面板后启用遮罩,以防止连续点击 显示很怪")] public bool enableRaycastMask;
        protected CanvasGroup _canvasGroup;
        protected Image mask;
        protected RectTransform rect;

        protected delegate void OnOpenDelegate();

        protected delegate void OnClosedDelegate();

        /// <summary>
        /// 面板打开时
        /// </summary>
        protected event OnOpenDelegate OnOpenEvent;

        /// <summary>
        /// 面板关闭时
        /// </summary>
        protected event OnClosedDelegate OnCloseEvent;

        public virtual IEnumerator InitPanel()
        {
            rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            if (!transform.Find("raycastMask"))
            {
                mask = new GameObject("raycastMask").AddComponent<Image>();
                mask.transform.SetParent(transform);
                // 设置锚点，使其填充整个父容器
                mask.rectTransform.anchorMin = Vector2.zero; // 左下角
                mask.rectTransform.anchorMax = Vector2.one; // 右上角

                // 设置偏移量为零
                mask.rectTransform.offsetMin = Vector2.zero; // 左下偏移
                mask.rectTransform.offsetMax = Vector2.zero; // 右上偏移

                // 可选：设置缩放为 1
                mask.rectTransform.localScale = Vector3.one;
                mask.color = Color.clear;
                mask.Hide();
            }

            yield return new WaitForEndOfFrame();
        }
        public virtual void Open(float duration =0.5f,float delay =0f)
        {
            if (enableRaycastMask)
            {
                mask.Show();
            }

            _canvasGroup.alpha = 0;
            gameObject.Show();
#if DG_Installed
            _canvasGroup.DOFade(1, duration).SetDelay(delay).OnComplete(()=>mask.Hide());
#else
            _canvasGroup.alpha = 1;
#endif
            OnOpenEvent?.Invoke();
        }

        public virtual void Close(float duration =0.5f,float delay =0f)
        {
            if (enableRaycastMask)
            {
                mask.Show();
            }

            OnCloseEvent?.Invoke();
#if DG_Installed
            _canvasGroup.DOFade(0, duration).SetDelay(delay).OnComplete
            (() =>
                {
                    gameObject.Hide();
                    mask.Hide();
                }
            );
#else
            _canvasGroup.alpha = 0;
            gameObject.Hide();
            mask.Hide();
#endif
        }
    }

    public class ViewControllerChildAttribute : Attribute
    {
    }
}