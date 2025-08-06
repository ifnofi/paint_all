using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using LFramework;


public class TCPServer
{
    public int severPort;

    private Socket socketWatch;
    private Socket socketSend;

    private Thread th;

    private List<Socket> sockets = new List<Socket>();

    public StringType codeType = StringType.ASCII;

    public int clientNum;

    public bool isListening;

    #region 服务端状态检测

    public delegate void BackMessageCallback<T>(T t);

    public delegate void BackMessageCallback<T, X>(T t, X x);
    public delegate void BackMessageCallback<T, X, Y>(T t, X x, Y y);

    //public event BackMessageCallback<string, SendData> BackMessageEvent; 
    public event BackMessageCallback<int> ClientConnectEvent;
    public event BackMessageCallback<bool> OpenEvent;

    public event BackMessageCallback<byte[], int, Socket> ReciveEvent;

    public event BackMessageCallback<string> DebugEvent;

    #endregion

    public TCPServer()
    {
    }

    public TCPServer(int port)
    {
        severPort = port;
    }

    public void StartListening()
    {
        SetOpen(true);
        //创建用来监听的Socket
        socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //创建ip地址和端口号
        //IPAddress ip = IPAddress.Parse(textServer.Text);
        var ip = IPAddress.Any;
        var point = new IPEndPoint(ip, severPort);

        //让Sockrt绑定ip跟端口号

        socketWatch.Bind(point);
        ShowMsg("开始监听");
        //设置监听队列
        socketWatch.Listen(20);
        //负责监听的Socket，来接收客户端的连接

        Loom.RunAsync(() =>
        {
            th = new Thread(Listen);
            th.IsBackground = true;
            th.Start();
        });

        isListening = true;
    }

    public void StopListening()
    {
        SetOpen(false);
        try
        {
            socketWatch.Shutdown(SocketShutdown.Both);
        }
        catch (Exception e)
        {
            ShowMsg(e.Message);
        }

        socketWatch.Close();
        for (var i = 0; i < sockets.Count; i++)
        {
            try
            {
                sockets[i].Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                ShowMsg(e.Message);
            }

            sockets[i].Close();
        }

        isListening = false;
    }

    public void SetListen()
    {
        if (isListening)
            StopListening();
        else
            StartListening();
    }

    public string ipMac;


    //public double waitTime = 50;
    private void Listen()
    {
        while (true)
            try
            {
                socketSend = socketWatch.Accept();
                var ipPort = socketSend.RemoteEndPoint.ToString();
                var ipPores = ipPort.Split(':');
                var ip = ipPores[0];
                var port = ipPores[1];
                var mac = TCPTool.GetMacAddress(ip);
                ShowMsg("IP:" + ip + ",Port:" + port + "\r\nMac:" + mac + "\r\n连接成功");

                Loom.RunAsync(() =>
                {
                    //服务端开始接收数据
                    var th = new Thread(Rec);
                    th.IsBackground = true;
                    th.Start(socketSend);
                });
            }
            catch (Exception e)
            {
                ShowMsg(e.Message + "停止监听");
                ShowMsg("停止监听");
                break;
            }
    }

    public int BufferSize = 65500;
    public int BufferHeadSize = 3;

    /// <summary>
    /// 与客户端通信
    /// </summary>
    /// <param name="o">客户端</param>
    private void Rec(object o)
    {
        var send = (Socket)o;
        sockets.Add(send);
        clientNum = SetClientNum(sockets.Count);
        var clientIP = send.RemoteEndPoint.ToString();

        while (true)
        {
            try
            {
                var buffer = new byte[BufferSize + BufferHeadSize];

                //服务端实际接收到的有效字节数
                var r = send.Receive(buffer);
                if (r == 0)
                {
                    sockets.Remove(send);
                    clientNum = SetClientNum(sockets.Count);
                    ShowMsg(clientIP + "：断开连接");
                    break;
                }

                //ShowMsg($"rec :{r},{buffer[0]}  {buffer[1]}    {buffer[2]}");

                Loom.QueueOnMainThread(() =>
                {
                    ReciveEvent?.Invoke(buffer, r, send);
                });
            }
            catch (Exception e)
            {
                ShowMsg(e.Message + ":::断开连接");
                ShowMsg(clientIP + "：断开连接");
                sockets.Remove(send);
                clientNum = SetClientNum(sockets.Count);
                break;
            }
        }
    }



    public void SendBuffer(byte[] buffer)
    {
        for (var i = 0; i < sockets.Count; i++) sockets[i].Send(buffer);
    }

    public void SendBuffer(byte[] buffer, Socket send)
    {
        send.Send(buffer);
    }

    public void ShowMsg(string str)
    {
        Loom.QueueOnMainThread(() =>
        {
            DebugEvent?.Invoke(str);
        });
    }


    /// <summary>
    /// 发送字符串
    /// </summary>
    /// <param name="str">内容</param>
    public void SendString(string str)
    {
        byte[] buffer;
        buffer = TCPTool.StringToBytesByEncoding(str, codeType);
        try
        {
            for (var i = 0; i < sockets.Count; i++) sockets[i].Send(buffer);
        }
        catch (Exception e)
        {
            ShowMsg(e.Message);
        }
    }

    public void SendString(string str, Socket send)
    {
        byte[] buffer;
        buffer = TCPTool.StringToBytesByEncoding(str, codeType);
        try
        {
            send.Send(buffer);
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
    /// <param name="type">编码</param>
    public void SendString(string str, StringType type)
    {
        codeType = type;
        SendString(str);
    }

    public bool SetOpen(bool b)
    {
        Loom.QueueOnMainThread(() =>
        {
            OpenEvent?.Invoke(b);
        });
        return b;
    }

    public int SetClientNum(int i)
    {
        Loom.QueueOnMainThread(() =>
        {
            ClientConnectEvent?.Invoke(i);
        });
        return i;
    }
}
