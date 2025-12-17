using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LFramework;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class BaseController : MonoBehaviour
{
    public Animator boyAnimator;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public CanvasGroup canvasGroup;
    public AudioSource audioSource;

    public List<Transform> pathPoints = new List<Transform>();

    protected bool tempIsSpeakOver;
    protected bool tempIsArrivePoint;

    protected AudioListener audioListener;

    protected void Awake()
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

    protected BoxCollider boxCollider;

    public virtual void Init()
    {
    }

    public virtual void Speak(int index)
    {
    }

    protected Sequence sequence;

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }
    }

    // 
    public string currentAnName = "Walk";

    public virtual void Pause()
    {
    }

    public virtual void UnPause()
    {
        audioSource.UnPause();
    }

    public float helloDuration = 1f;
    public AudioClip helloAudioClip;
    public bool startPlayHello;

    public virtual void Play(Action callback = null)
    {
    }

    public virtual void HideReset()
    {
        this.Hide();
    }
    public float moveUnit = 10f;
    public float fadeOutTime = 1f;

    protected virtual Vector3[] GetPathArray()
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

    public virtual void ArrivePoint()
    {
    }

    public virtual void AddSpeed(int timeScale)
    {
    }

    public virtual void ReduceSpeed(int timeScale)
    {
    }

    protected TweenerCore<float, float, FloatOptions> stopWaitTimeTweener;
    protected IActionController actionController;
    public virtual void StopWaitTime(int waitTime)
    {
    }

    public virtual void StopButWaitTimePlayAnim(int waitTime)
    {
    }

    public virtual void StopButWaitTimePlayAnim(int waitTime, float waitToPlayAnimTime)
    {
    }
}