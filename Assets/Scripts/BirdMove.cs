using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BirdMove : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public List<Vector3> pathsTemp = new List<Vector3>();
    public List<Vector3> paths = new List<Vector3>();

    private RectTransform _pathRoot;

    public RectTransform PathRoot
    {
        get
        {
            if (_pathRoot == null)
            {
                _pathRoot = transform.Find("PathPos") as RectTransform;
            }

            return _pathRoot;
        }
    }

    public float Range = 10;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        RectTransform preOne = null;

        if (paths.Count != PathRoot.transform.childCount)
        {
            UpdatePathLine();
        }

        for (int i = 0; i < PathRoot.transform.childCount; i++)
        {
            var rect = PathRoot.GetChild(i).GetComponent<RectTransform>();

            var rotateZ = rect.eulerAngles.z;

            // 根据旋转角度计算  up 和 down 方向 半径长度为 Range,也就是rect在中心店

            var up = Quaternion.Euler(0, 0, rotateZ) * Vector3.up * Range;
            var down = Quaternion.Euler(0, 0, rotateZ) * Vector3.down * Range;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(rect.position + up, rect.position + down);

            if (preOne != null)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawLine(paths[i] + PathRoot.GetChild(i).position, paths[i - 1] + PathRoot.GetChild(i - 1).position);
            }

            preOne = rect;
        }
    }

    [Button("更新路线")]
    public void Init()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (sequence != null)
            {
                sequence.Kill();
                sequence = null;
            }

            StartMove();
            return;
        }
#endif
        UpdatePathLine();
    }

    // 更新路径点
    private void UpdatePathLine()
    {
        paths.Clear();
        foreach (Transform trans in PathRoot)
        {
            var rotateZ = trans.eulerAngles.z;
            // 在up 和 down 之间随机一个点
            var randomValue = UnityEngine.Random.Range(-Range, Range);
            // 将得到的点 旋转 rotateZ 角度
            var randomDir = Quaternion.Euler(0, 0, -rotateZ) * trans.up * randomValue;
            paths.Add(randomDir);
        }
    }

    private CanvasGroup prefabCanvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        prefabCanvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
        StartMove();
    }

    private Sequence sequence;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;
    public float MoveTime = 30;

    // [Button]
    public void StartMove()
    {
        UpdatePathLine();
        sequence = DOTween.Sequence();
        sequence.SetAutoKill(true);
        // sequence.SetLoops(-1, LoopType.Restart);

        sequence.Append(prefabCanvasGroup.DOFade(0, 0));
        sequence.Join(prefabCanvasGroup.transform.DOMove(GetPosition(0), 0));

        sequence.Append(prefabCanvasGroup.DOFade(1, fadeInDuration).SetEase(Ease.Linear));
        sequence.Join(prefabCanvasGroup.transform.DOPath(GetPathArray(), MoveTime, PathType.CatmullRom).SetEase(Ease.Linear));

        sequence.Join(prefabCanvasGroup.DOFade(0, fadeOutDuration).SetEase(Ease.Linear).SetDelay(MoveTime - fadeOutDuration));

        sequence.OnComplete(StartMove);
        sequence.Play();
    }

    public Vector3[] GetPathArray()
    {
        if (paths.Count == 0)
        {
            throw new InvalidOperationException("Paths list is empty. Please initialize paths before calling GetPathArray.");
        }

        var pathArray = new List<Vector3>();
        for (int i = 0; i < paths.Count; i++)
        {
            pathArray.Add(paths[i] + PathRoot.GetChild(i).position);
        }

        pathArray.RemoveAt(0);


        return pathArray.ToArray();
    }

    public Vector3 GetPosition(int index)
    {
        if (index < 0 || index >= paths.Count)
        {
            throw new IndexOutOfRangeException("Index is out of range of the paths list.");
        }

        return paths[index] + PathRoot.GetChild(index).position;
    }

    // private Coroutine fadeCoroutine;

    // private void FadeIn()
    // {
    //     canvasGroup.alpha = 0f;
    //     if (fadeCoroutine != null)
    //     {
    //         StopCoroutine(fadeCoroutine);
    //     }
    //
    //     fadeCoroutine = StartCoroutine(FadeInCoroutine());
    // }
    //
    // private IEnumerator FadeInCoroutine()
    // {
    //     float duration = 1f;
    //     float elapsedTime = 0f;
    //
    //     while (elapsedTime < duration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         canvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
    //         yield return null;
    //     }
    //
    //     canvasGroup.alpha = 1f;
    //     fadeCoroutine = null;
    // }
}