using System;
using System.Collections.Generic;
using DG.Tweening;
using LFramework;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class StoneControl : MonoSingleton<StoneControl>
{
    public ParticleSystem stone;
    public ParticleSystem light;

    public Sequence sequence;

    public List<Transform> targets = new List<Transform>();
    private Vector3[] targetPositions = Array.Empty<Vector3>();

    private void Start()
    {
        targetPositions = new Vector3[targets.Count];
        foreach (var trans in targets)
        {
            targetPositions[targets.IndexOf(trans)] = trans.position;
        }
        stone.Stop(true);
        light.Stop(true);
        stone.transform.DOScale(Vector3.zero, 0);
        stone.transform.position = targetPositions[0];
        stone.transform.GetChild(0).GetChild(1).Show();
    }

    public Sequence Play(TweenCallback callback = null)
    {
        stone.Stop(true);
        light.Stop(true);
        sequence.Kill();
        sequence = DOTween.Sequence();
        stone.transform.DOScale(Vector3.zero, 0);
        stone.transform.position = targetPositions[0];
        stone.transform.GetChild(0).GetChild(1).Show();

        sequence.Append(stone.transform.DOScale(Vector3.one, 1f).SetEase(Ease.Linear));
        sequence.AppendCallback(() =>
        {
            stone.Play(true);
            light.Stop(true);
        });
        sequence.AppendInterval(3f);
        sequence.AppendCallback(() =>
        {
            stone.transform.GetChild(0).GetChild(1).Hide();
        });
        sequence.Append(stone.transform.DOPath(targetPositions, 2, PathType.CatmullRom).SetEase(Ease.Linear));
        sequence.Append(stone.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.Linear));
        sequence.AppendCallback(() =>
        {
            stone.Stop(true);
            light.Play(true);
        });
        sequence.AppendInterval(2f);
        sequence.AppendCallback(callback);
        sequence.AppendInterval(3f);
        sequence.AppendCallback(() =>
        {
            light.Stop(true);
        });


        return sequence;
    }
}
