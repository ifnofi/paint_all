using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using LFramework;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
#if UNITY_EDITOR
#endif

[DisallowMultipleComponent]
public class Encryption : MonoBehaviour
{
    [Header("DEMO预制体，为空自动生成")] public GameObject DEMO;
    [Header("截止日期")] public Date endTime;
    [Header("许可次数")] public int AvailableTime;

    [Header("出现DEMO后是否允许点击")] public bool overdueAvailable;

    [Header("显示提示的日期")] public int TipDay = 3;
    [Header("显示提示的次数")] public int TipTimes = 3;

    GameObject keyDownToShow;

    public string url;

    /// <summary>
    /// 重置
    /// </summary>
    public void Chongzhi()
    {
        if (Directory.Exists(url))
        {
            Directory.Delete(url);
        }

        //string lastCodePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/lts.ini";
        //Directory.Delete(lastCodePath);
        string path1 = Environment.CurrentDirectory + @"\Key\ist.cxp";
        if (File.Exists(path1))
        {
            File.Delete(path1);
        }

        Debug.Log(path1);
        //  Directory.Delete(path1);
        //string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Key\\ist.cxp";
        //Directory.Delete(path);

        SetTimeZero();
    }


    // Start is called before the first frame update
    private void Start()
    {
        playTime = GetInt("PlayTime");
        MAC = GetMac();
        CheckIsShowDemo();
        playTime += 1;
        SetInt("PlayTime", playTime);
        Debug.Log("使用总次数：" + playTime);

        #region 按键

        keyCodes.Add(KeyCode.A);
        keyCodes.Add(KeyCode.B);
        keyCodes.Add(KeyCode.C);
        keyCodes.Add(KeyCode.D);
        keyCodes.Add(KeyCode.E);
        keyCodes.Add(KeyCode.F);
        keyCodes.Add(KeyCode.G);
        keyCodes.Add(KeyCode.H);
        keyCodes.Add(KeyCode.I);
        keyCodes.Add(KeyCode.J);
        keyCodes.Add(KeyCode.K);
        keyCodes.Add(KeyCode.L);
        keyCodes.Add(KeyCode.M);
        keyCodes.Add(KeyCode.N);
        keyCodes.Add(KeyCode.O);
        keyCodes.Add(KeyCode.P);
        keyCodes.Add(KeyCode.Q);
        keyCodes.Add(KeyCode.R);
        keyCodes.Add(KeyCode.S);
        keyCodes.Add(KeyCode.T);
        keyCodes.Add(KeyCode.U);
        keyCodes.Add(KeyCode.V);
        keyCodes.Add(KeyCode.W);
        keyCodes.Add(KeyCode.X);
        keyCodes.Add(KeyCode.Y);
        keyCodes.Add(KeyCode.Z);
        keyCodes.Add(KeyCode.Alpha0);
        keyCodes.Add(KeyCode.Alpha1);
        keyCodes.Add(KeyCode.Alpha2);
        keyCodes.Add(KeyCode.Alpha3);
        keyCodes.Add(KeyCode.Alpha4);
        keyCodes.Add(KeyCode.Alpha5);
        keyCodes.Add(KeyCode.Alpha6);
        keyCodes.Add(KeyCode.Alpha7);
        keyCodes.Add(KeyCode.Alpha8);
        keyCodes.Add(KeyCode.Alpha9);

        #endregion

        keyboardBackground = GetComponent<RectTransform>();
        //  inputText = GetComponentInChildren<Text>();
    }

    /// <summary>
    /// 获取注册表数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int GetInt(string name)
    {
        int rt = 0;

        if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath + "/" + name + ".txt";
            Debug.Log(path);
            if (File.Exists(path))
            {
                rt = Convert.ToInt32(File.ReadAllText(path));
            }
            else
            {
                File.WriteAllText(path, "0");
            }
        }
        else
        {
            rt = PlayerPrefs.GetInt(name);
        }

