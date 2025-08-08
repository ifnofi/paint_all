using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PersonTalk
{
    public int PosNumber;
    public AudioClip TalkClip;
    public List<RectTransform> HeightLightIcons = new List<RectTransform>();
}

public class BoyAnimeMgr : SerializedMonoBehaviour
{
    public Animator boyAnimator;
    public List<PersonTalk> personTalks = new List<PersonTalk>();
    private AudioSource audioSource;


    [Header("移动")] private Transform pathPosParent;
    private RectTransform moveImg;
    private List<RectTransform> pathRect = new List<RectTransform>();
    private List<Vector3> pathPos = new List<Vector3>();

    public void Init()
    {
        moveImg.localPosition = pathRect[0].localPosition;
        moveImg.localRotation = pathRect[0].localRotation;
        moveImg.localScale = pathRect[0].localScale;
        moveImg.GetComponent<CanvasGroup>().alpha = 0;
    }


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on BoyAnimeMgr!");
            return;
        }

        pathPosParent = transform.Find("PathPos");
        moveImg = transform.Find($"{transform.name}").GetComponent<RectTransform>();
        foreach (Transform tra in pathPosParent)
        {
            pathRect.Add(tra.GetComponent<RectTransform>());
            pathPos.Add(tra.localPosition);
        }


        if (pathPosParent == null || moveImg == null)
        {
            Debug.LogError("PathPos or moveImg not found!");
            return;
        }

        Init();


        // DoMove3Init();
    }


    private void HeightLightIcon(List<RectTransform> icons)
    {
        DOTween.Kill(GetInstanceID() + "HeightLightIcon");

        foreach (var icon in icons)
        {
            if (icon != null)
            {
                if (icon.GetComponent<Image>() != null)
                {
                    icon.GetComponent<Image>().DOColor(Color.red, 0.5f).OnComplete(() =>
                    {
                        icon.GetComponent<Image>().DOColor(Color.white, 0.5f).OnComplete(() =>
                        {
                            icon.GetComponent<Image>().DOColor(Color.red, 0.5f).OnComplete(() => { icon.GetComponent<Image>().DOColor(Color.white, 0.5f); });
                        });
                    }).SetId(GetInstanceID() + "HeightLightIcon");
                }
            }
        }
    }

    private void SetSoundClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        // TimeController.Call(audioSource.clip.length, () =>
        // {
        //     audioSource.Stop();
        //     boyAnimator.Play("Walk", 0);
        //     sequence.Play();
        // }, GetInstanceID() + "SetSoundClip");
    }


    private int move3index = 0;

    Sequence sequence = DOTween.Sequence();

    public float move3Unit = 15;
    // public float move3RotateUnit = 15;
    // public float move3ScaleUnit = 15;

    public void DoMove3Init(Action callback = null)
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        moveImg.localPosition = pathPosParent.GetChild(0).localPosition;


        // moveImg.localRotation = pathPosParent.GetChild(0).localRotation;
        // moveImg.localScale = pathPosParent.GetChild(0).localScale;
        moveImg.GetComponent<CanvasGroup>().alpha = 0;

        for (int i = 1; i < pathPos.Count; i++)
        {
            var index = i;
            var targetObj = pathPosParent.GetChild(i);
            var targetPos = targetObj.GetComponent<RectTransform>().localPosition;
            // var targetRot = targetObj.GetComponent<RectTransform>().localRotation;
            // var targetScale = targetObj.GetComponent<RectTransform>().localScale;
            // sequence.AppendInterval(0.1f);
            sequence.Append(moveImg.DOLocalMove(targetPos, move3Unit).SetSpeedBased(true).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    var component = personTalks.FindAll(x => x.PosNumber == index).First();
                    if (component != null)
                    {
                        boyAnimator.Play("Talk", 0);
                        sequence.Pause();
                        SetSoundClip(component.TalkClip);
                        HeightLightIcon(component.HeightLightIcons);
                    }
                }));
            // sequence.Join(moveImg.DOLocalRotateQuaternion(targetRot, move3RotateUnit).SetSpeedBased(true).SetEase(Ease.Linear));
            // sequence.Join(moveImg.DOScale(targetScale, move3ScaleUnit).SetSpeedBased(true).SetEase(Ease.Linear));

            if (i == pathPos.Count - 1)
            {
                sequence.Join(moveImg.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => { callback?.Invoke(); }).SetEase(Ease.Linear));
            }
        }


        sequence.Play().SetEase(Ease.Linear);
        moveImg.GetComponent<CanvasGroup>().DOFade(1, 1f).SetEase(Ease.Linear);

        var data = personTalks[0];
        if (data.PosNumber == 0)
        {
            boyAnimator.Play("SayHello", 0);
            sequence.Pause();
            TimeController.Call(1f, () =>
            {
                audioSource.Stop();
                boyAnimator.Play("Walk", 0);
                sequence.Play();
            }, GetInstanceID() + "SayHello");
        }
        else if (data != null && data.PosNumber == 0)
        {
            SetSoundClip(data.TalkClip);
            // HeightLightIcon(data.HeightLightIcons);
            // boyAnimator.Play("Talk", 0);
            // sequence.Pause();
        }
        else
        {
            boyAnimator.Play("Walk", 0);
        }
    }
}