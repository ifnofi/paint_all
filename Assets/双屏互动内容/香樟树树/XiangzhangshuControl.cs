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

    public void Play(string content = "")
    {
        if (_sequence != null)
        {
            _sequence.Kill();
        }

        prefab.Hide();
        _sequence = DOTween.Sequence();
        var randomOne = GetOne();
        prefab.Init(randomOne);
        mainText.text = content;
        prefab.transform.DOLocalMove(Vector3.zero, 0);
        prefab.Show();
        _sequence.Append(prefab.transform.DOScale(Vector3.one * 5, 1f));
        _sequence.AppendInterval(1);
        // _sequence.AppendCallback(() =>
        // {
        //     print("???");
        //     prefab.canMove = true;
        // });
        _sequence.Append(prefab.transform.DOMove(randomOne.position, 0.5f));
        _sequence.Join(prefab.transform.DOScale(randomOne.localScale, 0.5f));
        _sequence.AppendCallback(() =>
        {
            prefab.Hide();
            randomOne.GetComponentInChildren<TMP_Text>().text = prefab.GetComponentInChildren<TMP_Text>().text;
            randomOne.Show();
            ActionKit.Delay(15, () =>
            {
                randomOne.Hide();
            }).Start(this);
        });
        _sequence.Play();
    }

    public RectTransform GetOne()
    {
        if (positionTemps.Count <= 0)
        {
            positionTemps.AddRange(positions);
        }

        return positionTemps.GetAndRemoveRandomItem();
    }

    private void Start()
    {
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
