using System.Collections.Generic;
using LFramework;
using TMPro;
using UnityEngine;

public class SetPos : MonoBehaviour
{
    public Transform cameras;
    public float moveUnit;
    public bool isSet;


    [Space(15)] public Transform tipTra;
    public TMP_Text positionText;
    public TMP_Text stateText;


    private void TextInput()
    {
        if (!isSet) return;
        var pos = cameras.localPosition;
        positionText.text = $"摄像机位置:({pos.x})";
    }


    private void Start()
    {
        moveUnit = float.Parse(IniTool.GetValue("设置", "移动系数", PathTool.Path.Combine("/Image.txt"), "1"));
        isSet = IniTool.GetValue("设置", "是否设置", PathTool.Path.Combine("/Image.txt"), "1") == "1" ? true : false;

        LoadData();

        if (!isSet) return;
        tipTra.gameObject.SetActive(true);
    }


    public void Update()
    {
        if (!isSet) return;

        #region 移动

        if (Input.GetKeyDown(KeyCode.A))
        {
            stateText.text = "未保存";
            cameras.localPosition -= new Vector3(moveUnit, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            stateText.text = "未保存";
            cameras.localPosition += new Vector3(moveUnit, 0, 0);
        }

        #endregion


        TextInput();

        if (Input.GetKeyDown(KeyCode.B))
        {
            SaveData();
        }
    }


    private void LoadData()
    {
        print("load data");
        var valueGroup = IniTool.GetValueGroup("Camera", PathTool.Path.Combine("/Image.txt"));
        cameras.localPosition = new Vector3(float.Parse(valueGroup["X"]), 0, -2000f);
    }

    private void SaveData()
    {
        var cameraPos = cameras.localPosition;
        var cameraDic = new Dictionary<string, string>
        {
            { "X", $"{cameraPos.x}" }
        };
        stateText.text = "保存成功";
        IniTool.SetValue("Camera", cameraDic, PathTool.Path.Combine("/Image.txt"));
    }
}