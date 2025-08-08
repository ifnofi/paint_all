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
    public List<BoyAnimeMgr> boyAnimeMgrs = new List<BoyAnimeMgr>();
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
    }

    private void BoysInit()
    {
        foreach (var boy in boyAnimeMgrs)
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
        Debug.Log("StartMove");
        BoysInit();
        boyIndex = 0;

        var doing = false;
        while (boyIndex <= boyAnimeMgrs.Count)
        {
            doing = true;
            boyAnimeMgrs[boyIndex].Show();
            boyAnimeMgrs[boyIndex].StartMove(() =>
            {
                boyAnimeMgrs[boyIndex].Hide();
                Debug.Log($"播放第{boyIndex}个主角动画");
                boyIndex++;
                doing = false;
            });
            yield return new WaitUntil(() => !doing);
        }

        Debug.Log("主角动画播放完毕" + boyIndex);
    }
}