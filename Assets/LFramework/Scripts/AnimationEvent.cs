using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public bool isEnable = true;
    public List<AnEvent> list = new List<AnEvent>();

    public void Trigger(string anName)
    {
        if(!isEnable)return;
            
        list.Find(a => a.anName == anName).unityEvent.Invoke();
    }
    
}


[Serializable]
public class AnEvent
{
    public string anName;
    public UnityEvent unityEvent;
}