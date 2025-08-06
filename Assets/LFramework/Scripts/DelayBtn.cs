using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DelayBtn : MonoBehaviour
{
    private Button btn;
    public float delayTime = 0.5f;
    public bool isEnableEventTrigger = true;
    public bool isEnableDelay = true;

    private void Awake()
    {
        btn = GetComponent<Button>();
        var path = Application.streamingAssetsPath + "/DelayTime.txt";

        if (!File.Exists(path))
        {
            File.Create(path);
        }


        delayTime = float.Parse(File.ReadAllLines(path)[0]);

        btn.onClick.AddListener
        (
            () =>
            {
                if (isEnableDelay)
                {
                    btn.interactable = false;
                    InvokeController.Call
                    (
                        delayTime, () =>
                        {
                            btn.interactable = true;
                        }
                    );
                }
            }
        );


        if (isEnableEventTrigger)
        {
            var tr = gameObject.AddComponent<EventTrigger>();
            var trigger = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter,
            };

            trigger.callback.AddListener
            (
                (a) =>
                {
                    if (btn.interactable)
                    {
                        btn.onClick.Invoke();
                    }
                }
            );

            tr.triggers.Add(trigger);
        }
    }

}