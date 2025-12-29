using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using DG.Tweening;
using LFramework;
using UnityEngine;
using UnityEngine.UI;

public enum MoveState
{
    OnlyMove, // 仅位移
    AllChange // 位移 缩放 旋转
}

public class GameMgr : MonoSingleton<GameMgr>
{
    public TCPServer tcpServer;
    public List<BaseController> BaseControllers = new List<BaseController>();
    private int boyIndex = 0;
    public Button startBtn;

    private void Start()
    {
        BoysInit();
        startBtn.onClick.AddListener(CharacterMoveStart);
        // audioSources = get
        tcpServer = new TCPServer(1234);
        tcpServer.ReciveEvent += TcpServerOnReciveEvent;
        tcpServer.DebugEvent += Debug.Log;
        tcpServer.codeType = StringType.UTF8;
        tcpServer.StartListening();
    }

    public void OnDestroy()
    {
        if (tcpServer != null)
        {
            tcpServer.ReciveEvent -= TcpServerOnReciveEvent;
            tcpServer.DebugEvent -= Debug.Log;
            tcpServer.StopListening();
        }
    }

    private void TcpServerOnReciveEvent(byte[] t, int x, Socket y)
    {
        var rec = TCPTool.BytesToStringByEncoding(t, 0, x, tcpServer.codeType);
        Debug.Log($"接收到数据: {rec}");
        if (rec == "start")
        {
            CharacterMoveStart();
        }
        else if (rec == "stop")
        {
            CharacterMoveStop();
        }
        else if (rec == "pause")
        {
            CharacterMovePause();
        }
        else if (rec == "unpause")
        {
            CharacterMoveUnPause();
        }
        else
        {
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CharacterMoveStart();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterMoveStop();
        }
    }

    private void BoysInit()
    {
        

        foreach (var baseController in BaseControllers)
        {
            baseController.Init();
            baseController.Hide();
        }
    }

    public bool IsPlaying;

    public void CharacterMoveStart()
    {
        DualScreenControl.Instance.HideAll();
        IsPlaying = true;
        CharacterMoveStop();
        var sequence = StoneControl.Instance.Play(RoleBegin);
        sequence.OnComplete(() =>
        {
            IsPlaying = false;
        }).Play();
    }

    public void RoleBegin()
    {
        if (characterMoveCoroutine != null)
        {
            StopCoroutine(characterMoveCoroutine);
        }

        foreach (var audioSource in audioSources)
        {
            audioSource.UnPause();
        }

        Time.timeScale = 1;
        characterMoveCoroutine = StartCoroutine(CharacterMove());
    }

    public void CharacterMoveStop()
    {
        if (characterMoveCoroutine != null)
        {
            StopCoroutine(characterMoveCoroutine);
        }

        foreach (var audioSource in audioSources)
        {
            audioSource.UnPause();
        }

        Time.timeScale = 1;
        BoysInit();
        IsPlaying = false;
    }

    public void CharacterMovePause()
    {
        foreach (var audioSource in audioSources)
        {
            audioSource.Pause();
        }

        Time.timeScale = 0;
    }

    public void CharacterMoveUnPause()
    {
        Time.timeScale = 1;
        foreach (var audioSource in audioSources)
        {
            audioSource.UnPause();
        }
    }

    public List<AudioSource> audioSources = new List<AudioSource>();
    private Coroutine characterMoveCoroutine;


    private IEnumerator CharacterMove()
    {
        Debug.Log("CharacterMove");
        BoysInit();
        boyIndex = 0;

        var doing = false;
       
        {
            while (boyIndex < BaseControllers.Count)
            {
                doing = true;
                // BaseControllers[boyIndex].Show();
                BaseControllers[boyIndex].Play(() =>
                {
                    BaseControllers[boyIndex].HideReset();
                    Debug.Log($"播放第{boyIndex}个主角动画");
                    boyIndex++;
                    doing = false;
                });
                yield return new WaitUntil(() => !doing);
            }
        }


        Debug.Log("主角动画播放完毕" + boyIndex);
    }
}
