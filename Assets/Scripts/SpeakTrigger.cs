using System;
using UnityEngine;

public class SpeakTrigger : MonoBehaviour
{
    public int index;

    public enum TriggerType
    {
        Speak,
        StopPoint,
        AddSpeed,
        ReduceSpeed,
    }

    public TriggerType triggerType;

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger Entered" + name);
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponentInParent<BoyController>();
            if (player != null)
            {
                switch (triggerType)
                {
                    case TriggerType.Speak:
                        player.Speak(index);
                        break;
                    case TriggerType.StopPoint:
                        player.ArrivePoint();
                        break;
                    case TriggerType.AddSpeed:
                        player.AddSpeed();
                        break;
                    case TriggerType.ReduceSpeed:
                        player.ReduceSpeed(); 
                        break;
                }
            }
        }
    }
}