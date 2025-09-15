using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class PeopleController : BaseController
{
    public override void Init()
    {
        boyAnimator.transform.GetComponent<RectTransform>().localPosition = pathPoints[0].localPosition;
        boyAnimator.transform.GetComponent<RectTransform>().localRotation = pathPoints[0].localRotation;
        boyAnimator.transform.GetComponent<RectTransform>().localScale = pathPoints[0].localScale;
        boyAnimator.transform.GetComponent<CanvasGroup>().alpha = 0;
        audioListener.enabled = false;
        boxCollider.enabled = false;
        audioSource.Stop();
    }

    private void Talk1()
    {
        sequence.Pause();
        boyAnimator.Play("Talk_Hand", 0);
    }
    public override void StopButWaitTimePlayAnim(int waitTime)
    {
        Talk1();
        if (stopWaitTimeTweener != null)
        {
            stopWaitTimeTweener.Kill(true);
        }

        stopWaitTimeTweener = TimeController.Call(waitTime, Walk, GetInstanceID() + "StopWaitTime");
    }

    public override void Speak(int index)
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }
    }

    public override void Pause()
    {
        audioSource.Pause();
    }

    public override void UnPause()
    {
        audioSource.UnPause();
    }

    public override void ArrivePoint()
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

    public override void Play(Action callback = null)
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

    public override void AddSpeed(int index)
    {
        if (sequence != null)
        {
            sequence.DOTimeScale(sequence.timeScale + index, 1f).SetEase(Ease.Linear).OnUpdate(() => { boyAnimator.speed = sequence.timeScale; });
        }

        // sequence.timeScale += 1f; boyAnimator.Play("Run", 0);
    }

    public override void ReduceSpeed(int index)
    {
        if (sequence != null)
        {
            sequence.DOTimeScale(sequence.timeScale - index, 1f).SetEase(Ease.Linear).OnUpdate(() => { boyAnimator.speed = sequence.timeScale; });
            // sequence.timeScale -= 1f;
            // boyAnimator.Play("Walk", 0);
        }
    }

    public override void StopWaitTime(int index)
    {
        Talk();
        if (stopWaitTimeTweener != null)
        {
            stopWaitTimeTweener.Kill(true);
        }

        stopWaitTimeTweener = TimeController.Call(index, Walk, GetInstanceID() + "StopWaitTime");
    }

    private void Walk()
    {
        sequence.Play();
        boyAnimator.Play("Walk", 0);
    }

    private void Hello()
    {
        sequence.Pause();
        boyAnimator.Play("SayHello", 0);
    }

    private void Talk()
    {
        sequence.Pause();
        boyAnimator.Play("Talk", 0);
    }
}