using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierController : MonoBehaviour
{
    public float updateTime = 2f;
    public List<RectTransform> playSequences = new List<RectTransform>();


    private void Start()
    {
        foreach (var item in transform.GetComponentsInChildren<PlaySequence>())
        {
            if (item != null)
            {
                playSequences.Add(item.GetComponent<RectTransform>());
            }
        }
    }

    void OnEnable()
    {
        if (checkCoroutine != null)
        {
            StopCoroutine(checkCoroutine);
        }

        checkCoroutine = StartCoroutine(Check());
    }

    private void OnDisable()
    {
        if (checkCoroutine != null)
        {
            StopCoroutine(checkCoroutine);
            checkCoroutine = null;
        }
    }


    private Coroutine checkCoroutine;

    private IEnumerator Check()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateTime);
            SetSort();
        }
    }


    private void SetSort()
    {
        // 按照 y 值进行排序
        playSequences.Sort((a, b) => b.anchoredPosition.y.CompareTo(a.anchoredPosition.y));

        // 根据排序结果调整层级
        foreach (var rect in playSequences)
        {
            if (rect.parent.name != rect.name)
            {
                rect.SetAsLastSibling();
            }
            else
            {
                rect.parent.SetAsLastSibling();
            }
        }
    }
}