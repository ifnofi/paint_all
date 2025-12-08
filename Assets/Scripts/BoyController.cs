using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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

    private AudioListener audioListener;

    private void Awake()
    {
        // helloAudioClip =Resources.Load<AudioClip>("哈喽");
        boxCollider = canvasGroup.GetComponentInChildren<BoxCollider>();
        audioListener = GetComponentInChildren<AudioListener>();
        if (audioListener != null)
        {
            audioListener.enabled = false;
        }
        else
        {
            Debug.LogWarning("AudioListener component not found in children.");
        }

        canvasGroup.alpha = 0;
    }

    BoxCollider boxCollider;

    public void Init()
    {
        print(name);
        boyAnimator.transform.GetComponent<RectTransform>().localPosition =
            pathPoints[0].localPosition;
        boyAnimator.transform.GetComponent<RectTransform>().localRotation = 
            pathPoints[0].localRotation;
        boyAnimator.transform.GetComponent<RectTransform>().localScale =
            pathPoints[0].localScale;
        boyAnimator.transform.GetComponent<CanvasGroup>().alpha = 0;
        audioListener.enabled = false;
        boxCollider.enabled = false;
        audioSource.Stop();
    }

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

    // 
    public string currentAnName = "Walk";

    public void Pause()
    {
        audioSource.Pause();
        // if (sequence != null && sequence.IsPlaying())
        // {
        //     sequence.Pause();
        //     audioSource.Pause();
        //     stopWaitTimeTweener?.Pause();
        //     audioListener.enabled = false;
        //     currentAnName = boyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk") ? "Walk" :
        //         boyAnimator.GetCurrentAnimatorStateInfo(0).IsName("SayHello") ? "SayHello" : "Talk";
        //
        //     boyAnimator.Play("Talk", 0);
        // }
    }

    public void UnPause()
    {
        audioSource.UnPause();
        // if (sequence != null && sequence.IsActive() && !sequence.IsPlaying())
        // {
        //     switch (currentAnName)
        //     {
        //         case "Walk":
        //             boyAnimator.Play("Walk", 0);
        //             break;
        //         case "SayHello":
        //             boyAnimator.Play("SayHello", 0);
        //             break;
        //         case "Talk":
        //             boyAnimator.Play("Talk", 0);
        //             break;
        //     }
        //
        //     sequence.Play();
        //     stopWaitTimeTweener?.Play();
        //     audioSource.UnPause();
        //     audioListener.enabled = true;
        // }
    }

    public float helloDuration = 1f;
    public AudioClip helloAudioClip;
    public bool startPlayHello;

    public void Play(Action callback = null)
    {
        TimeController.Kill(GetInstanceID() + "StopWaitTime");

        if (stopWaitTimeTweener != null)
        {
            stopWaitTimeTweener.Kill(true);
            stopWaitTimeTweener = null;
        }

        if (sequence != null)
        {
            sequence.Kill();
        }

        audioListener.enabled = true;
        audioSource.Pause();

        tempIsArrivePoint = false;
        sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0, 0).SetEase(Ease.Linear));
        sequence.Join(boyAnimator.transform.DOMove(pathPoints[0].position, 0).SetEase(Ease.Linear));
        if (startPlayHello)
        {
            sequence.AppendCallback(() =>
            {
                tempIsArrivePoint = false;
                boyAnimator.Play("SayHello", 0);
                audioSource.clip = helloAudioClip;
                audioSource.Play();
            });
            sequence.Join(canvasGroup.DOFade(1, 1).SetEase(Ease.Linear));
            sequence.AppendInterval(helloDuration + helloAudioClip.length);
            sequence.AppendCallback(() =>
            {
                boxCollider.enabled = true;
            });
            sequence.AppendInterval(0.1f);
            sequence.AppendCallback(() =>
            {
                if (!boyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    boyAnimator.Play("Walk", 0);
                }
            });
            sequence.Join(boyAnimator.transform.DOPath(GetPathArray(), moveUnit, PathType.CatmullRom).SetEase(Ease.Linear));
            sequence.Join(canvasGroup.DOFade(0, 1).SetEase(Ease.Linear).SetDelay(moveUnit - 1f));
            sequence.OnComplete(() =>
            {
                audioListener.enabled = false;
                callback.Invoke();
            });
        }
        else
        {
            boxCollider.enabled = true;
            canvasGroup.DOFade(1, 0.5f).SetEase(Ease.Linear);
            if (!boyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                boyAnimator.Play("Walk", 0);
            }
            // sequence.AppendCallback(() =>
            // {
            //     boyAnimator.Play("SayHello", 0);
            // });
            // sequence.Join(canvasGroup.DOFade(1, 0.5f).SetEase(Ease.Linear));
            // sequence.AppendCallback(() =>
            // {
            //     boxCollider.enabled = true;
            //     if (!boyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            //     {
            //         boyAnimator.Play("Walk", 0);
            //     }
            // });

            sequence.Join(boyAnimator.transform.DOPath(GetPathArray(), moveUnit, PathType.CatmullRom).SetEase(Ease.Linear));
            sequence.Join(canvasGroup.DOFade(0, 1).SetEase(Ease.Linear).SetDelay(moveUnit - 1f));
            sequence.OnComplete(() =>
            {
                audioListener.enabled = false;
                callback.Invoke();
            });
        }
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

    public void AddSpeed(int index)
    {
        if (sequence != null)
        {
            sequence.DOTimeScale(sequence.timeScale + index, 1f).SetEase(Ease.Linear).OnUpdate(() => { boyAnimator.speed = sequence.timeScale; });
        }

        // sequence.timeScale += 1f; boyAnimator.Play("Run", 0);
    }

    public void ReduceSpeed(int index)
    {
        if (sequence != null)
        {
            sequence.DOTimeScale(sequence.timeScale - index, 1f).SetEase(Ease.Linear).OnUpdate(() => { boyAnimator.speed = sequence.timeScale; });
            // sequence.timeScale -= 1f;
            // boyAnimator.Play("Walk", 0);
        }
    }

    TweenerCore<float, float, FloatOptions> stopWaitTimeTweener;

    public void StopWaitTime(int index)
    {
        Talk();
        if (stopWaitTimeTweener != null)
        {
            stopWaitTimeTweener.Kill(true);
        }

        stopWaitTimeTweener = TimeController.Call(index, Walk, GetInstanceID() + "StopWaitTime");
    }

    public void Walk()
    {
        sequence.Play();
        boyAnimator.Play("Walk", 0);
    }

    public void Hello()
    {
        sequence.Pause();
        boyAnimator.Play("SayHello", 0);
    }

    public void Talk()
    {
        sequence.Pause();
        boyAnimator.Play("Talk", 0);
    }
}