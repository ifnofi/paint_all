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
        ActionKit.Sequence().Callback((() =>
        {
            myToggleBtn.Click();
        })).Delay(delayTime).Callback((() =>
        {
            myToggleBtn.Click();
        })).Start(this);
    }
}