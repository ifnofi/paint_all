using LFramework;
using UnityEngine;

public class ShowUIHelper : MonoBehaviour
{
    private MyToggleBtn myToggleBtn;

    public float delayTime = 1.5f;

    private void Awake()
    {
        myToggleBtn = GetComponent<MyToggleBtn>();
    }

    private IActionController i;

    public void ShowUI()
    {
        print("show ui " + name);
        if (i != null)
        {
            i.Deinit();
        }

        i = ActionKit.Sequence()
           .Callback((() =>
            {
                myToggleBtn.Choose();
            }))
           .Delay(delayTime, (() =>
            {
                myToggleBtn.UnChoose();
            }))
           .Start(this);
    }
}
