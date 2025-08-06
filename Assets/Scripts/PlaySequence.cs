using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PlayS
{
    播放,
    暂停,
    停止,
    调试
}

public enum PlayStatus
{
    一遍,
    循环,
    来回,
    //结束跳转下段,
    //触发事件进入下一段
}

public enum EventStatus
{
    none,
    结束跳转下段,
    结束跳转某个,
    //触发事件进入下一段
}

[Serializable]
public class ActionXulie
{
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("基础数据")]
#endif
    public int MinNumber = 0;
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("基础数据")]
#endif
    public int MaxNumber = 1;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("基础数据")]
#endif
    public float Framerate = 20.0f;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("基础数据")]
#endif
    public PlayS _playS = PlayS.播放;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("基础数据")]
#endif
    public PlayStatus playStatus = PlayStatus.一遍;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("基础数据")]
#endif
    public EventStatus eventStatus = EventStatus.none;

    private bool showJumpTo => eventStatus == EventStatus.结束跳转某个;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("基础数据")] [Sirenix.OdinInspector.ShowIf("showJumpTo")]
#endif
    public int jumpTo;

    [HideInInspector] public PlaySequence owner;

    /// <summary>
    /// 当前播放的方向
    /// </summary>
    /// 
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("基础数据")]
#endif
    public bool currentIsForward = true;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.PropertyRange("MinNumber", "MaxNumber")] [Sirenix.OdinInspector.TabGroup("基础数据")]
        [Sirenix.OdinInspector.OnValueChanged("OnRangeChanged1")] 
#endif
    public int currentFrameIndex = 0;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("基础数据")]
#endif
    public float timer = 0f; // 用于计时的变量

    public void Reset()
    {
        _playS = PlayS.播放;
        if (!currentIsForward)
        {
            currentFrameIndex = MaxNumber;
        }
        else
        {
            currentFrameIndex = MinNumber;
        }
    }

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.MinMaxSlider("MinNumber", "MaxNumber", true)] [Sirenix.OdinInspector.OnValueChanged("OnRangeChanged")] [Sirenix.OdinInspector.TabGroup("编辑数据")]
#endif
    public Vector2Int range = new Vector2Int(0, 1);

    private Vector2Int previousRange;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("基础数据")]
#endif
    public UnityEvent OverEvents;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("编辑数据")]
#endif
    private void OnRangeChanged()
    {
        if (range.x != previousRange.x)
        {
            currentFrameIndex = range.x;
        }

        if (range.y != previousRange.y)
        {
            currentFrameIndex = range.y;
        }

        // 更新 previousRange 为当前的 range
        previousRange = range;
        owner.Update();
    }

    public void Check()
    {
        if (currentFrameIndex < MinNumber)
        {
            currentFrameIndex = MinNumber;
        }

        if (currentFrameIndex > MaxNumber)
        {
            currentFrameIndex = MaxNumber;
        }
    }


    private void OnRangeChanged1()
    {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {Check();
               
            owner.Update();
            Check();

            }
#endif

    }

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.TabGroup("编辑数据")]
    [Sirenix.OdinInspector.Button("根据范围创建序列", Sirenix.OdinInspector.ButtonSizes.Large, Sirenix.OdinInspector.ButtonStyle.CompactBox)]
#endif
    public void CreateXulieByRange()
    {
        var xulie = new ActionXulie
        {
            MinNumber = range.x,
            MaxNumber = range.y,
            Framerate = Framerate,
            _playS = _playS,
            playStatus = PlayStatus.一遍,
            eventStatus = EventStatus.结束跳转下段,
            jumpTo = jumpTo,
            currentIsForward = true,
            currentFrameIndex = range.x
        };
        xulie.range = new Vector2Int(xulie.MinNumber, xulie.MaxNumber);
        xulie.OverEvents = OverEvents.Copy();
        xulie.owner = owner;
        owner._xulies.Add(xulie);

        range = new Vector2Int(MinNumber, MaxNumber);
        owner.Save();
    }
}