        return rt;
    }

    /// <summary>
    /// 设置注册表数据
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void SetInt(string name, int value)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath + "/" + name + ".txt";
            if (File.Exists(path))
            {
                File.WriteAllText(path, value.ToString());
            }
            else
            {
                File.Create(path).Close();
                File.WriteAllText(path, value.ToString());
            }
        }
        else
        {
            PlayerPrefs.SetInt(name, value);
        }
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void SetTimeZero()
    {
        SetInt("PlayTime", 0);
        SetInt("NumberOfTime", 0);
    }


    /// <summary>
    /// 验证是否显示demo
    /// </summary>
    public void CheckIsShowDemo()
    {
        Debug.Log("check");
        try
        {
            // 是否永久授权 并且获取end时间
            if (CheckNoLimitAndGetEndData())
            {
                if (DEMO != null)
                    Destroy(DEMO);
                else
                {
                    Debug.Log("NODEMO");
                }

                return;
            }

            // 启动次数是否超限     时间是否超限
            if (CheckNumberOfTime() && CheckDate())
            {
                if (DEMO != null)
                    Destroy(DEMO);
            }
            else
            {
                if (DEMO != null)
                {
                    DEMO.SetActive(true);
                }
                else
                {
                    DEMO = ShowDEMO();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            if (DEMO != null)
            {
                DEMO.SetActive(true);
            }
            else
            {
                DEMO = ShowDEMO();
            }
        }
    }

    public void SetNumberOfTime(int time)
    {
        int usedTime = AvailableTime - time;
        print(time);
        SetInt("NumberOfTime", usedTime);
    }

    public void SetEncode(InputField input)
    {
        string pwd = input.text.Replace(" ", "");
        if (pwd == "" || pwd.Length != 12 || !Regex.IsMatch(pwd, @"^[a-zA-Z0-9]+$"))
        {
            ShowMessageTip("请输入正确的授权码");
            // 退出
            return;
        }

        string lastCodePath;
        if (Application.platform == RuntimePlatform.Android)
        {
            lastCodePath = Application.persistentDataPath + "/lts.ini";
        }
        else
        {
            lastCodePath = Environment.CurrentDirectory + "/lts.ini";
        }

        // 检测永久授权
        if (CheckNoLimitAndGetEndData(pwd))
        {
            // 通过永久授权 Check
            CheckIsShowDemo();
            // 退出    (为了防止永久授权码被放入历史中)
            return;
        }

        // 未通过时  继续后续流程  (为了防止永久授权码被放入历史中)

        if (!File.Exists(lastCodePath))
        {
            // Debug.Log("配置文件缺失，注册失败");
            // return;

            File.Create(lastCodePath).Close();
        }

        string[] lastPwd = File.ReadAllLines(lastCodePath);


        for (int i = 0; i < lastPwd.Length; i++)
        {
            Debug.Log(lastPwd[i]);

            if (lastPwd[i] != "" && lastPwd[i] == pwd)
            {
                Debug.Log("注册码已失效");
                ShowMessageTip("注册码已失效");
                return;
            }
        }

        List<string> oldPwd = new List<string>();
        oldPwd.AddRange(lastPwd);
        oldPwd.Add(pwd);

        string newPasswordPath;
        if (Application.platform == RuntimePlatform.Android)
        {
            newPasswordPath = Application.persistentDataPath + "/ist.cxp";
        }
        else
        {
            newPasswordPath = EncryptionTool.GetEncryptionKeyPath();
        }

        Debug.Log(pwd);
        File.WriteAllText(newPasswordPath, pwd);
        File.WriteAllLines(lastCodePath, oldPwd);

        SetNumberOfTime(GetTimes(newPasswordPath));
        CheckIsShowDemo();
    }

    int GetTimes(string path)
    {
        string password = File.ReadAllText(path);
        password = password.Replace(" ", "");

        if (string.IsNullOrEmpty(password))
        {
            return 0;
        }

        var jiemi = JieMi(password);

        string mac = MAC.Substring(2, 2);
        if (!jiemi.StartsWith(mac))
        {
            print(jiemi + ":未通过");
            return 0;
        }

        try
        {
            // 后四位是次数
            var times = int.Parse(jiemi.Substring(jiemi.Length - 4, 4));
            return (times);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return 0;
        }


        // char key = jiemi[jiemi.Length - 1];
        // switch (key)
        // {
        //     case 'A':
        //         return 7;
        //     case 'B':
        //         return 15;
        //     case 'C':
        //         return 30;
        //     case 'D':
        //         return 60;
        //     case 'E':
        //         return 90;
        //     default:
        //         return 0;
        // }
    }

    int playTime;


    /// <summary>
    /// 校验是否永久授权  解码得到 end data
    /// </summary>
    /// <returns></returns>
    private bool CheckNoLimitAndGetEndData()
    {
        string lastCodePath;
        if (Application.platform == RuntimePlatform.Android)
        {
            lastCodePath = Application.persistentDataPath + "/lts.ini";
        }
        else
        {
            lastCodePath = EncryptionTool.GetLocalApplicationDataPath() + "/lts.ini";
        }

        // 历史注册码
        Debug.Log(lastCodePath);
        if (playTime <= 0 && !File.Exists(lastCodePath))
        {
            File.Create(lastCodePath);
        }

        // 当前注册码信息
        string path;

        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/ist.cxp";
        }
        else
        {
            path = EncryptionTool.GetEncryptionKeyPath();
            url = path;
            Debug.Log(path);
        }

        // 如果当前注册码文件不存在 则创建 
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(EncryptionTool.GetLocalApplicationDataPath() + "\\Key");
            File.Create(path);
            return false;
        }
        else
        {
            // 读取文件内容
            string password = File.ReadAllText(path);
            Debug.Log(password);
            // 获取mac地址 截取 3 4 位
            string mac = MAC.Substring(2, 2);

            Debug.Log(mac);
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            Debug.Log("密码:" + password);

            var jieMiPassword = JieMi(password);
            Debug.Log("解密:" + jieMiPassword);
            // 解密 如果秘闻 不是以mac地址开头 直接返回false
            if (!jieMiPassword.StartsWith(mac))
            {
                Debug.Log(jieMiPassword + ":未通过");
                return false;
            }

            // 截取 mac 后面的内容
            string restValue = jieMiPassword.Substring(2);

            Debug.Log(restValue);

            if (restValue.Contains("nolimit"))
            {
                Debug.Log("永久授权");
                return true;
            }
            else
            {
                // 20240817888
                string dateValue = "20" + restValue;

                Debug.Log("日期更新");
                print(dateValue);
                endTime.year = Convert.ToInt32(dateValue.Substring(0, 4));
                endTime.month = Convert.ToInt32(dateValue.Substring(4, 2));
                endTime.day = Convert.ToInt32(dateValue.Substring(6, 2));
                return false;
            }
        }
    }

    bool CheckNoLimitAndGetEndData(string input)
    {
        // 判断是否存在 
        string path;
        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/ist.cxp";
        }
        else
        {
            path = EncryptionTool.GetLocalApplicationDataPath() + "\\Key\\ist.cxp";
        }

        if (!File.Exists(path))
        {
            if (!Directory.Exists(EncryptionTool.GetLocalApplicationDataPath() + "\\Key"))
            {
                Directory.CreateDirectory(EncryptionTool.GetLocalApplicationDataPath() + "\\Key");
            }

            File.Create(path);

            if (Application.platform == RuntimePlatform.Android)
            {
            }
            else
            {
                // using (FileStream fs = new FileStream(parentPath + "/pwd.tts", FileMode.OpenOrCreate, FileAccess.Write))
                // {
                //     byte[] buffer = new byte[UnityEngine.Random.Range(5000, 10000)];
                //     fs.Write(buffer, 0, buffer.Length);
                // }
                //
                // using (FileStream fs = new FileStream(parentPath + "/app.manifest", FileMode.OpenOrCreate, FileAccess.Write))
                // {
                //     byte[] buffer = new byte[UnityEngine.Random.Range(5000, 10000)];
                //     fs.Write(buffer, 0, buffer.Length);
                // }
                //
                // using (FileStream fs = new FileStream(parentPath + "/main.user", FileMode.OpenOrCreate, FileAccess.Write))
                // {
                //     byte[] buffer = new byte[UnityEngine.Random.Range(5000, 10000)];
                //     fs.Write(buffer, 0, buffer.Length);
                // }
            }
        }

        string password = input;
        // 获取mac地址 截取第 3 4 位
        string mac = MAC.Substring(2, 2);
        // 为空 直接返回false
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }

        // 解密 如果秘闻 不是以mac地址开头 直接返回false
        if (!JieMi(password).StartsWith(mac))
        {
            Console.WriteLine(JieMi(password) + ":未通过");
            return false;
        }

        // 截取 mac 后面的内容
        string head = mac;

        string restValue = JieMi(password).Replace(head, "");
        if (restValue.Contains("nolimit"))
        {
            Console.WriteLine("永久授权");
            File.WriteAllText(path, input);
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckDate()
    {
        var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        var end = new DateTime(endTime.year, endTime.month, endTime.day);
        Debug.Log($"截止日期：{end.Year}年{end.Month}月{end.Day}日");

        int restOfDay = (end - now).Days;

        if (restOfDay >= 0 && restOfDay <= TipDay - 1)
        {
            ShowTip(restOfDay + 1, restTime);
        }

        if (restOfDay >= 0)
        {
            Debug.Log($"剩余{restOfDay}天");
            return true;
        }
        else
        {
            Debug.Log("已到期");
            ShowMessageTip("已到期");
            return false;
        }
    }

    int restTime;

    /// <summary>
    /// 检测 次数
    /// </summary>
    /// <returns></returns>
    bool CheckNumberOfTime()
    {
        // 获取注册表启动次数
        int useTime = GetInt("NumberOfTime");
        useTime += 1;

        Debug.Log(useTime);
        Debug.Log(AvailableTime);

        if (useTime <= AvailableTime)
        {
            restTime = AvailableTime - useTime;
            if (restTime >= 0 && restTime <= TipTimes - 1)
            {
                ShowTip(99, restTime + 1);
            }

            print($"剩余{restTime}次");
            SetInt("NumberOfTime", useTime);
            return true;
        }
        else
        {
            print("次数已用完");
            ShowMessageTip("次数已用完");
            return false;
        }
    }

    public string JiaMi(string INPUT)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < INPUT.Length; i++)
        {
            stringBuilder.Append(EncodeCXP(INPUT[i]));
        }

        return stringBuilder.ToString();
    }

    public string JieMi(string INPUT)
    {
        var stringBuilder = new StringBuilder();
        foreach (var t in INPUT)
        {
            stringBuilder.Append(DecryptCXP(t));
        }

        return stringBuilder.ToString();
    }

    private List<char> originalChar = new List<char>()
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
        'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    };

    private List<char> encodeChar = new List<char>()
    {
        'P', 'O', 'M', 'U', 'Y', 'T', 'R', 'E', 'W', 'Q', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'I', 'N', 'B', 'V', 'C', 'X', 'Z', '0', '2', '4', '6', '8', '1', '3', '5',
        '7', '9', 'p', 'o', 'm', 'u', 'y', 't', 'r', 'e', 'w', 'q', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'i', 'n', 'b', 'v', 'c', 'x', 'z',
    };

    char EncodeCXP(char value)
    {
        var index = originalChar.IndexOf(value);
        return index >= 0 ? encodeChar[index] : ' ';
    }

    char DecryptCXP(char value)
    {
        var index = encodeChar.IndexOf(value);
        return index >= 0 ? originalChar[index] : ' ';
    }

    public Transform _demo;
    public Text _xinxiTip;

    public GameObject ShowDEMO()
    {
        if (GameObject.Find("EventSystem") == null)
        {
            GameObject tGO = new GameObject("EventSystem");
            tGO.AddComponent<EventSystem>();
            tGO.AddComponent<StandaloneInputModule>();
        }

        //if (GameObject.Find("Canvas") == null)
        //{

        //    GameObject tGO = new GameObject("Canvas");
        //    tGO.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        //    tGO.AddComponent<CanvasScaler>();
        //    tGO.AddComponent<GraphicRaycaster>();
        //    tGO.layer = 100;

        //}


        Font defaultFont = Font.CreateDynamicFontFromOSFont("Arial", 14);
        Canvas canvas = (new GameObject("到期提示")).AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        CanvasScaler cs = canvas.gameObject.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        canvas.gameObject.AddComponent<GraphicRaycaster>();
        Text demo = new GameObject("DEMO").AddComponent<Text>();
        _demo = demo.transform;
        demo.font = defaultFont;
        //demo.transform.parent = canvas.transform;
        demo.transform.SetParent(canvas.transform);
        demo.rectTransform.anchorMin = Vector2.zero;
        demo.rectTransform.anchorMax = Vector2.one;
        demo.rectTransform.sizeDelta = Vector2.zero;
        demo.rectTransform.anchoredPosition = Vector2.zero;
        demo.lineSpacing = 0.48f;
        demo.fontSize = 150;
        demo.text = "DEMO\r\n" + "\u2000\u2000<Size=80>-未授权(new)-</Size>";
        demo.color = new Color(0, 0, 0, 0.55f);
        demo.raycastTarget = !overdueAvailable;
        Image background = new GameObject("Background").AddComponent<Image>();
        keyDownToShow = background.gameObject;
        //background.transform.parent = demo.transform;
        background.transform.SetParent(demo.transform);
        background.rectTransform.sizeDelta = new Vector2(910, 380);
        background.rectTransform.anchoredPosition = Vector2.zero;
        background.color = new Color(0.85f, 0.85f, 0.85f, 1);
        background.gameObject.AddComponent<Outline>().effectDistance = new Vector2(3, -3);
        Text Tip = new GameObject("提示").AddComponent<Text>();
        Tip.font = defaultFont;
        Tip.fontSize = 55;
        //Tip.transform.parent = background.transform;
        Tip.transform.SetParent(background.transform);
        Tip.rectTransform.sizeDelta = new Vector2(800, 115);
        Tip.rectTransform.anchoredPosition = new Vector2(0, 132.5f);
        Tip.gameObject.AddComponent<Outline>().effectDistance = new Vector2(2, -2);
        Tip.text = "试用结束，未注册";
        Tip.alignment = TextAnchor.MiddleCenter;

        Text xinxiTip = new GameObject("信息").AddComponent<Text>();
        xinxiTip.font = defaultFont;
        xinxiTip.fontSize = 30;
        //Tip.transform.parent = background.transform;
        xinxiTip.transform.SetParent(background.transform);
        xinxiTip.rectTransform.sizeDelta = new Vector2(800, 60);
        xinxiTip.rectTransform.anchoredPosition = new Vector2(0, -160f);
        xinxiTip.gameObject.AddComponent<Outline>().effectDistance = new Vector2(2, -2);
        xinxiTip.text = "message";
        xinxiTip.alignment = TextAnchor.MiddleCenter;
        _xinxiTip = xinxiTip;


        Text characteristic = new GameObject("特征码").AddComponent<Text>();
        characteristic.font = defaultFont;
        characteristic.fontSize = 30;
        //characteristic.transform.parent = background.transform;
        characteristic.transform.SetParent(background.transform);
        characteristic.rectTransform.sizeDelta = new Vector2(800, 50);
        characteristic.rectTransform.anchoredPosition = new Vector2(0, 71);
        characteristic.gameObject.AddComponent<Outline>().effectDistance = new Vector2(2, -2);
        characteristic.text = "特征码：" + MAC;
        characteristic.alignment = TextAnchor.MiddleCenter;
        Button ButtonClose = new GameObject("关闭按钮").AddComponent<Button>();
        CircleImage closeImg = ButtonClose.gameObject.AddComponent<CircleImage>();
        //closeImg.transform.parent = background.transform;
        closeImg.transform.SetParent(background.transform);
        closeImg.rectTransform.sizeDelta = new Vector2(80, 80);
        closeImg.rectTransform.anchoredPosition = new Vector2(450, 185);
        closeImg.gameObject.AddComponent<Outline>().effectDistance = new Vector2(3, -3);
        ButtonClose.targetGraphic = closeImg;
        ButtonClose.onClick.AddListener(() => { background.gameObject.SetActive(false); });
        Text closeText = new GameObject("X").AddComponent<Text>();
        //closeText.transform.parent = ButtonClose.transform;
        closeText.transform.SetParent(ButtonClose.transform);
        closeText.rectTransform.anchorMin = Vector2.zero;
        closeText.rectTransform.anchorMax = Vector2.one;
        closeText.rectTransform.sizeDelta = Vector2.zero;
        closeText.rectTransform.anchoredPosition = Vector2.zero;
        closeText.font = defaultFont;
        closeText.fontSize = 46;
        closeText.lineSpacing = -0.17f;
        closeText.alignment = TextAnchor.MiddleCenter;
        closeText.color = Color.black;
        closeText.text = "\r\nX";
        Image input = new GameObject("注册码输入框").AddComponent<Image>();
        input.gameObject.AddComponent<Outline>().effectDistance = new Vector2(3, -3);
        //input.transform.parent = background.transform;
        input.transform.SetParent(background.transform);
        input.GetComponent<RectTransform>().sizeDelta = new Vector2(-120, 80);
        input.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        input.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
        input.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
        Text ph = new GameObject("Placeholder").AddComponent<Text>();
        //ph.transform.parent = input.transform;
        ph.transform.SetParent(input.transform);
        ph.rectTransform.anchorMin = Vector2.zero;
        ph.rectTransform.anchorMax = Vector2.one;
        ph.rectTransform.sizeDelta = Vector2.zero;
        ph.rectTransform.anchoredPosition = Vector2.zero;
        ph.font = defaultFont;
        ph.fontStyle = FontStyle.Italic;
        ph.fontSize = 30;
        ph.alignment = TextAnchor.MiddleCenter;
        ph.color = new Color(0.196f, 0.196f, 0.196f, 0.5f);
        ph.text = "请输入注册码";
        Text it = new GameObject("Text").AddComponent<Text>();
        //it.transform.parent = input.transform;
        it.transform.SetParent(input.transform);
        it.rectTransform.anchorMin = Vector2.zero;
        it.rectTransform.anchorMax = Vector2.one;
        it.rectTransform.sizeDelta = Vector2.zero;
        it.rectTransform.anchoredPosition = Vector2.zero;
        it.supportRichText = false;
        it.font = defaultFont;
        it.fontSize = 30;
        it.alignment = TextAnchor.MiddleCenter;
        it.color = new Color(0.196f, 0.196f, 0.196f, 1f);
        InputField inputField = input.gameObject.AddComponent<InputField>();

        _inputField = inputField;
        inputField.textComponent = it;
        inputField.placeholder = ph;


        Button ButtonRegister = new GameObject("注册按钮").AddComponent<Button>();
        Image registerImg = ButtonRegister.gameObject.AddComponent<Image>();
        //ButtonRegister.transform.parent = background.transform;
        ButtonRegister.transform.SetParent(background.transform);
        ButtonRegister.targetGraphic = registerImg;
        registerImg.rectTransform.anchoredPosition = new Vector2(0, -90);
        registerImg.rectTransform.sizeDelta = new Vector2(215, 70);
        ButtonRegister.gameObject.AddComponent<Outline>().effectDistance = new Vector2(3, -3);
        Text registerText = new GameObject("注册").AddComponent<Text>();
        //registerText.transform.parent = registerImg.transform;
        registerText.transform.SetParent(registerImg.transform);
        registerText.rectTransform.anchorMin = Vector2.zero;
        registerText.rectTransform.anchorMax = Vector2.one;
        registerText.rectTransform.sizeDelta = Vector2.zero;
        registerText.rectTransform.anchoredPosition = Vector2.zero;
        registerText.alignment = TextAnchor.MiddleCenter;
        registerText.font = defaultFont;
        registerText.fontSize = 28;
        registerText.color = new Color(0.196f, 0.196f, 0.196f, 1f);
        registerText.text = "注册";
        ButtonRegister.onClick.AddListener(() => { SetEncode(inputField); });


        Button ButtonRegister2 = new GameObject("呼出键盘").AddComponent<Button>();
        Image registerImg2 = ButtonRegister2.gameObject.AddComponent<Image>();
        //ButtonRegister.transform.parent = background.transform;
        ButtonRegister2.transform.SetParent(background.transform);
        ButtonRegister2.targetGraphic = registerImg2;
        registerImg2.rectTransform.anchoredPosition = new Vector2(250, -90);
        registerImg2.rectTransform.sizeDelta = new Vector2(115, 70);
        ButtonRegister2.gameObject.AddComponent<Outline>().effectDistance = new Vector2(3, -3);
        Text registerText2 = new GameObject("键盘").AddComponent<Text>();
        //registerText.transform.parent = registerImg.transform;
        registerText2.transform.SetParent(registerImg2.transform);
        registerText2.rectTransform.anchorMin = Vector2.zero;
        registerText2.rectTransform.anchorMax = Vector2.one;
        registerText2.rectTransform.sizeDelta = Vector2.zero;
        registerText2.rectTransform.anchoredPosition = Vector2.zero;
        registerText2.alignment = TextAnchor.MiddleCenter;
        registerText2.font = defaultFont;
        registerText2.fontSize = 28;
        registerText2.color = new Color(0.196f, 0.196f, 0.196f, 1f);
        registerText2.text = "系统键盘";
        ButtonRegister2.onClick.AddListener(() => { System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\osk.exe"); });

        Button ButtonRegister3 = new GameObject("呼出键盘2").AddComponent<Button>();
        Image registerImg3 = ButtonRegister3.gameObject.AddComponent<Image>();
        //ButtonRegister.transform.parent = background.transform;
        ButtonRegister3.transform.SetParent(background.transform);
        ButtonRegister3.targetGraphic = registerImg3;
        registerImg3.rectTransform.anchoredPosition = new Vector2(380, -90);
        registerImg3.rectTransform.sizeDelta = new Vector2(115, 70);
        ButtonRegister3.gameObject.AddComponent<Outline>().effectDistance = new Vector2(3, -3);
        Text registerText3 = new GameObject("键盘2").AddComponent<Text>();
        //registerText.transform.parent = registerImg.transform;
        registerText3.transform.SetParent(registerImg3.transform);
        registerText3.rectTransform.anchorMin = Vector2.zero;
        registerText3.rectTransform.anchorMax = Vector2.one;
        registerText3.rectTransform.sizeDelta = Vector2.zero;
        registerText3.rectTransform.anchoredPosition = Vector2.zero;
        registerText3.alignment = TextAnchor.MiddleCenter;
        registerText3.font = defaultFont;
        registerText3.fontSize = 28;
        registerText3.color = new Color(0.196f, 0.196f, 0.196f, 1f);
        registerText3.text = "键盘2";
        ButtonRegister3.onClick.AddListener(GenerateKeyboard);


        return canvas.gameObject;
    }

    public InputField _inputField;

    public void ShowMessageTip(string message)
    {
        if (_xinxiTip != null)
        {
            _xinxiTip.text = message;
        }
    }

    public void ShowTip(int day, int time)
    {
        if (GameObject.Find("到期提醒"))
            DestroyImmediate(GameObject.Find("到期提醒"));
        Font defaultFont = Font.CreateDynamicFontFromOSFont("Arial", 14);
        Canvas canvas = (new GameObject("到期提醒")).AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        CanvasScaler cs = canvas.gameObject.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        canvas.gameObject.AddComponent<GraphicRaycaster>();
        Image background = new GameObject("Background").AddComponent<Image>();
        //background.transform.parent = demo.transform;
        background.transform.SetParent(canvas.transform);
        background.rectTransform.sizeDelta = new Vector2(910, 380);
        background.rectTransform.anchoredPosition = Vector2.zero;
        background.color = new Color(0.85f, 0.85f, 0.85f, 1);
        background.gameObject.AddComponent<Outline>().effectDistance = new Vector2(3, -3);
        Text Tip = new GameObject("提示").AddComponent<Text>();
        Tip.font = defaultFont;
        Tip.fontSize = 55;
        //Tip.transform.parent = background.transform;
        Tip.transform.SetParent(background.transform);
        Tip.rectTransform.sizeDelta = new Vector2(800, 115);
        Tip.rectTransform.anchoredPosition = new Vector2(0, 46f);
        Tip.gameObject.AddComponent<Outline>().effectDistance = new Vector2(2, -2);
        if (day > TipDay)
        {
            Tip.text = String.Format("试用剩余<color=red>{1}</color>次", day, time);
        }
        else
        {
            Tip.text = String.Format("试用剩余<color=red>{0}</color>天,<color=red>{1}</color>次", day, time);
        }

        Tip.alignment = TextAnchor.MiddleCenter;
        Button ButtonRegister = new GameObject("确定按钮").AddComponent<Button>();
        Image registerImg = ButtonRegister.gameObject.AddComponent<Image>();
        //ButtonRegister.transform.parent = background.transform;
        ButtonRegister.transform.SetParent(background.transform);
        ButtonRegister.targetGraphic = registerImg;
        registerImg.rectTransform.anchoredPosition = new Vector2(0, -110);
        registerImg.rectTransform.sizeDelta = new Vector2(215, 70);
        ButtonRegister.gameObject.AddComponent<Outline>().effectDistance = new Vector2(3, -3);
        Text registerText = new GameObject("确定").AddComponent<Text>();
        //registerText.transform.parent = registerImg.transform;
        registerText.transform.SetParent(registerImg.transform);
        registerText.rectTransform.anchorMin = Vector2.zero;
        registerText.rectTransform.anchorMax = Vector2.one;
        registerText.rectTransform.sizeDelta = Vector2.zero;
        registerText.rectTransform.anchoredPosition = Vector2.zero;
        registerText.alignment = TextAnchor.MiddleCenter;
        registerText.font = defaultFont;
        registerText.fontSize = 28;
        registerText.color = new Color(0.196f, 0.196f, 0.196f, 1f);
        registerText.text = "确定";
        ButtonRegister.onClick.AddListener(() => { Destroy(canvas.gameObject); });
    }

    private string MAC;

    /// <summary>
    /// 此法获取只有四位
    /// </summary>
    /// <returns></returns>
    public string GetMac()
    {
        var deviceID = SystemInfo.deviceUniqueIdentifier;
        if (!string.IsNullOrEmpty(deviceID))
        {
            Debug.Log("Device ID: " + deviceID);
            deviceID = deviceID.Length < 2 ? deviceID : deviceID.Substring(deviceID.Length - 4);
        }
        else
        {
            Debug.Log("获取不到设备编号");
            return "0000";
        }

        return deviceID.ToUpper();
    }

    //public string GetMac()
    //{
    //    string result = "";

    //    if (Application.platform == RuntimePlatform.Android)
    //    {
    //        result = "000000000000";
    //    }
    //    else
    //    {
    //        string key = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\";
    //        string macAddress = string.Empty;
    //        try
    //        {
    //            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
    //            foreach (NetworkInterface adapter in nics)
    //            {
    //                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet
    //                    && adapter.GetPhysicalAddress().ToString().Length != 0)
    //                {
    //                    string fRegistryKey = key + adapter.Id + "\\Connection";
    //                    RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
    //                    if (rk != null)
    //                    {
    //                        string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
    //                        int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
    //                        if (fPnpInstanceID.Length > 3 &&
    //                            fPnpInstanceID.Substring(0, 3) == "PCI")
    //                        {
    //                            macAddress = adapter.GetPhysicalAddress().ToString();
    //                            //for (int i = 1; i < 6; i++)
    //                            //{
    //                            //    macAddress = macAddress.Insert(3 * i - 1, ":");
    //                            //}
    //                            break;
    //                        }
    //                    }

    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.Log(ex.Message);
    //        }
    //        Debug.Log(macAddress);
    //        StringBuilder stringBuilder = new StringBuilder();
    //        stringBuilder.Append(macAddress[10]);
    //        stringBuilder.Append(macAddress[3]);
    //        stringBuilder.Append(macAddress[5]);
    //        stringBuilder.Append(macAddress[11]);
    //        stringBuilder.Append(macAddress[1]);
    //        stringBuilder.Append(macAddress[7]);
    //        stringBuilder.Append(macAddress[2]);
    //        stringBuilder.Append(macAddress[8]);
    //        stringBuilder.Append(macAddress[6]);
    //        stringBuilder.Append(macAddress[0]);
    //        stringBuilder.Append(macAddress[4]);
    //        result = stringBuilder.ToString();
    //    }

    //    return result.ToLower();
    //}

    //public string GetTrueMac()
    //{
    //    string macAddress = "";

    //    if (Application.platform == RuntimePlatform.Android)
    //    {
    //        macAddress = "000000000000";
    //    }
    //    else
    //    {
    //        string key = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\";
    //        try
    //        {
    //            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
    //            foreach (NetworkInterface adapter in nics)
    //            {
    //                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet
    //                    && adapter.GetPhysicalAddress().ToString().Length != 0)
    //                {
    //                    string fRegistryKey = key + adapter.Id + "\\Connection";
    //                    RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
    //                    if (rk != null)
    //                    {
    //                        string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
    //                        int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
    //                        if (fPnpInstanceID.Length > 3 &&
    //                            fPnpInstanceID.Substring(0, 3) == "PCI")
    //                        {
    //                            macAddress = adapter.GetPhysicalAddress().ToString();
    //                            break;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.Log(ex.Message);
    //        }
    //        Debug.Log(macAddress);
    //    }

    //    return macAddress;
    //}
    public void ResetValue()
    {
        DateTime date = DateTime.Today;
        date = date.AddDays(30);
        endTime.year = date.Year;
        endTime.month = date.Month;
        endTime.day = date.Day;
        AvailableTime = 50;
        TipDay = 3;
        Debug.Log("<color=green>参数已恢复默认值</color>");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            if (DEMO != null && keyDownToShow != null)
            {
                keyDownToShow.SetActive(true);
            }
            else if (DEMO != null && keyDownToShow == null)
            {
                DEMO.SetActive(true);
            }
            else
            {
                DEMO = ShowDEMO();
            }
        }
    }

    public float buttonWidth = 100f;
    public float buttonHeight = 100f;
    public float fontSize = 20f;
    public Color fontColor = Color.white;
    public Color backgroundColor = Color.grey;
    public Color highlightColor = Color.yellow;
    public List<KeyCode> keyCodes;

    private RectTransform keyboardBackground;
    // public Text inputText;

    GameObject JianPanObject;

    /// <summary>
    /// 创建键盘
    /// </summary>
    private void GenerateKeyboard()
    {
        if (JianPanObject == null)
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            var scaleFactor = screenWidth / 1920f;
            var width = buttonWidth * scaleFactor;
            var height = buttonHeight * scaleFactor;
            var paddingX = width * 0.2f;
            var paddingY = height * 0.2f;
            var xStart = -screenWidth / 2 + paddingX + width / 2;
            var yStart = screenHeight / 2 - paddingY - height / 2;


            JianPanObject = new GameObject("键盘");
            JianPanObject.transform.SetParent(_demo, false);
            var JIanPnaTransform = JianPanObject.AddComponent<RectTransform>();
            //JIanPnaTransform.sizeDelta = new Vector2(0, screenHeight / 2);

            JIanPnaTransform.anchoredPosition = new Vector2(0, 0);
            JIanPnaTransform.localPosition = new Vector3(0, 0, 0);

            JIanPnaTransform.sizeDelta = new Vector2(0, 0);

            JIanPnaTransform.anchorMax = new Vector2(1, 1);
            JIanPnaTransform.anchorMin = new Vector2(0, 0);

            JIanPnaTransform.pivot = new Vector2(0.5f, 1);
            var _GridLayoutGroup = JianPanObject.AddComponent<GridLayoutGroup>();
            _GridLayoutGroup.padding.left = 199;
            _GridLayoutGroup.padding.right = 169;
            _GridLayoutGroup.padding.top = 25;

            _GridLayoutGroup.spacing = new Vector2(20, 10);
            _GridLayoutGroup.childAlignment = TextAnchor.LowerCenter;

            for (var i = 0; i < keyCodes.Count; i++)
            {
                var buttonText = GetButtonLabel(keyCodes[i]);
                var buttonObject = new GameObject(buttonText);
                buttonObject.transform.SetParent(JianPanObject.transform, false);

                var buttonTransform = buttonObject.AddComponent<RectTransform>();
                buttonTransform.sizeDelta = new Vector2(buttonWidth * (screenWidth / 1920), buttonHeight * (screenHeight / 1080));
                buttonTransform.anchoredPosition = new Vector2(xStart + i % 10 * (buttonWidth * screenWidth / 1920 + paddingX),
                    yStart - i / 10f * (buttonHeight * screenHeight / 1080 + paddingY));
                var buttonImage = buttonObject.AddComponent<Image>();
                buttonImage.color = backgroundColor;
                var button = buttonObject.AddComponent<Button>();
                button.onClick.AddListener(() => OnButtonClick(buttonText));


                var TextObject = new GameObject("text");

                TextObject.transform.SetParent(buttonObject.transform, false);
                var TextObjectRectTransform = TextObject.AddComponent<RectTransform>();
                TextObjectRectTransform.localPosition = new Vector3(0, 0, 0);

                TextObjectRectTransform.sizeDelta = new Vector2(0, 0);

                TextObjectRectTransform.anchorMax = new Vector2(1, 1);
                TextObjectRectTransform.anchorMin = new Vector2(0, 0);


                var buttonTextObject = TextObject.AddComponent<Text>();
                buttonTextObject.text = buttonText;
                _daXieAction += a => { buttonTextObject.text = a ? buttonText.ToUpper() : buttonText.ToLower(); };
                buttonTextObject.fontSize = (int)(fontSize * scaleFactor);


                buttonTextObject.color = fontColor;
                buttonTextObject.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                buttonTextObject.alignment = TextAnchor.MiddleCenter;
                buttonTextObject.fontStyle = FontStyle.Bold;
                buttonTextObject.resizeTextForBestFit = true;
                buttonTextObject.resizeTextMinSize = 1;
                buttonTextObject.resizeTextMaxSize = 30;

                var buttonColorBlock = new ColorBlock();
                buttonColorBlock.normalColor = backgroundColor;
                buttonColorBlock.highlightedColor = highlightColor;
                buttonColorBlock.pressedColor = backgroundColor;
                buttonColorBlock.colorMultiplier = 1f;
                button.colors = buttonColorBlock;
            }


            CreatButton(JianPanObject.transform, "大/小写", scaleFactor, DaXie);

            CreatButton(JianPanObject.transform, "删除", scaleFactor, ShanChu);

            CreatButton(JianPanObject.transform, "关闭", scaleFactor, CloseJianPan);
        }
        else
        {
            JianPanObject.gameObject.SetActive(true);
        }
    }


    public void CreatButton(Transform tran, string name, float b, System.Action action)
    {
        GameObject buttonObject2 = new GameObject(name);
        buttonObject2.transform.SetParent(tran, false);

        RectTransform buttonTransform2 = buttonObject2.AddComponent<RectTransform>();
        Image buttonImage2 = buttonObject2.AddComponent<Image>();
        buttonImage2.color = backgroundColor;
        Button button2 = buttonObject2.AddComponent<Button>();
        button2.onClick.AddListener(() => action());

        GameObject TextObject2 = new GameObject("text");

        TextObject2.transform.SetParent(buttonObject2.transform, false);
        RectTransform TextObjectRectTransform2 = TextObject2.AddComponent<RectTransform>();
        // TextObjectRectTransform2.sizeDelta = new Vector2(buttonWidth * screenWidth / 1920, buttonHeight * screenHeight / 1080);

        TextObjectRectTransform2.localPosition = new Vector3(0, 0, 0);

        TextObjectRectTransform2.sizeDelta = new Vector2(0, 0);

        TextObjectRectTransform2.anchorMax = new Vector2(1, 1);
        TextObjectRectTransform2.anchorMin = new Vector2(0, 0);

        Text buttonTextObject2 = TextObject2.AddComponent<Text>();
        buttonTextObject2.text = name;
        // buttonTextObject2.fontSize = (int)(fontSize * b / 2);
        buttonTextObject2.fontSize = (int)(fontSize * b / 2);
        buttonTextObject2.resizeTextMinSize = 1;
        buttonTextObject2.resizeTextMaxSize = 20;

        buttonTextObject2.color = fontColor;
        buttonTextObject2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        buttonTextObject2.alignment = TextAnchor.MiddleCenter;
        buttonTextObject2.fontStyle = FontStyle.Bold;
    }


    public void CloseJianPan()
    {
        JianPanObject.SetActive(false);
    }

    void ShanChu()
    {
        _inputField.text = _inputField.text.Substring(0, _inputField.text.Length - 1);
    }


    string GetButtonLabel(KeyCode keyCode)
    {
        if (keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9)
        {
            return (keyCode - KeyCode.Alpha0).ToString();
        }
        else if (keyCode >= KeyCode.A && keyCode <= KeyCode.Z)
        {
            return keyCode.ToString();
        }
        else
        {
            return "";
        }
    }

    public bool isDaXie = true;
    private Action<bool> _daXieAction;

    public void DaXie()
    {
        isDaXie = !isDaXie;
        _daXieAction?.Invoke(isDaXie);
    }


    public void OnButtonClick(string buttonText)
    {
        if (!isDaXie)
        {
            buttonText = buttonText.ToLower();
        }

        _inputField.text += buttonText;
    }
}

