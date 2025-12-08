using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class SpeakTrigger : MonoBehaviour
{
    public int index;
    [ShowIf("ShowIf")]
    public float waitToPlayAnimTime = 0.5f;

    public bool ShowIf()
    {
        return triggerType == TriggerType.WaitToPlayAnim;
    }
    public enum TriggerType
    {
        Speak,
        StopPoint,
        AddSpeed,
        ReduceSpeed,
        StopButWaitTime,
        StopButWaitTimePlayAnim,
        WaitToPlayAnim,
    }

    public TriggerType triggerType;
    public UnityEvent onTriggerEnterEvent;

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger Entered" + name);
        if (other.CompareTag("Player"))
        {
           
            {
                var player2 = other.GetComponentInParent<BaseController>();
                if (player2 != null)
                {
                    onTriggerEnterEvent.Invoke();

                    switch (triggerType)
                    {
                        case TriggerType.Speak:
                            player2.Speak(index);
                            break;
                        case TriggerType.StopPoint:
                            player2.ArrivePoint();
                            break;
                        case TriggerType.AddSpeed:
                            player2.AddSpeed(index);
                            break;
                        case TriggerType.ReduceSpeed:
                            player2.ReduceSpeed(index);
                            break;
                        case TriggerType.StopButWaitTime:
                            player2.StopWaitTime(index);
                            break;
                        case TriggerType.StopButWaitTimePlayAnim:
                            player2.StopButWaitTimePlayAnim(index);
                            break;
                        case TriggerType.WaitToPlayAnim:
                            player2.StopButWaitTimePlayAnim(index, waitToPlayAnimTime);
                            break;
                    }
                }
            }
        }
    }
}