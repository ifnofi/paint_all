using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class IconMoveForPath : MonoBehaviour
{
    public MoveState moveState = MoveState.OnlyMove;
    [Header("仅位移")] public float moveUnit = 10f; // Speed of the icon movement
    [Header("位移缩放旋转")] public float speed = 10f; // Speed
    public float fadeDistance = 80f; // 距离小于此值时开始淡出

    [Header("模型")] public bool model;
    public ModelRotation modelRotation;


    private Transform pathPosParent;
    private RectTransform moveImg;
    private List<RectTransform> pathRect = new List<RectTransform>();
    private List<Vector3> pathPos = new List<Vector3>();


    private void Start()
    {
        Init();
    }

    private void Init()
    {
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


        switch (moveState)
        {
            case MoveState.OnlyMove:
                DoMove1();
                // DoMove3Init();
                break;
            case MoveState.AllChange:
                move2Coroutine = StartCoroutine(DoMove2());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private Coroutine move2Coroutine;

    /// <summary>
    /// 位移 缩放 旋转
    /// </summary>
    private IEnumerator DoMove2()
    {
        while (true)
        {
            int item = -1;

            for (int i = 0; i < pathRect.Count; i++)
            {
                if (model && item != i)
                {
                    modelRotation.SetEndPos(pathRect[i].position);
                }

                RectTransform start = pathRect[i];
                RectTransform end = (i == pathRect.Count - 1) ? pathRect[0] : pathRect[i + 1];
                // 如果是最后一个到第一个，直接赋值
                if (i == pathRect.Count - 1)
                {
                    moveImg.anchoredPosition3D = end.anchoredPosition3D;
                    moveImg.rotation = end.rotation;
                    moveImg.localScale = end.localScale;
                }
                else
                {
                    Vector3 startPosition = start.anchoredPosition3D;
                    Vector3 endPosition = end.anchoredPosition3D;
                    Quaternion startRotation = start.rotation;
                    Quaternion endRotation = end.rotation;
                    Vector3 startScale = start.localScale;
                    Vector3 endScale = end.localScale;

                    float journeyLength = Vector3.Distance(startPosition, endPosition);
                    float journeyTime = journeyLength / speed;
                    float elapsedTime = 0f;
                    while (elapsedTime < journeyTime)
                    {
                        //出现
                        var posFirst = Vector3.Distance(moveImg.anchoredPosition3D, pathRect[0].anchoredPosition3D);
                        var posEnd = Vector3.Distance(moveImg.anchoredPosition3D, pathRect[pathRect.Count - 1].anchoredPosition3D);

                        // Debug.Log("targetUI" + targetUI.anchoredPosition3D);
                        // Debug.Log("一" + waypoints[0].anchoredPosition3D);
                        // Debug.Log("二" + waypoints[waypoints.Count - 1].anchoredPosition3D);
                        // Debug.Log("posFirst" + posFirst);
                        // Debug.Log("posEnd" + posEnd);

                        if (posFirst < fadeDistance || posEnd < fadeDistance)
                        {
                            var min = Mathf.Min(posFirst, posEnd);
                            moveImg.GetComponent<CanvasGroup>().alpha = min / fadeDistance;
                        }
                        else
                        {
                            moveImg.GetComponent<CanvasGroup>().alpha = 1;
                        }


                        // 计算时间
                        elapsedTime += Time.deltaTime;
                        float t = elapsedTime / journeyTime;

                        // 线性插值
                        moveImg.anchoredPosition3D = Vector3.Lerp(startPosition, endPosition, t);
                        moveImg.rotation = Quaternion.Lerp(startRotation, endRotation, t);
                        moveImg.localScale = Vector3.Lerp(startScale, endScale, t);


                        yield return null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 只有位移
    /// </summary>
    private void DoMove1()
    {
        moveImg.localPosition = pathPosParent.GetChild(0).localPosition;
        moveImg.DOLocalPath(pathPos.ToArray(), moveUnit, PathType.CatmullRom).SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear).SetOptions(false).SetSpeedBased(true).OnUpdate(() =>
            {
                //渐隐渐现
                var posFirst = Vector3.Distance(moveImg.localPosition, pathPos[0]);
                var posEnd = Vector3.Distance(moveImg.localPosition, pathPos[pathPos.Count - 1]);
                if (posFirst < 15f || posEnd < 15f)
                {
                    var min = Mathf.Min(posFirst, posEnd);
                    moveImg.GetComponent<CanvasGroup>().alpha = min / 15;
                }
                else
                {
                    moveImg.GetComponent<CanvasGroup>().alpha = 1;
                }
            }).SetId(GetInstanceID() + "Move");


    }


    private int move3index = 0;

    private Sequence sequence;
    public float move3Unit=15;
    public float move3RotateUnit=15;
    public float move3ScaleUnit=15;
    public void DoMove3Init()
    {
         sequence = DOTween.Sequence();
         moveImg.localPosition = pathPosParent.GetChild(0).localPosition;
         moveImg.localRotation = pathPosParent.GetChild(0).localRotation;
         moveImg.localScale = pathPosParent.GetChild(0).localScale;
         moveImg.GetComponent<CanvasGroup>().DOFade(1,0.1f).SetEase(Ease.Linear);
         for (int i = 1; i < pathPos.Count; i++)
         {
             var targetObj = pathPosParent.GetChild(i);
             var targetPos = targetObj.GetComponent<RectTransform>().localPosition;
             var targetRot = targetObj.GetComponent<RectTransform>().localRotation;
             var targetScale = targetObj.GetComponent<RectTransform>().localScale;
             // sequence.AppendInterval(0.1f);
             sequence.Append(moveImg.DOLocalMove(targetPos, move3Unit).SetSpeedBased(true).SetEase(Ease.Linear)
                 .OnComplete(() =>
                 {
                     var component = targetObj.GetComponent<GameObject>();
                        if (component != null)
                        {
                            component.SetActive(true);
                        }
                 }));
             sequence.Join(moveImg.DOLocalRotateQuaternion(targetRot, move3RotateUnit).SetSpeedBased(true).SetEase(Ease.Linear));
             sequence.Join(moveImg.DOScale(targetScale, move3ScaleUnit).SetSpeedBased(true).SetEase(Ease.Linear));
             if (i == pathPos.Count - 1)
             {
                 sequence.Join(moveImg.GetComponent<CanvasGroup>().DOFade(0,1).SetEase(Ease.Linear));
             }
         }

         sequence.Play().SetLoops(-1,LoopType.Restart);
    }

    [Button]
    public void Play()
    {
        switch (moveState)
        {
            case MoveState.OnlyMove:
                DOTween.Kill(GetInstanceID() + "Move");
                DoMove1();
                break;
            case MoveState.AllChange:
                if (move2Coroutine != null)
                {
                    StopCoroutine(move2Coroutine);
                }

                move2Coroutine = StartCoroutine(DoMove2());

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}