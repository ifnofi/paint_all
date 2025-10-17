using System.Collections.Generic;
using DG.Tweening;
using LFramework;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DualScreenControl : MonoSingleton<DualScreenControl>
{
    [TabGroup("桂博园")]
    public Transform guiboyuantrans;

    [TabGroup("桂博园")]
    public List<Texture2D> guiboyuan = new List<Texture2D>();

    [TabGroup("桂博园")]
    public RawImage guiboyuan_leftImage;

    [TabGroup("桂博园")]
    public RawImage guiboyuan_rightImage;

    [TabGroup("营地")]
    public Transform yingditrans;

    [TabGroup("营地")]
    public List<Texture2D> yingdi = new List<Texture2D>();

    [TabGroup("营地")]
    public RawImage yingdi_leftImage;

    [TabGroup("营地")]
    public RawImage yingdi_rightImage;

    [TabGroup("文昌阁")]
    public List<AnimationItem> wenchangge = new List<AnimationItem>();

    [TabGroup("面馆")]
    public List<AnimationItem> mianguan = new List<AnimationItem>();

    [TabGroup("面馆")]
    public TMP_Text mianText;
    
    
    [TabGroup("安乐塔")]
    public TMP_Text anletaText;
    [TabGroup("安乐塔")]
    public Transform anletatrans;

    public void AnletaShow()
    {
        anletaText.text = "<rotate=90>";
        anletaText.DOText("<rotate=90>　　安乐塔始建于五代吴越时期，因王子钱锷养病痊愈而建，山名由狮子山改为安乐山。现塔为明代重建，1 9 8 5 年修缮。 \n　　楼阁式砖木结构，白色塔身，高约 三十五米，底径近九米。塔内一百五十一 级螺旋台阶，层层交替方向。每层南北对开塔门，第五层为东南、西北对开。下四层外墙有砖雕及火焰龛，上三层外壁素面，仅有火焰龛和门。",
            20f).SetEase(Ease.Linear).SetSpeedBased(true);
        anletatrans.Show();
    }
}
