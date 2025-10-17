using System;
using DG.Tweening;
using LFramework;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class AnimationItem : MonoBehaviour
{
    public RectTransform mask;
    public RectTransform self;

    public float duration = 1f;

    private Sequence sequence;

    private void OnEnable()
    {
        sequence?.Kill();

        sequence = DOTween.Sequence();
        mask.sizeDelta = new Vector2(0, mask.sizeDelta.y);
        sequence.Append(mask.DOSizeDelta(new Vector2(self.sizeDelta.x, mask.sizeDelta.y), duration).SetEase(Ease.Linear));
        sequence.AppendInterval(5);
        sequence.Append(mask.DOSizeDelta(new Vector2(0, mask.sizeDelta.y), duration / 2f).SetEase(Ease.Linear));
        sequence.OnComplete(() =>
        {
            this.Hide();
        });

        sequence.Play();
    }
}