#region 网络加密类

public class NetWorkEncryption
{
    public static string NetWorkEncryptionData(NetWorkEncryptionRoot data)
    {
        return JsonUtility.ToJson(data);
    }

    /// <summary>RSAs the encrypt.RSA加密.</summary>
    /// <param name="publickey">The publickey.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static byte[] RSAEncrypt(string publickey, byte[] data)
    {
        var rsa = new RSACryptoServiceProvider();

        var key = Convert.FromBase64String(publickey);
        rsa.ImportCspBlob(key);

        var bytes = rsa.Encrypt(data, false);

        return bytes;
    }

    /// <summary>RSAs the decrypt.RSA解密.</summary>
    /// <param name="privatekey">The privatekey.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static byte[] RSADecrypt(string privatekey, byte[] data)
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        var key = Convert.FromBase64String(privatekey);
        Debug.Log(TCPTool.BytesToStringByEncoding(key, 0, key.Length, LFramework.StringType.UTF8));
        rsa.ImportCspBlob(key);


        var bytes = rsa.Decrypt(data, false);

        return bytes;
    }
}

[Serializable]
public class NetWorkEncryptionRoot
{
    [SerializeField] public bool success;
    [SerializeField] public string message;
    [SerializeField] public int code;
    [SerializeField] public NetWorkEncryptionData data;

