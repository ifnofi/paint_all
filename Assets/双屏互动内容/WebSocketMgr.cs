using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LFramework;
using UnityEngine;
using UnityWebSocket;

public class WebSocketMgr : MonoSingleton<WebSocketMgr>
{
    private IWebSocket socket;
    private CancellationTokenSource _cts;
    private string _url;
    private bool _manualClose = false;

    // 收到的原始文本消息队列，保证在 Unity 主线程处理
    private readonly Queue<string> _recvQueue = new Queue<string>();
    private readonly object _queueLock = new object();

    // JSON 路由：根据 JSON 中的 "type" 字段分发
    // key = type 字符串, value = list of handlers (each handler receives raw json string and internally解析为具体类型)
    private readonly Dictionary<string, List<Action<string>>> _jsonHandlers = new Dictionary<string, List<Action<string>>>();

    // 重连配置
    public int MaxReconnectAttempts = 8;
    public int BaseReconnectDelayMs = 1000; // 基础延迟
    public int MaxReconnectDelayMs = 30000; // 最大延迟

    // 心跳（可选）
    public int HeartbeatIntervalSeconds = 20;
    private Task _heartbeatTask;

    // 主线程处理收到的消息并分发
    private void Update()
    {
        ProcessRecvQueue();
    }

    // 将收到的消息放入队列（从 WebSocket 回调线程或同线程都可安全调用）
    private void EnqueueMessage(string message)
    {
        lock (_queueLock)
        {
            _recvQueue.Enqueue(message);
        }
    }

    private void ProcessRecvQueue()
    {
        while (true)
        {
            string msg = null;
            lock (_queueLock)
            {
                if (_recvQueue.Count > 0) msg = _recvQueue.Dequeue();
            }

            if (msg == null || !(msg.Contains("{") && msg.Contains("}"))) break;

            // 分发：先触发原始文本处理器（如果有）
            // 然后尝试按照 JSON 中的 "type" 字段路由到注册的处理器
            try
            {
                // GBY-桂博园、YQLYJD-义桥露营基地、WCG文昌阁、BNXZS-百年香樟树、ALT-安乐塔
                // （简单解析，不依赖第三方 JSON 库）
                var data = JSON.Parse(msg);
                var message = "";
                if (data && data.num)
                {
                    switch (((string)data.num))
                    {
                        case "1":
                            message = "前程似锦";
                            break;
                        case "2":
                            message = "风调雨顺";
                            break;
                        case "3":
                            message = "金榜题名";
                            break;
                        case "4":
                            message = "匠心独运";
                            break;
                        case "5":
                            message = "财源广进";
                            break;
                    }
                }


                if (data && data.type)
                {
                    DualScreenControl.Instance.Open(data.type, data.num ? message : "");
                }
                else
                {
                    Debug.LogWarning("无法解析收到的消息为 JSON");
                    continue;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"ProcessRecvQueue 错误: {ex}");
            }
        }
    }

    private void Start()
    {
        _url = "ws://120.26.8.184:10110/meeting_websocket/ID=YHDFHZ001"; // 默认 URL，可根据需要修改
        _url = IniTool.GetValue("WebSocket", "URL", PathTool.ConfigPath, _url);
        Connect(_url);
    }

    // 连接入口
    public void Connect(string url)
    {
        _manualClose = false;
        _url = url;
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        _ = ConnectWithRetryAsync(_cts.Token);
        StartHeartbeat(_cts.Token);
    }

    // 主动断开
    public void Close()
    {
        _manualClose = true;
        _cts?.Cancel();
        _ = CloseSocketAsync();
    }

    // 发送任意对象（会序列化为 JSON）
    public void SendJson<T>(T obj)
    {
        if (socket == null || socket.ReadyState != WebSocketState.Open) return;
        try
        {
            string json = JsonUtility.ToJson(obj);
            socket.SendAsync(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"SendJson 错误: {ex}");
        }
    }

