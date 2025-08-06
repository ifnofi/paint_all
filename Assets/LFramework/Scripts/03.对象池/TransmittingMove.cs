using System;
using System.Collections;
using System.Collections.Generic;
using LFramework;
using UnityEngine;
using Random = UnityEngine.Random;

public class TransmittingMove : MonoBehaviour
{
    public RectTransform prefab;
    public Transform parent;


    public List<RectTransform> poss = new List<RectTransform>();

    private void Awake()
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            poss.Add(parent.GetChild(i).GetComponent<RectTransform>());
        }
    }

    private List<RectTransform> temp = new List<RectTransform>();
    private IEnumerator Start()
    {
        while (true)
        {
            if (temp.Count <= 0)
            {
                temp.AddRange(poss);
            }
            yield return new WaitForSeconds(0.1f);
            var go = PoolManagerControl.Instance.GetRandom();
            go.SetAsFirstSibling();
            var item = go.GetComponent<MoveItem>();
            var random = Random.Range(0, temp.Count);
            item.BeginMove(temp[random],5);
            temp.RemoveAt(random);
        }
    }

    public float fadeTime = 5;
}