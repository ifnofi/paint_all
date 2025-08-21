using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MyToggleBtn : MonoBehaviour, IPointerClickHandler
{
    private bool isOn = false;
    public bool IsOn{get{return isOn;}}

    private RectTransform chooseRect;
    private RectTransform unChooseRect;

    public bool use;

    private void Awake()
    {
        Init();
    }

    public UnityEvent onToggle = new UnityEvent();

    public void Init()
    {
        chooseRect = transform.Find("选中").GetComponent<RectTransform>();
        unChooseRect = transform.Find("未选中").GetComponent<RectTransform>();
        isOn = false;

        chooseRect.gameObject.SetActive(isOn);
        unChooseRect.gameObject.SetActive(!isOn);
    }

    public void UnChoose()
    {
        isOn = false;

        if (chooseRect == null)
        {
            chooseRect = transform.Find("选中").GetComponent<RectTransform>();
        }

        if (unChooseRect == null)
        {
            unChooseRect = transform.Find("未选中").GetComponent<RectTransform>();
        }

        chooseRect.gameObject.SetActive(isOn);
        unChooseRect.gameObject.SetActive(!isOn);
    }

    public void Choose()
    {
        isOn = true;
        if (chooseRect == null)
        {
            chooseRect = transform.Find("选中").GetComponent<RectTransform>();
        }

        if (unChooseRect == null)
        {
            unChooseRect = transform.Find("未选中").GetComponent<RectTransform>();
        }

        chooseRect.gameObject.SetActive(isOn);
        unChooseRect.gameObject.SetActive(!isOn);

        onToggle.Invoke();
    }

    public void ChooseNoInvoke()
    {
        isOn = true;
        if (chooseRect == null)
        {
            chooseRect = transform.Find("选中").GetComponent<RectTransform>();
        }

        if (unChooseRect == null)
        {
            unChooseRect = transform.Find("未选中").GetComponent<RectTransform>();
        }

        chooseRect.gameObject.SetActive(isOn);
        unChooseRect.gameObject.SetActive(!isOn);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!use)
        {
            return;
        }

        if (isOn && canUnchoose)
        {
            UnChoose();
            return;
        }

        Choose();
    }

    [Button]
    public void Click()
    {
        if (!use)
        {
            return;
        }

        if (isOn && canUnchoose)
        {
            UnChoose();
            return;
        }

        Choose();
    }

    public bool canUnchoose = false;
}