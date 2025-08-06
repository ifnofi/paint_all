using DG.Tweening;
using LFramework;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class TweenAn : MonoBehaviour
{
    [Header("目标")] public Transform moveTarget;
    [Header("初始")] public Transform moveStart;
    public float moveSpeed = 1.0f;

    public bool isMove = true;
    public bool isSizeDelta = false;
    public bool isScale = false;
    public bool isRotate = false;
    public bool isAlpha = false;
    public Ease ease = Ease.InOutQuad;

    public bool isAnimationCurve = false;

    public AnimationCurve animationCurve;
    public float delay = 0;

    private void Awake()
    {
        moveStart = transform.parent.Find($"pos/{name}start");
        moveTarget = transform.parent.Find($"pos/{name}end");
    }

    private void OnEnable()
    {
        // if (GameManager.Instance.canTween)
        {
            Play();
        }
    }

    private void Play()
    {
        DOTween.Kill(GetInstanceID());
        if (moveStart == null)
            return;

        if (isMove)
        {
            transform.localPosition = moveStart.localPosition;
            if (isAnimationCurve)
            {
                transform.DOLocalMove(moveTarget.localPosition, moveSpeed).SetEase(animationCurve).SetDelay(delay).SetId(GetInstanceID());
            }
            else
            {
                transform.DOLocalMove(moveTarget.localPosition, moveSpeed).SetEase(ease).SetDelay(delay).SetId(GetInstanceID());
            }
        }

        if (isScale)
        {
            transform.localScale = moveStart.localScale;

            if (isAnimationCurve)
            {
                transform.DOScale(moveTarget.localScale, moveSpeed).SetEase(animationCurve).SetDelay(delay).SetId(GetInstanceID());
            }
            else
            {
                transform.DOScale(moveTarget.localScale, moveSpeed).SetEase(ease).SetDelay(delay).SetId(GetInstanceID());
            }
        }

        if (isRotate)
        {
            if (isAnimationCurve)
            {
                transform.DORotate(moveTarget.localEulerAngles, moveSpeed).SetEase(animationCurve).SetDelay(delay).SetId(GetInstanceID());
            }
            else
            {
                transform.DORotate(moveTarget.localEulerAngles, moveSpeed).SetEase(ease).SetDelay(delay).SetId(GetInstanceID());
            }
        }

        if (isAlpha)
        {
            transform.GetComponent<CanvasGroup>().alpha = moveStart.GetComponent<CanvasGroup>().alpha;
            ;
            if (isAnimationCurve)
            {
                transform.GetComponent<CanvasGroup>().DOFade(moveTarget.GetComponent<CanvasGroup>().alpha, moveSpeed).SetEase(animationCurve).SetDelay(delay)
                    .SetId(GetInstanceID());
            }
            else
            {
                transform.GetComponent<CanvasGroup>().DOFade(moveTarget.GetComponent<CanvasGroup>().alpha, moveSpeed).SetEase(ease).SetDelay(delay).SetId(GetInstanceID());
            }
        }

        if (isSizeDelta)
        {
            transform.GetComponent<RectTransform>().sizeDelta = moveStart.GetComponent<RectTransform>().sizeDelta;
            if (isAnimationCurve)
            {
                transform.GetComponent<RectTransform>().DOSizeDelta(moveTarget.GetComponent<RectTransform>().sizeDelta, moveSpeed).SetEase(animationCurve).SetDelay(delay)
                    .SetId(GetInstanceID());
            }
            else
            {
                transform.GetComponent<RectTransform>().DOSizeDelta(moveTarget.GetComponent<RectTransform>().sizeDelta, moveSpeed).SetEase(ease).SetDelay(delay)
                    .SetId(GetInstanceID());
            }
        }
    }

    private void OnDisable()
    {
        if (moveStart == null)
            return;
        if (isMove)
        {
            transform.localPosition = moveStart.localPosition;
        }

        if (isScale)
        {
            transform.localScale = moveStart.localScale;
        }

        if (isRotate)
        {
            transform.localEulerAngles = moveStart.localEulerAngles;
        }


        if (isAlpha)
        {
            transform.GetComponent<CanvasGroup>().alpha = moveStart.GetComponent<CanvasGroup>().alpha;
        }

        if (isSizeDelta)
        {
            transform.GetComponent<RectTransform>().sizeDelta = moveStart.GetComponent<RectTransform>().sizeDelta;
        }
    }
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
#endif
    public void Create()
    {
        var posTrans = transform.parent.Find($"pos");
        GameObject pos = null;
        if (!posTrans)
        {
            pos = new GameObject("pos");
            pos.gameObject.AddComponent<RectTransform>().anchoredPosition = Vector2.zero;
            pos.gameObject.AddComponent<CanvasGroup>().alpha = 0;

            pos.transform.SetParent(transform.parent);
            pos.transform.localScale = Vector3.one;
            pos.transform.localPosition = Vector3.zero;
        }
        else
        {
            pos = posTrans.gameObject;
        }

        if (!pos.transform.Find($"{name}end"))
        {
            var obj = Instantiate(this.gameObject, pos.transform, true);
            DestroyImmediate(obj.gameObject.GetComponent<TweenAn>());
            obj.name = $"{name}end";
        }

        if (!pos.transform.Find($"{name}start"))
        {
            var obj = Instantiate(this.gameObject, pos.transform, true);
            DestroyImmediate(obj.gameObject.GetComponent<TweenAn>());
            obj.name = $"{name}start";
        }
    }
}