    [Serializable]
    public class NetWorkEncryptionData
    {
        [SerializeField] public string createDate;
        [SerializeField] public string updateDate;
        [SerializeField] public int id;
        [SerializeField] public string projectName;
        [SerializeField] public string personInCharge;
        [SerializeField] public string projectAddress;
        [SerializeField] public string remoteCode;
        [SerializeField] public string remotePass;
        [SerializeField] public int frequency;
        [SerializeField] public int status;
        [SerializeField] public string endDate;
        [SerializeField] public string softwareCode;
        [SerializeField] public string nowDate;
        [SerializeField] public int secondNumber;
    }
}

#endregion

#region 工具类

[Serializable]
public struct Date
{
    public int year;
    public int month;
    public int day;
}

public static partial class EncryptionTool
{
    public static string GetLocalApplicationDataPath()
    {
        return PathTool.DataPath;
    }

    /// <summary>
    /// \Key\ist.cxp
    /// </summary>
    /// <returns></returns>
    public static string GetEncryptionKeyPath()
    {
        return GetLocalApplicationDataPath() + @"\Key\ist.cxp";
        ;
    }
}

public static partial class RsaHelper
{
    /// <summary>
    /// 签名算法
    /// </summary>
    public const string SIGNATURE_ALGORITHM = "MD5withRSA";

