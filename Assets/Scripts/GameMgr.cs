using System;
using System.Collections;
using System.Collections.Generic;
using LFramework;
using UnityEngine;
using UnityEngine.UI;

public enum MoveState
{
    OnlyMove, // 仅位移
    AllChange // 位移 缩放 旋转
}

public class GameMgr : MonoBehaviour
{
    public List<BoyController> BoyControllers = new List<BoyController>();
    private int boyIndex = 0;
    public Button startBtn;

    private void Start()
    {
        BoysInit();
        startBtn.onClick.AddListener(CharacterMoveStart);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CharacterMoveStart();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterMoveStart();
        }
    }

    private void BoysInit()
    {
        foreach (var boy in BoyControllers)
        {
            boy.Init();
            boy.Hide();
        }
    }

    public void CharacterMoveStart()
    {
        if (characterMoveCoroutine != null)
        {
            StopCoroutine(characterMoveCoroutine);
        }

        characterMoveCoroutine = StartCoroutine(CharacterMove());
    }

    public void CharacterMoveStop()
    {
        if (characterMoveCoroutine != null)
        {
            StopCoroutine(characterMoveCoroutine);
        }
    }

    private Coroutine characterMoveCoroutine;

    private IEnumerator CharacterMove()
    {
        Debug.Log("CharacterMove");
        BoysInit();
        boyIndex = 0;

        var doing = false;
        while (boyIndex <= BoyControllers.Count)
        {
            doing = true;
            BoyControllers[boyIndex].Show();
            BoyControllers[boyIndex].Play(() =>
            {
                BoyControllers[boyIndex].Hide();
                Debug.Log($"播放第{boyIndex}个主角动画");
                boyIndex++;
                doing = false;
            });
            yield return new WaitUntil(() => !doing);
        }

        Debug.Log("主角动画播放完毕" + boyIndex);
    }
}