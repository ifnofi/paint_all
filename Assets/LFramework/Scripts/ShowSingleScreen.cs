using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using LFramework;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ShowSingleScreen))]
public class ShowSingleScreenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ShowSingleScreen myScript = (ShowSingleScreen)target;

        GUILayout.Label("提示:ESC切换鼠标显隐");

        if (GUILayout.Button("创建配置文件"))
        {
            myScript.ConfigInit();
        }
    }
}
#endif

public class ShowSingleScreen : MonoBehaviour
{
    #region DllImport

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetParent(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

    [DllImport("kernel32.dll")]
    public static extern void SetLastError(uint dwErrCode);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetSystemMetrics(int nIndex);

    #endregion

    private float lastwidth = 0f;
    private float lastheight = 0f;

    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16; //边框用的
    const int WS_POPUP = 0x800000;
    private static int SM_CMONITORS = 80; //获取屏幕数量指令
    int _ScreenNum = 0;

    private int _posX = 0;

    private int _posY = 0;

    // 在这里设置你想要的窗口宽
    private int _Txtwith = 1152;

    // 在这里设置你想要的窗口高
    private int _Txtheight = 576;

    private int _SetScreen;
    private string _SetFullScreen;

    private string[] defaultLines = new string[]
    {
        "0",
        "0",
        "0,0",
        "1152,576",
        "",
        "",
        "",
        "",
        "// 是否全屏",
        "// 屏幕编号",
        "// 程序的左上角位置",
        "// 程序的 长  和  宽",
        "// 空格控制鼠标的显示",
    };

    private string path = PathTool.DataPath + "/WindowConfig.txt";

    public void ConfigInit()
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        if (!File.Exists(path))
        {
            File.WriteAllLines(path, defaultLines);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }

    private void Awake()
    {
#if UNITY_EDITOR
        Cursor.visible = true; // 鼠标隐藏
#else
        Cursor.visible = false; // 鼠标隐藏
#endif
        try
        {
            ConfigInit();
            var settings = File.ReadAllLines(path);
            _SetScreen = Convert.ToInt32(settings[1]);

            _posX = Convert.ToInt32(settings[2].Split(',')[0]);
            _posY = Convert.ToInt32(settings[2].Split(',')[1]);


            _SetFullScreen = settings[0];

            _Txtwith = Convert.ToInt32(settings[3].Split(',')[0]);
            _Txtheight = Convert.ToInt32(settings[3].Split(',')[1]);
        }
        catch
        {
        }
    }

    void Start()
    {
        StartCoroutine(JianCe());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
        }
    }

    IEnumerator JianCe()
    {
        while (true)
        {
            if (lastwidth != Screen.width || lastheight != Screen.height)
            {
                lastwidth = Screen.width;
                lastheight = Screen.height;
                _ScreenNum = GetSystemMetrics(SM_CMONITORS);
                if (_SetFullScreen == "1" || _SetFullScreen == "是")
                {
                    if (_SetScreen > 0 && _SetScreen <= _ScreenNum - 1)
                    {
                        _posX = 0;
                        _posY = 0;
                        for (int i = 0; i < _SetScreen; i++)
                        {
                            _posX += Display.displays[i].systemWidth;
                        }

                        _Txtwith = Display.displays[_SetScreen].systemWidth;
                        _Txtheight = Display.displays[_SetScreen].systemHeight;
                    }
                    else
                    {
                        _Txtwith = Display.displays[0].systemWidth;
                        _Txtheight = Display.displays[0].systemHeight;
                        _posX = 0;
                        _posY = 0;
                    }
                }

                print("窗口");
                Screen.SetResolution(_Txtwith, _Txtheight, false);
                if (Application.platform != RuntimePlatform.WindowsEditor)
                {
                    StartCoroutine(Setposition());
                    print("打包出来了");
                }
                else
                {
                    print("WINDOWS EDITOR");
                }
            }

            yield return new WaitForSeconds(5f);
        }
    }


    IEnumerator Setposition()
    {
        yield return new WaitForSeconds(1f); //不知道为什么发布于行后，设置位置的不会生效，我延迟0.1秒就可以
        SetWindowLong(GetProcessWnd(), GWL_STYLE, WS_POPUP); //无边框
        bool result = SetWindowPos(GetProcessWnd(), 0, _posX, _posY, _Txtwith, _Txtheight, SWP_SHOWWINDOW); //设置屏幕大小和位置
    }

    public static IntPtr GetProcessWnd()
    {
        IntPtr ptrWnd = IntPtr.Zero;
        uint pid = (uint)Process.GetCurrentProcess().Id; // 当前进程 ID

        bool bResult = EnumWindows
        (
            new WNDENUMPROC
            (
                delegate(IntPtr hwnd, uint lParam)
                {
                    uint id = 0;

                    if (GetParent(hwnd) == IntPtr.Zero)
                    {
                        GetWindowThreadProcessId(hwnd, ref id);
                        if (id == lParam) // 找到进程对应的主窗口句柄
                        {
                            ptrWnd = hwnd; // 把句柄缓存起来
                            SetLastError(0); // 设置无错误
                            return false; // 返回 false 以终止枚举窗口
                        }
                    }

                    return true;
                }
            ), pid
        );

        return (!bResult && Marshal.GetLastWin32Error() == 0) ? ptrWnd : IntPtr.Zero;
    }
}