    private const string PUBLIC_KEY = "RSAPublicKey";

    /// <summary>
    /// C# 不能省略padding
    /// </summary>
    public const string KEY_ALGORITHM = "RSA//PKCS1PADDING";

    private const string PRIVATE_KEY = "RSAPrivateKey";

    /// <summary>
    /// 加密块
    /// </summary>
    private static readonly int MAX_ENCRYPT_BLOCK = 117;

    /// <summary>
    /// 解密块
    /// </summary>
    private static readonly int MAX_DECRYPT_BLOCK = 128;

    public static Dictionary<string, AsymmetricKeyParameter> GenKeyPair()
    {
        // 创建一个安全的随机数生成器
        var random = new SecureRandom();

        // 设置密钥生成参数，包括使用的随机数生成器和密钥长度
        var keyGenerationParameters = new KeyGenerationParameters(random, 1024);

        // 创建一个RSA密钥对生成器
        var generator = new RsaKeyPairGenerator();

        // 初始化密钥对生成器
        generator.Init(keyGenerationParameters);

        // 创建一个字典来存储公钥和私钥
        var obj = new Dictionary<string, AsymmetricKeyParameter>();

        // 生成RSA密钥对
        var keyPair = generator.GenerateKeyPair();

        // 将公钥和私钥添加到字典中
        obj.Add(PUBLIC_KEY, keyPair.Public);
        obj.Add(PRIVATE_KEY, keyPair.Private);

        // 返回包含公钥和私钥的字典
        return obj;
    }