public static class UnityEventExtensions
{
    public static UnityEvent Copy(this UnityEvent sourceEvent)
    {
        UnityEvent copiedEvent = new UnityEvent();

        // 反射获取源事件的所有监听器
        var sourceListeners = sourceEvent.GetPersistentEventCount();
        for (int i = 0; i < sourceListeners; i++)
        {
            // 获取每个监听器的信息
            var target = sourceEvent.GetPersistentTarget(i);
            var methodName = sourceEvent.GetPersistentMethodName(i);

            // 将监听器添加到新事件中
            if (target != null && !string.IsNullOrEmpty(methodName))
            {
                copiedEvent.AddListener((UnityAction)System.Delegate.CreateDelegate(typeof(UnityAction), target, methodName));
            }
        }

        return copiedEvent;
    }
}

/// <summary>
/// 序列帧管理
/// </summary>
public class PlaySequence : MonoBehaviour
{
    public List<Image> _images;

    //初始化 播放
    public bool PlayOnStart;

    //当前的序列帧动画
    public List<Sprite> _image_list = new List<Sprite>();

    //序列管理类
    public List<ActionXulie> _xulies;

    //当前帧索引 [是全局帧]
    private int currentFrameIndex = 0;

    //当前序列索引
    private int currentXulieIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (_image_list.Count <= 0)
        {
            Debug.Log("请添加序列");
        }

        foreach (var actionXuly in _xulies)
        {
            actionXuly.owner = this;
        }

