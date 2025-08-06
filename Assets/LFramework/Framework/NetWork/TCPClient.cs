using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using LFramework;
using Timer = System.Timers.Timer;

#if UNITY_EDITOR
#endif

[Serializable]
public class TCPClient
{
    public string ServerIP;
    public int ServerPort;
    public double waitTime = 5;

    public double testingWaitTime = 2;

    Socket socket;
    public bool autoSendConnect;
    public string autoSendString;
    Thread th;

    public StringType codeType = StringType.ASCII;

    public bool isConnected;

    public bool isOpen;

    public bool couldTesting;
    public string strSend = "FF";

    bool couldRec = true;

    public double testingTime = 2;

    Timer testingTimer;
    Timer timer;

    #region 客户端状态检测

    public delegate void BackMessageCallback();
    public delegate void BackMessageCallback<T>(T msg);
    public delegate void BackMessageCallback<T, X>(T t, X x);
    public delegate void BackMessageCallback<T, X, Y>(T t, X x, Y y);

    public event BackMessageCallback<object> DebugEnevt;
    public event BackMessageCallback<bool> OpenEvent;
    public event BackMessageCallback<bool> ConnectedEvent;

    public event BackMessageCallback<byte[], int, Socket> ReciveEvent;

    public event BackMessageCallback<bool> TestingChangeEvent;
    public event BackMessageCallback TestingEvent;

    #endregion

    public TCPClient()
    {
    }

    public TCPClient(string ip, int port, double tWT, double wT)
    {
        ServerIP = ip;
        ServerPort = port;
        testingWaitTime = tWT;
        waitTime = wT;
        Initialization();
    }

    public void Initialization()
    {
        ShowMsg("初始化客户端");
        testingTimer = new Timer(1000);
        testingTimer.AutoReset = true;
        testingTimer.Enabled = true;
        testingTimer.Elapsed += TestingTimer_Elapsed;
        timer = new Timer(waitTime * 1000);
        timer.AutoReset = true;
        timer.Enabled = true;
        timer.Elapsed += Timer_Elapsed;
    }

    public void ResetTestingTimer()
    {
        testingTimer.Close();
        testingTimer.Dispose();
        testingTimer = new Timer(1000);
        testingTimer.AutoReset = true;
        testingTimer.Enabled = true;
        testingTimer.Elapsed += TestingTimer_Elapsed;
    }

    /// <summary>
    /// 开关心跳
    /// </summary>
    public void SetHeartOpenOrClose(bool isOepn)
    {
        HeartisOpen = isOepn;
    }

    bool HeartisOpen = true;

    private void TestingTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        testingTime -= 1;
        if (HeartisOpen)
        {
            if (testingTime <= 0 && !couldTesting)
            {
                couldTesting = SetTesting(true);
            }
        }
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        if (couldTesting)
        {
            if (isOpen)
            {
                TestingEvent?.Invoke();
                if (isConnected)
                {
                    try
                    {
                        SendHeartBeat(strSend);
                    }
                    catch (Exception ex)
                    {
                        ShowMsg(ex.Message);
                        th.Abort();
                        isConnected = SetOpen(false);
                    }
                }
                else
                {
                    ShowMsg("断线重连");
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress ip = IPAddress.Parse(ServerIP);
                    IPEndPoint point = new IPEndPoint(ip, ServerPort);
                    try
                    {
                        th.Abort();
                        // Loom.RunAsync(() =>
                        // {
                        th = new Thread(Rec);
                        th.IsBackground = true;
                        th.Start(point);
                        // });
                    }
                    catch
                    {
                        ShowMsg("重连失败");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 和服务器沟通
    /// </summary>
    /// <param name="o">服务器的端口</param>
    void Rec(object o)
    {
        IPEndPoint point = o as IPEndPoint;
        try
        {
            socket.Connect(point);

            string mac = TCPTool.GetMacAddress(point.Address.ToString());

            ShowMsg("连接成功,目标MAC：" + mac);
            isConnected = SetConnected(true);
            if (autoSendConnect)
            {
                try
                {
                    string strSend = autoSendString;
                    byte[] buffer = TCPTool.StringToBytesByEncoding(strSend, codeType);
                    socket.Send(buffer);
                }
                catch
                {
                    ShowMsg("未知异常");
                    isConnected = SetConnected(false);
                    goto FliedReconnection;
                }
            }
        }
        catch
        {
            ShowMsg("连接失败");
            isConnected = SetConnected(false);
            goto FliedReconnection;
        }

        while (true)
        {
            if (couldRec)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024 * 5];
                    int dataLength = 0;
                    int index = 0;

                    dataLength = socket.Receive(buffer);
                    Thread.Sleep(100);
                    while (socket.Available > 0)
                    {
                        index = socket.Receive(buffer, dataLength, socket.Available, SocketFlags.None);
                        dataLength += index;
                        Thread.Sleep(100);
                    }

                    if (dataLength == 0)
                    {
                        ShowMsg("与服务器断开1");

                        isConnected = SetConnected(false);
                        break;
                    }

                    Loom.QueueOnMainThread(() =>
                    {
                        ReciveEvent?.Invoke(buffer, dataLength, socket);
                    });
                }
                catch
                {
                    if (socket.Connected == false)
                    {
                        ShowMsg("与服务器断开2");
                        isConnected = SetConnected(false);
                        break;
                    }
                }
            }
        }

    FliedReconnection:;
    }

    public void TryToConnect()
    {
        ShowMsg("初始化");
        isOpen = SetOpen(true);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            IPAddress ip = IPAddress.Parse(ServerIP);
            IPEndPoint point = new IPEndPoint(ip, ServerPort);
            if (th != null)
            {
                th.Abort();
            }

            Loom.RunAsync(() =>
            {
                th = new Thread(Rec);
                th.IsBackground = true;
                th.Start(point);
            });
        }
        catch
        {
            ShowMsg("连接失败,请检查IP及端口是否有效");
        }
    }