    // 关闭 socket 的异步封装
    private UniTaskVoid CloseSocketAsync()
    {
        try
        {
            if (socket != null)
            {
                socket.CloseAsync();
                socket = null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"CloseSocketAsync 错误: {ex}");
        }

        return new UniTaskVoid();
    }

    // 连接并且在失败时重试（带指数退避）
    private async UniTask ConnectWithRetryAsync(CancellationToken token)
    {
        int attempt = 0;
        while (!token.IsCancellationRequested && !_manualClose)
        {
            attempt++;
            try
            {
                await EstablishConnectionAsync(token);
                // 成功连接后返回（事件关闭或错误时会触发重连逻辑）
                return;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"WebSocket 连接失败 (attempt {attempt}): {ex.Message}");
                if (attempt >= MaxReconnectAttempts)
                {
                    Debug.LogWarning("达到最大重连次数，停止重连。");
                    return;
                }

                int delay = Math.Min(BaseReconnectDelayMs * (1 << (attempt - 1)), MaxReconnectDelayMs);
                // 加入一定随机抖动
                var jitter = UnityEngine.Random.Range(0, 500);
                try { await Task.Delay(delay + jitter, token); }
                catch (TaskCanceledException) { return; }
            }
        }
    }

    // 建立单次连接并注册事件
    private UniTask EstablishConnectionAsync(CancellationToken token)
    {
        var tcs = new TaskCompletionSource<bool>();
        try
        {
            socket = new WebSocket(_url);

            socket.OnOpen += (s, e) =>
            {
                Debug.Log($"[WebSocket] 已连接: {_url}");
                tcs.TrySetResult(true);
            };

            socket.OnMessage += (s, e) =>
            {
                if (e.IsText)
                {
                    print(e.Data);
                    EnqueueMessage(e.Data);
                }
                else if (e.IsBinary)
                {
                    // 若需要处理二进制，可在此添加
                    Debug.Log("[WebSocket] 收到二进制数据");
                }
            };

            socket.OnClose += async (s, e) =>
            {
                Debug.Log($"[WebSocket] 已关闭: Code={e.StatusCode}, Reason={e.Reason}");
                // 如果不是手动关闭，启动重连
                if (!_manualClose && !token.IsCancellationRequested)
                {
                    // 启动一个新的重连任务（若外层 ConnectWithRetry 正在运行则会处理）
                    await ConnectWithRetryAsync(_cts?.Token ?? CancellationToken.None);
                }
            };

            socket.OnError += (s, e) =>
            {
                Debug.LogWarning($"[WebSocket] 错误: {e.Message}");
                // 发生错误时关闭当前 socket，触发重连
                try { socket.CloseAsync(); }
                catch { }
            };

            socket.ConnectAsync();
        }
        catch (Exception ex)
        {
            tcs.TrySetException(ex);
        }

        // 如果 token 被取消，则尝试取消
        if (token != CancellationToken.None)
        {
            token.Register(() => tcs.TrySetCanceled());
        }

        return tcs.Task.AsUniTask();
    }

    // 启动简单的心跳任务（可用于保持连接活跃）
    private void StartHeartbeat(CancellationToken token)
    {
        if (_heartbeatTask != null && !_heartbeatTask.IsCompleted) return;
        _heartbeatTask = Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(HeartbeatIntervalSeconds), token);
                    if (token.IsCancellationRequested) break;
                    if (socket != null && socket.ReadyState == WebSocketState.Open)
                    {
                        // 发送一个轻量心跳 JSON，如果服务器期望 ping/pong，请改为发送对应帧
                        var ping = new { type = "heartbeat", ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() };
                        var json = JsonUtility.ToJson(ping);
                        socket.SendAsync(json);
                    }
                }
                catch (TaskCanceledException) { break; }
                catch (Exception ex) { Debug.LogWarning($"Heartbeat 错误: {ex}"); }
            }
        }, token);
    }

    protected override void OnApplicationQuit()
    {
        _manualClose = true;
        _cts?.Cancel();
        _ = CloseSocketAsync();
    }
}