        if (PlayOnStart && _xulies.Count > 0)
            Sequence_Play(0);
    }

    /// <summary>
    /// 播放序列
    /// </summary>
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button]
#endif
    public void Sequence_Play(int a)
    {
        currentXulieIndex = a;
        _xulies[currentXulieIndex].Reset();
        currentFrameIndex = 0;
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Sequence_Puse()
    {
        _xulies[currentXulieIndex]._playS = PlayS.暂停;
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Sequence_Stop()
    {
        _xulies[currentXulieIndex]._playS = PlayS.停止;
    }

    private float timer = 0f; // 用于计时的变量

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button("序列Owner 防报空")]
#endif
    public void Owner()
    {
        foreach (var actionXuly in _xulies)
        {
            actionXuly.owner = this;
        }
    }

    public void Update()
    {
        if (_xulies.Count <= 0)
            return;

        //更新序列帧
        UpdateXulie();


        //获取当前帧索引
        currentFrameIndex = _xulies[currentXulieIndex].currentFrameIndex;
        if (currentFrameIndex >= _image_list.Count || currentFrameIndex < 0)
        {
            return;
        }

        //更新图片
        for (int i = 0; i < _images.Count; i++)
        {
            if (i < _image_list.Count)
            {
                _images[i].sprite = _image_list[currentFrameIndex];
            }
            else
            {
                _images[i].sprite = null;
            }
        }

        if (!Application.isPlaying)
        {
#if UNITY_EDITOR
            Save();

#endif
        }
    }

    private void UpdateXulie()
    {
        var currentXulie = _xulies[currentXulieIndex];


        if (currentFrameIndex < currentXulie.MinNumber)
        {
            currentFrameIndex = currentXulie.MinNumber;
        }

        if (currentFrameIndex > currentXulie.MaxNumber)
        {
            currentFrameIndex = currentXulie.MaxNumber;
        }


        currentXulie.Check();



        if (currentXulie._playS == PlayS.播放)
        {
            timer += Time.deltaTime;
            // print(timer);
            // 正向播放
            if (currentXulie.currentIsForward)
            {
                // 如果经过的时间大于或等于 1/p 秒，则进行更新
                if (timer >= 1f / currentXulie.Framerate)
                {
                    currentXulie.currentFrameIndex++;
                    // 重置计时器
                    timer = 0f;
                }

                // print(timer + "  " + currentXulie.currentFrameIndex + "  " + currentXulie.MaxNumber);

                //播放结束
                if (currentXulie.currentFrameIndex >= currentXulie.MaxNumber)
                {
                    currentXulie.OverEvents?.Invoke();
                    switch (currentXulie.playStatus)
                    {
                        case PlayStatus.一遍:
                            currentXulie._playS = PlayS.停止;
                            //停在最后一帧
                            currentXulie.currentFrameIndex = currentXulie.MaxNumber;
                            if (currentXulie.eventStatus == EventStatus.结束跳转下段)
                            {
                                NextXulie();
                            }
                            // else if (currentXulie.eventStatus == EventStatus.结束跳转某个)
                            // {
                            //     //跳转到指定序列
                            //     if (currentXulie.jumpTo >= 0 && currentXulie.jumpTo < _xulies.Count)
                            //     {
                            //         Sequence_Play(currentXulie.jumpTo);
                            //     }
                            //     else
                            //     {
                            //         Debug.LogError("跳转的序列索引不正确: " + currentXulie.jumpTo);
                            //     }
                            // }


                            break;
                        case PlayStatus.循环:
                            currentXulie.currentFrameIndex = currentXulie.MinNumber;
                            break;
                        case PlayStatus.来回:
                            currentXulie.currentIsForward = !currentXulie.currentIsForward;
                            break;
                    }
                }
            }
            else
            {
                // 如果经过的时间大于或等于 1/p 秒，则进行更新
                if (timer >= 1f / currentXulie.Framerate)
                {
                    currentXulie.currentFrameIndex--;
                    // 重置计时器
                    timer = 0f;
                }

                //播放结束
                if (currentXulie.currentFrameIndex <= currentXulie.MinNumber)
                {
                    currentXulie.OverEvents?.Invoke();


                    switch (currentXulie.playStatus)
                    {
                        case PlayStatus.一遍:
                            currentXulie._playS = PlayS.停止;
                            //停在最后一帧
                            currentXulie.currentFrameIndex = currentXulie.MinNumber;
                            if (currentXulie.eventStatus == EventStatus.结束跳转下段)
                            {
                                NextXulie();
                            }
                            else if (currentXulie.eventStatus == EventStatus.结束跳转某个)
                            {
                                //跳转到指定序列
                                if (currentXulie.jumpTo >= 0 && currentXulie.jumpTo < _xulies.Count)
                                {
                                    Sequence_Play(currentXulie.jumpTo);
                                }
                                else
                                {
                                    Debug.LogError("跳转的序列索引不正确: " + currentXulie.jumpTo);
                                }
                            }


                            break;
                        case PlayStatus.循环:
                            currentXulie.currentFrameIndex = currentXulie.MaxNumber;
                            break;
                            // case PlayStatus.来回一遍:
                            //     currentXulie._playS = PlayS.停止;
                            //     //停在最后一帧
                            //     currentXulie.currentFrameIndex = currentXulie.MinNumber;
                            //     if (currentXulie.eventStatus == EventStatus.结束跳转下段)
                            //     {
                            //         NextXulie();
                            //     }
                            //     else if (currentXulie.eventStatus == EventStatus.结束跳转某个)
                            //     {
                            //         //跳转到指定序列
                            //         if (currentXulie.jumpTo >= 0 && currentXulie.jumpTo < _xulies.Count)
                            //         {
                            //             Sequence_Play(currentXulie.jumpTo);
                            //         }
                            //         else
                            //         {
                            //             Debug.LogError("跳转的序列索引不正确: " + currentXulie.jumpTo);
                            //         }
                            //     }

                            break;
                        case PlayStatus.来回:
                            currentXulie.currentIsForward = !currentXulie.currentIsForward;
                            break;
                    }
                }
            }
        }
    }

    private void NextXulie()
    {
        var next = currentXulieIndex + 1;
        if (next > _xulies.Count - 1)
        {
            //如果没有下一个序列了
            Debug.Log("没有下一个序列了");
            return;
        }
        else if (next < 0)
        {
            //如果没有上一个序列了
            Debug.Log("没有上一个序列了");
            return;
        }
        else if (next < _xulies.Count)
        {
            Sequence_Play(next);
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public void Save()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        // Debug.Log("保存成功");
#endif
    }
}