    /// <summary>
    /// 私钥生成签名
    /// </summary>
    /// <param name="data">待签名的数据字节数组</param>
    /// <param name="privateKey">私钥</param>
    /// <returns></returns>
    public static string SignByPrivate(byte[] data, string privateKey)
    {
        // 创建私钥对象
        var paramters = PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));

        // 获取签名器对象
        var sig = SignerUtilities.GetSigner(SIGNATURE_ALGORITHM);

        // 初始化签名器
        sig.Init(true, paramters);

        // 获取待签名的数据字节数组
        var bytes = data;

        // 计算签名
        sig.BlockUpdate(bytes, 0, bytes.Length);
        byte[] signature = sig.GenerateSignature();

        // 对签名进行Base64编码，使其符合8位清洁标准
        var signedString = Convert.ToBase64String(signature);

        return signedString;
    }

    /// <summary>
    /// 公钥验证签名
    /// </summary>
    /// <param name="data">待验证的数据字节数组</param>
    /// <param name="publicKey">公钥</param>
    /// <param name="sign">签名</param>
    /// <returns></returns>
    private static bool VerifyByPublic(byte[] data, string publicKey, string sign)
    {
        // 创建公钥对象
        var paramters = PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));

        // 获取签名器对象
        var signer = SignerUtilities.GetSigner(SIGNATURE_ALGORITHM);

        // 初始化签名器
        signer.Init(false, paramters);

        // 将签名字符串转换为字节数组
        var expectedSig = Convert.FromBase64String(sign);

        // 将待签名的数据字节数组赋值给msgBytes
        var msgBytes = data;

        // 更新签名器
        signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
        // 验证签名
        return signer.VerifySignature(expectedSig);
    }

    /// <summary>
    /// 私钥加密
    /// </summary>
    /// <param name="data">被加密数据</param>
    /// <param name="privateKey">私钥</param>
    /// <returns></returns>
    public static byte[] EncryptByPrivateKey(byte[] data, string privateKey)
    {
        // 从私钥创建参数
        var privateKeyParam = (RsaKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));
        // 获取加解密算法
        var cipher = CipherUtilities.GetCipher(KEY_ALGORITHM);
        // 使用私钥初始化加密器
        cipher.Init(true, privateKeyParam);
        // 对数据进行加密操作，并返回加密后的字节数组
        return GetByteArray(data, cipher, MAX_ENCRYPT_BLOCK);
    }

    /// <summary>
    /// 私钥解密
    /// </summary>
    /// <param name="encryptedData">被解密数据</param>
    /// <param name="privateKey">私钥</param>
    /// <returns></returns>
    public static byte[] DecryptByPrivateKey(byte[] encryptedData, string privateKey)
    {
        // 从私钥创建参数
        var privateKeyParam = (RsaKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));
        // 获取加解密算法
        var cipher = CipherUtilities.GetCipher(KEY_ALGORITHM);
        // 使用私钥参数初始化解密器
        cipher.Init(false, privateKeyParam);
        // 对加密数据进行解密操作，并返回解密后的字节数组
        return GetByteArray(encryptedData, cipher, MAX_DECRYPT_BLOCK);
    }

    /// <summary>
    /// 公钥解密
    /// </summary>
    /// <param name="encryptedData">加密数据</param>
    /// <param name="publicKey">公钥</param>
    /// <returns></returns>
    public static byte[] DecryptByPublicKey(byte[] encryptedData, string publicKey)
    {
        var publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
        var cipher = CipherUtilities.GetCipher(KEY_ALGORITHM);
        cipher.Init(false, publicKeyParam);
        return GetByteArray(encryptedData, cipher, MAX_DECRYPT_BLOCK);
    }

    /// <summary>
    /// 公钥加密
    /// </summary>
    /// <param name="data">被加密数据</param>
    /// <param name="publicKey">公钥</param>
    /// <returns></returns>
    public static byte[] EncryptByPublicKey(byte[] data, string publicKey)
    {
        var publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
        var cipher = CipherUtilities.GetCipher(KEY_ALGORITHM);
        cipher.Init(true, publicKeyParam);
        return GetByteArray(data, cipher, MAX_ENCRYPT_BLOCK);
    }

    /// <summary>
    /// 分段加解密数据
    /// </summary>
    /// <param name="data">被处理数据</param>
    /// <param name="cipher">密码器</param>
    /// <param name="maxBlockSize">加密块\解密块 大小</param>
    /// <returns></returns>
    private static byte[] GetByteArray(byte[] data, IBufferedCipher cipher, int maxBlockSize)
    {
        var inputLen = data.Length;
        var outdatas = new MemoryStream(inputLen);
        var offSet = 0;
        byte[] cache;
        var i = 0;
        // 对数据分段加解密
        while (inputLen - offSet > 0)
        {
            if (inputLen - offSet > maxBlockSize)
            {
                cache = cipher.DoFinal(data, offSet, maxBlockSize);
            }
            else
            {
                cache = cipher.DoFinal(data, offSet, inputLen - offSet);
            }

            outdatas.Write(cache, 0, cache.Length);
            i++;
            offSet = i * maxBlockSize;
        }

        var endDate = outdatas.ToArray();
        outdatas.Close();
        return endDate;
    }
}

