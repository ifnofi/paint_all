using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LFramework;
using TMPro;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class XiangzhangshuControl : MonoBehaviour
{
    public List<RectTransform> positions = new List<RectTransform>();
    private List<RectTransform> positionTemps = new List<RectTransform>();
    public RectTransform main;
    public TMP_Text mainText;

    public XiangzhangshuItem prefab;
    private Sequence _sequence;

    Queue<string> contentQueue = new Queue<string>();

    public void Play(string content = "")
    {
        contentQueue.Enqueue(content);
    }

    public RectTransform GetOne()
    {
        if (positionTemps.Count <= 0)
        {
            positionTemps.AddRange(positions);
        }

        return positionTemps.GetAndRemoveRandomItem();
    }

    private IEnumerator Start()
    {
        while (true)
        {
            if (contentQueue.Count > 0 && _sequence == null)
            {
                var content = contentQueue.Dequeue();
                prefab.Hide();
                _sequence = DOTween.Sequence();
                var randomOne = GetOne();
                prefab.Init(randomOne);
                prefab.GetComponent<CanvasGroup>().alpha = 0;
                mainText.text = content;
                prefab.transform.DOLocalMove(Vector3.zero, 0);
                prefab.Show();
                _sequence.Append(prefab.transform.DOScale(Vector3.one * 5, 1f));
                _sequence.Join(prefab.GetComponent<CanvasGroup>().DOFade(1, 1f));
                _sequence.AppendInterval(1);
                _sequence.Append(prefab.transform.DOMove(randomOne.position, 0.5f));
                _sequence.Join(prefab.transform.DOScale(randomOne.localScale, 0.5f));
                _sequence.OnComplete(() =>
                {
                    prefab.Hide();
                    randomOne.Show();
                    randomOne.GetOrAddComponent<CanvasGroup>().DOFade(0, 0.5f).SetDelay(15f).OnComplete(() =>
                    {
                        randomOne.Hide();
                    });
                    _sequence = null;
                });
                _sequence.Play();
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void Update()
    {
        return;
        if (Input.GetKeyDown(KeyCode.J))
        {
            Play();
        }
    }

    public void HideAll()
    {
        if (_sequence != null)
        {
            _sequence.Kill();
        }

        prefab.Hide();
        foreach (var rectTransform in positions)
        {
            rectTransform.Hide();
        }
    }
}
