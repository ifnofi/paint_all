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

    public void ShowUI()
    {
        print("show ui " + name);

        TimeController.Kill(GetInstanceID() + "ShowUI", true);
        myToggleBtn.Choose();
        TimeController.Call(delayTime, () =>
        {
            myToggleBtn.UnChoose();
        }, GetInstanceID() + "ShowUI");
    }
}