#endregion

#region 自定义UGUI类

public class CircleImage : Image
{
    /// <summary>
    /// 圆形由多少块三角形拼成
    /// </summary>
    [SerializeField] public int segements = 100;

    //显示部分占圆形的百分比.
    [SerializeField] public float showPercent = 1;
    public readonly Color32 GRAY_COLOR = new Color32(60, 60, 60, 255);
    public List<Vector3> _vertexList;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        _vertexList = new List<Vector3>();

        AddVertex(vh, segements);

        AddTriangle(vh, segements);
    }

    private void AddVertex(VertexHelper vh, int segements)
    {
        float width = rectTransform.rect.width;
        float heigth = rectTransform.rect.height;
        int realSegments = (int)(segements * showPercent);

        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
        float uvWidth = uv.z - uv.x;
        float uvHeight = uv.w - uv.y;
        Vector2 uvCenter = new Vector2(uvWidth * 0.5f, uvHeight * 0.5f);
        Vector2 convertRatio = new Vector2(uvWidth / width, uvHeight / heigth);

        float radian = (2 * Mathf.PI) / segements;
        float radius = width * 0.5f;

        Vector2 originPos = new Vector2((0.5f - rectTransform.pivot.x) * width, (0.5f - rectTransform.pivot.y) * heigth);
        Vector2 vertPos = Vector2.zero;

        Color32 colorTemp = GetOriginColor();
        UIVertex origin = GetUIVertex(colorTemp, originPos, vertPos, uvCenter, convertRatio);
        vh.AddVert(origin);

        int vertexCount = realSegments + 1;
        float curRadian = 0;
        Vector2 posTermp = Vector2.zero;
        for (int i = 0; i < segements + 1; i++)
        {
            float x = Mathf.Cos(curRadian) * radius;
            float y = Mathf.Sin(curRadian) * radius;
            curRadian += radian;

            if (i < vertexCount)
            {
                colorTemp = color;
            }
            else
            {
                colorTemp = GRAY_COLOR;
            }

            posTermp = new Vector2(x, y);
            UIVertex vertexTemp = GetUIVertex(colorTemp, posTermp + originPos, posTermp, uvCenter, convertRatio);
            vh.AddVert(vertexTemp);
            _vertexList.Add(posTermp + originPos);
        }
    }

    private Color32 GetOriginColor()
    {
        Color32 colorTemp = (Color.white - GRAY_COLOR) * showPercent;
        return new Color32(
            (byte)(GRAY_COLOR.r + colorTemp.r),
            (byte)(GRAY_COLOR.g + colorTemp.g),
            (byte)(GRAY_COLOR.b + colorTemp.b),
            255);
    }

    private void AddTriangle(VertexHelper vh, int realSegements)
    {
        int id = 1;
        for (int i = 0; i < realSegements; i++)
        {
            vh.AddTriangle(id, 0, id + 1);
            id++;
        }
    }

    private UIVertex GetUIVertex(Color32 col, Vector3 pos, Vector2 uvPos, Vector2 uvCenter, Vector2 uvScale)
    {
        UIVertex vertexTemp = new UIVertex();
        vertexTemp.color = col;
        vertexTemp.position = pos;
        vertexTemp.uv0 = new Vector2(uvPos.x * uvScale.x + uvCenter.x, uvPos.y * uvScale.y + uvCenter.y);
        return vertexTemp;
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint);
        return IsValid(localPoint);
    }

    private bool IsValid(Vector2 localPoint)
    {
        return GetCrossPointNum(localPoint, _vertexList) % 2 == 1;
    }

    private int GetCrossPointNum(Vector2 localPoint, List<Vector3> vertexList)
    {
        int count = 0;
        Vector3 vert1 = Vector3.zero;
        Vector3 vert2 = Vector3.zero;
        int vertCount = vertexList.Count;

        for (int i = 0; i < vertCount; i++)
        {
            vert1 = vertexList[i];
            vert2 = vertexList[(i + 1) % vertCount];

            if (IsYInRang(localPoint, vert1, vert2))
            {
                if (localPoint.x < GetX(vert1, vert2, localPoint.y))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool IsYInRang(Vector2 localPoint, Vector3 vert1, Vector3 vert2)
    {
        if (vert1.y > vert2.y)
        {
            return localPoint.y < vert1.y && localPoint.y > vert2.y;
        }
        else
        {
            return localPoint.y < vert2.y && localPoint.y > vert1.y;
        }
    }

    private float GetX(Vector3 vert1, Vector3 vert2, float y)
    {
        float k = (vert1.y - vert2.y) / (vert1.x - vert2.x);
        return vert1.x + (y - vert1.y) / k;
    }
}

#endregion


#region Editor

#if UNITY_EDITOR
[CustomEditor(typeof(Encryption))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Encryption myEncryption = (Encryption)target;

        if (GUILayout.Button("恢复预设值"))
        {
            myEncryption.ResetValue();
        }

        if (GUILayout.Button("重置"))
        {
            myEncryption.Chongzhi();
        }
    }
}


public class Editor_Encryption : Editor
{
    [MenuItem("GameObject/必须脚本/加密", false, -20)]
    static void OnCreate()
    {
        GameObject go = new GameObject("加密");
        Encryption encryption = go.AddComponent<Encryption>();
        encryption.ResetValue();
        Debug.Log("<color=yellow>加密模块已添加</color>");
    }
}

/// <summary>
/// 添加创建按钮
/// </summary>
class ChuanJianJiaMi
{
    //给Project里面加一些自定义的按钮之类的
    [InitializeOnLoadMethod]
    private static void Init2()
    {
        return;
        EditorApplication.projectWindowItemOnGUI += (guid, rect) =>
        {
            //当选定project当中的某个东西时
            if (Selection.activeObject != null)
            {
                //Unity通过给Asset文件夹下方物体设置某个特定的guid来判定到底是哪个物体
                var chosen_guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(Selection.activeObject));
                //Debug.Log(Selection.activeObject.GetType());

                //不光要对应上，而且不应为空
                if (Selection.activeObject.name == "Encryption" && chosen_guid == guid && !string.IsNullOrEmpty(chosen_guid) &&
                    Selection.activeObject.GetType().ToString() == "UnityEditor.MonoScript")
                {
                    rect.x = rect.width - 100;
                    rect.width = 100;

                    //创建一个用来删除某个物体的按钮
                    if (GUI.Button(rect, "创建"))
                    {
                        // AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(Selection.activeObject));
                        GameObject tGO = new GameObject("加密");
                        tGO.AddComponent<Encryption>();
                    }
                }
            }
        };
    }
}

#endif

#endregion