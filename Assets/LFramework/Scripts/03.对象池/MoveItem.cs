using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveItem : MonoBehaviour
{
    public RectTransform target;
    public float moveSpeed=1;
    public void BeginMove(RectTransform rectTransform,float time)
    {
        transform.position = rectTransform.position;
        transform.GetComponent<CanvasGroup>().DOFade(1, time);
        StartCoroutine(Move());
    }

    private bool isFading;
    private IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.position += new Vector3(0, 0, -moveSpeed*Time.deltaTime);
            if (transform.position.z<=target.transform.position.z && !isFading)
            {
                isFading = true;
                transform.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(
                    () =>
                    {
                        StopAllCoroutines();
                        isFading = false;
                        PoolManagerControl.Instance.Despawnfab(transform);
                    });
            }
        }
    }
}
