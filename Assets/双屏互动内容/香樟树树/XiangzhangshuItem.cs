using System;
using LFramework;
using TMPro;
using UnityEngine;

public class XiangzhangshuItem : MonoBehaviour
{
    public Transform target;
    public CanvasGroup canvasGroup;
    public bool canMove;

    public float speed = 1;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (canMove)
        {
            // 慢慢移动到目标点
            var dir = target.position - transform.position;

            transform.position += dir.normalized * speed;
            if (Vector2.Distance(target.position, transform.position) < 0.2f)
            {
                target.GetComponentInChildren<TMP_Text>().text = GetComponentInChildren<TMP_Text>().text;
                target.Show();
                transform.Hide();
                canMove = false;
            }
        }
    }

    public void Init(RectTransform randomOne)
    {
        randomOne.Hide();
        randomOne.GetOrAddComponent<CanvasGroup>().alpha = 1;
        randomOne.GetComponentInChildren<TMP_Text>().text = GetComponentInChildren<TMP_Text>().text;
        target = randomOne;
        canMove = false;
    }
}
