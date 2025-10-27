using System;
using System.Collections.Generic;
using DG.Tweening;
using LFramework;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DualScreenControl : MonoSingleton<DualScreenControl>
{
    #region 桂博园

    [TabGroup("桂博园")]
    public Transform guiboyuantrans;

    [TabGroup("桂博园")]
    public List<Texture2D> guiboyuan = new List<Texture2D>();

    private List<Texture2D> guiboyuanTemp = new List<Texture2D>();

    [TabGroup("桂博园")]
    public RawImage guiboyuan_leftImage;

    [TabGroup("桂博园")]
    public RawImage guiboyuan_rightImage;

    [TabGroup("桂博园")]
    public AnimationCurve gui_scaleEase;

    public void GuiboyuanShow()
    {
        TimeController.Kill("guiboyuantrans");
        DOTween.Kill(guiboyuantrans.GetOrAddComponent<CanvasGroup>());
        guiboyuantrans.GetOrAddComponent<CanvasGroup>().alpha = 1;
        guiboyuan_leftImage.texture = gui_GetRandomTexture();
        guiboyuan_rightImage.texture = gui_GetRandomTexture();

        guiboyuantrans.parent.SetAsLastSibling();
        guiboyuantrans.Show();
        TimeController.Call(10, () =>
        {
            guiboyuantrans.GetOrAddComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
            {
                guiboyuantrans.Hide();
            });
        }, "guiboyuantrans");
    }

    public void Guiboyuan_Hide()
    {
        DOTween.Kill(guiboyuantrans.GetOrAddComponent<CanvasGroup>());
        TimeController.Kill("guiboyuantrans");
        guiboyuantrans.Hide();
    }

    private Texture2D gui_GetRandomTexture()
    {
        if (guiboyuanTemp.Count == 0)
        {
            guiboyuanTemp.AddRange(guiboyuan);
        }

        return guiboyuanTemp.GetAndRemoveRandomItem();
    }

    #endregion

    #region 营地

    [TabGroup("营地")]
    public Transform yingditrans;

    [TabGroup("营地")]
    public List<Texture2D> yingdi = new List<Texture2D>();

    private List<Texture2D> yingdiTemp = new List<Texture2D>();

    [TabGroup("营地")]
    public RawImage yingdi_leftImage;

    [TabGroup("营地")]
    public RawImage yingdi_rightImage;

    public void YingdiShow()
    {
        TimeController.Kill("yingditrans");
        DOTween.Kill(yingditrans.GetOrAddComponent<CanvasGroup>());
        yingditrans.GetOrAddComponent<CanvasGroup>().alpha = 1;
        yingdi_leftImage.texture = yingdi_GetRandomTexture();
        yingdi_rightImage.texture = yingdi_GetRandomTexture();
        yingditrans.parent.SetAsLastSibling();
        yingditrans.Show();
        TimeController.Call(10, () =>
        {
            yingditrans.GetOrAddComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
            {
                yingditrans.Hide();
            });
        }, "yingditrans");
    }

    public void Yingdi_Hide()
    {
        TimeController.Kill("yingditrans");
        DOTween.Kill(yingditrans.GetOrAddComponent<CanvasGroup>());
        yingditrans.Hide();
    }

    private Texture2D yingdi_GetRandomTexture()
    {
        if (yingdiTemp.Count == 0)
        {
            yingdiTemp.AddRange(yingdi);
        }

        return yingdiTemp.GetAndRemoveRandomItem();
    }

    #endregion

    #region 文昌阁

    [TabGroup("文昌阁")]
    public List<AnimationItem> wenchangge = new List<AnimationItem>();

    private List<AnimationItem> wenchanggeTemp = new List<AnimationItem>();

    public void WenchanggeShow()
    {
        if (wenchanggeTemp.Count == 0)
        {
            wenchanggeTemp.AddRange(wenchangge);
        }

        var item = wenchanggeTemp.GetAndRemoveRandomItem();
        item.Show();
    }

    public void Wenchangge_Hide()
    {
        foreach (var animationItem in wenchangge)
        {
            animationItem.Hide();
        }
    }

    #endregion

    #region 面馆

    [TabGroup("面馆")]
    public List<AnimationItem> mianguan = new List<AnimationItem>();

    [TabGroup("面馆")]
    public TMP_Text mianText;

    private List<AnimationItem> mianguanTemp = new List<AnimationItem>();

    public void MianguanShow()
    {
        if (mianguanTemp.Count == 0)
        {
            mianguanTemp.AddRange(mianguan);
        }

        var item = mianguanTemp.GetAndRemoveRandomItem();
        item.Show();
    }

    public void Mian_Hide()
    {
        foreach (var animationItem in mianguan)
        {
            animationItem.Hide();
        }
    }

    #endregion

    #region 安乐塔

    [TabGroup("安乐塔")]
    public TMP_Text anletaText;

    [TabGroup("安乐塔")]
    public Transform anletatrans;

    public void AnletaShow()
    {
        DOTween.Kill(anletaText);
        DOTween.Kill(anletatrans);
        anletatrans.Hide();
        anletatrans.GetOrAddComponent<CanvasGroup>().alpha = 1;
        anletaText.text = "<rotate=90>";
        anletaText.DOText("<rotate=90>　　安乐塔始建于五代吴越时期，因王子钱锷养病痊愈而建，山名由狮子山改为安乐山。现塔为明代重建，1 9 8 5 年修缮。 \n　　楼阁式砖木结构，白色塔身，高约 三十五米，底径近九米。塔内一百五十一 级螺旋台阶，层层交替方向。每层南北对开塔门，第五层为东南、西北对开。下四层外墙有砖雕及火焰龛，上三层外壁素面，仅有火焰龛和门。",
            20f).SetEase(Ease.Linear).SetSpeedBased(true);
        anletatrans.Show();

        anletatrans.GetOrAddComponent<CanvasGroup>().DOFade(0, 0.5f).SetDelay(30f).OnComplete(() =>
        {
            anletatrans.Hide();
            anletaText.text = "";
        });
    }

    public void AnletaHide()
    {
        DOTween.Kill(anletaText);
        DOTween.Kill(anletatrans);
        anletatrans.Hide();
        anletaText.text = "";
    }

    #endregion

    #region 百年香樟树

    // 预留位置
    public XiangzhangshuControl xiangzhangshu;

    #endregion

    private void Start()
    {
        HideAll();
    }


    public void HideAll()
    {
        Guiboyuan_Hide();
        Mian_Hide();
        Wenchangge_Hide();
        Yingdi_Hide();
        AnletaHide();
        xiangzhangshu.HideAll();
    }

    public void Open(string type, string message = "")
    {
        if (GameMgr.Instance.IsPlaying)
        {
            return;
        }

        print("open" + type);
        switch (type)
        {
            case "GBY": //桂博园
                Guiboyuan_Hide();
                GuiboyuanShow();
                break;
            case "MG": //面馆
                Mian_Hide();
                MianguanShow();
                break;
            case "YQLYJD": //营地
                Yingdi_Hide();
                YingdiShow();
                break;
            case "WCG": //文昌阁
                Wenchangge_Hide();
                WenchanggeShow();

                break;
            case "BNXZS": //百年香樟树
                xiangzhangshu.Play(message);
                break;
            case "ALT": //安乐塔
                AnletaHide();
                AnletaShow();
                break;
            default:
                Debug.LogWarning($"未知的 type: {type}");
                break;
        }
    }
}
