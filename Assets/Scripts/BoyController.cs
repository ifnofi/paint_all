using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LFramework;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class BoyController : MonoBehaviour
{
    public Animator boyAnimator;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public CanvasGroup canvasGroup;
    public AudioSource audioSource;

    public List<Transform> pathPoints = new List<Transform>();

    private bool tempIsSpeakOver;
    private bool tempIsArrivePoint;

    public void Speak(int index)
    {
        TimeController.Kill(GetInstanceID() + "Speak");
        tempIsSpeakOver = false;
        audioSource.clip = audioClips[index];
        audioSource.Play();
        TimeController.Call(audioSource.clip.length,
            () =>
            {
                tempIsSpeakOver = true;
                if (tempIsArrivePoint)
                {
                    sequence.Play();
                    boyAnimator.Play("Walk", 0);
                    tempIsArrivePoint = false;
                }
            },
            GetInstanceID() + "Speak");
    }

    Sequence sequence;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }
    }

    private void Play()
    {
        if (sequence != null)
        {
            sequence.Kill();
        }

        audioSource.Pause();

        sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0, 0).SetEase(Ease.Linear));
        sequence.Join(boyAnimator.transform.DOMove(pathPoints[0].position, 0).SetEase(Ease.Linear));

        sequence.AppendCallback(() =>
        {
            tempIsArrivePoint = false;
            boyAnimator.Play("SayHello", 0);
        });
        sequence.Join(canvasGroup.DOFade(1, 1).SetEase(Ease.Linear));
        sequence.AppendInterval(1f);

        sequence.AppendCallback(() =>
        {
            boyAnimator.Play("Walk", 0);
        });
        sequence.Join(boyAnimator.transform.DOPath(GetPathArray(), moveUnit, PathType.CatmullRom).SetEase(Ease.Linear));
        sequence.Join(canvasGroup.DOFade(0, 1).SetEase(Ease.Linear).SetDelay(moveUnit - 1f));
    }

    public float moveUnit = 10f;

    private Vector3[] GetPathArray()
    {
        if (pathPoints.Count == 0)
        {
            throw new System.InvalidOperationException("Path points list is empty. Please initialize path points before calling GetPathArray.");
        }

        Vector3[] pathArray = new Vector3[pathPoints.Count];
        for (int i = 0; i < pathPoints.Count; i++)
        {
            pathArray[i] = pathPoints[i].position;
        }

        return pathArray;
    }

    public void ArrivePoint()
    {
        // 还没播放完成
        if (!tempIsSpeakOver)
        {
            tempIsArrivePoint = true;
            // 等待
            sequence.Pause();
            // 播放说话动画
            boyAnimator.Play("Talk", 0);
        }
    }

    public void AddSpeed()
    {
        if (sequence != null)
        {
            sequence.timeScale += 1f;
        }
    }

    public void ReduceSpeed()
    {
        if (sequence != null)
        {
            sequence.timeScale -= 1f;
        }
    }
}