    public void DisConnect()
    {
        testingTimer.Close();
        testingTimer.Dispose();
        timer.Close();
        timer.Dispose();
        isOpen = SetOpen(false);
        try
        {
            if (th != null)
            {
                th.Abort();
            }

            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            socket.Dispose();
            isConnected = SetOpen(false);
        }
        catch (Exception e)
        {
            ShowMsg(e.Message);
        }
    }

    /// <summary>
    /// 发送字符串
    /// </summary>
    /// <param name="str">内容</param>
    public void SendString(string str)
    {
        if (!isConnected)
        {
            return;
        }
        couldTesting = SetTesting(false);
        testingTime = testingWaitTime;
        ResetTestingTimer();
        byte[] buffer = TCPTool.StringToBytesByEncoding(str, codeType);
        ShowMsg("发送：" + str);
        try
        {
            socket.Send(buffer);
        }
        catch
        {
            ShowMsg("未连接服务器");
        }
    }

    public void SendHeartBeat(string str)
    {
        if (!isConnected)
        {
            return;
        }
        ShowMsg("心跳:" + str);
        couldTesting = SetTesting(false);
        byte[] buffer = TCPTool.StringToBytesByEncoding(str, codeType);
        ShowMsg(str);
        try
        {
            socket.Send(buffer);
        }
        catch
        {
            ShowMsg("未连接服务器");
        }
    }

    public void SendObject(object o)
    {
        if (!isConnected)
        {
            return;
        }

        byte[] buffer = TCPTool.ObjectToBytes(o);

        couldTesting = SetTesting(false);
        testingTime = testingWaitTime;
        ResetTestingTimer();

        List<byte> bytes = new List<byte>();
        bytes.Add(0);
        bytes.AddRange(buffer);

        try
        {
            socket.Send(bytes.ToArray());
            ShowMsg("byte");
        }
        catch (Exception ex)
        {
            ShowMsg(ex.Message);
        }
    }

    /// <summary>
    /// 发送字符串
    /// </summary>
    /// <param name="str">内容</param>
    /// <param name="type">编码</param>
    public void SendString(string str, StringType type)
    {
        if (!isConnected)
        {
            return;
        }

        codeType = type;
        SendString(str);
    }

    public void SendBuffer(byte[] buffer)
    {
        if (!isConnected)
        {
            return;
        }
        //ShowMsg("buffer.Length:" + buffer.Length.ToString());
        couldTesting = SetTesting(false);
        testingTime = testingWaitTime;
        Thread th = new Thread(() =>
        {
            try
            {
                socket.Send(buffer);
            }
            catch
            {
                ShowMsg("未连接服务器");
            }
        });
        th.Start();
    }

    public void SendBuffer(byte[] buffer, int size)
    {
        ShowMsg("buffer.Length:" + buffer.Length.ToString());
        couldTesting = SetTesting(false);
        testingTime = testingWaitTime;
        ResetTestingTimer();
        Thread th = new Thread(() =>
        {
            try
            {
                socket.Send(buffer, 0, size, SocketFlags.None);
            }
            catch
            {
                ShowMsg("未连接服务器");
            }
        });
        th.Start();
    }

    void ShowMsg(string str)
    {
        Loom.QueueOnMainThread(() =>
        {
            DebugEnevt?.Invoke(str);
        });
    }

    bool SetOpen(bool b)
    {
        Loom.QueueOnMainThread(() =>
        {
            OpenEvent?.Invoke(b);
        });

        return b;
    }

    bool SetConnected(bool b)
    {
        Loom.QueueOnMainThread(() =>
        {
            ConnectedEvent?.Invoke(b);
        });
        return b;
    }

    bool SetTesting(bool b)
    {
        Loom.QueueOnMainThread(() =>
        {
            TestingChangeEvent?.Invoke(b);
        });

        return b;
    }
}