using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InvokeController : MonoBehaviour
{
    private static InvokeController instance;
    private static InvokeController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject temp = new GameObject("InvokeController");
                instance = temp.AddComponent<InvokeController>();
                instance.callBackList = new List<CallBackT>(10);
            }

            return instance;
        }
    }

    private List<CallBackT> callBackList;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < callBackList.Count; i++)
        {
            callBackList[i]._time -= Time.deltaTime;
            if (callBackList[i]._time <= 0.0f)
            {
                callBackList[i]._delegate();
                try
                {
                    this.callBackList.RemoveAt(i);
                }
                catch
                {
                }
                i--;
            }
        }
    }

    public static void Call(float _time, System.Action _func, string id = default)
    {
        Instance.callBackList.Add(new CallBackT(_func, _time, id));
    }

    public static void Kill(string id)
    {
        if (Instance.callBackList.Count <= 0)
            return;
        for (int i = 0; i < Instance.callBackList.Count; i++)
        {
            int li = i;
            if (Instance.callBackList[i]._id == id)
            {
                Instance.callBackList.RemoveAt(li);
            }
        }
    }

    public static void StopAll()
    {
        Instance.callBackList.Clear();
    }
}

public class CallBackT
{
    public CallBackT() { }
    public CallBackT(System.Action _d, float _t)
    {
        this._delegate = _d;
        this._time = _t;
    }

    public CallBackT(Action @delegate, float time, string id) : this(@delegate, time)
    {
        _id = id;
    }

    public System.Action _delegate;
    public float _time;
    public string _id;
}
