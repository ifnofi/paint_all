using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ClickEventTrigger : MonoBehaviour
{
    public Button button; // 按钮
    [Header("需要的点击次数")] public int requiredClicks = 5; // 需要的点击次数
    [Header("几秒内")] public float timeWindow = 2f; // 时间窗口（秒）
    private int clickCount = 0; // 点击计数器
    private Coroutine clickRoutine; // 协程引用
    public UnityEvent clickEvent;
    public bool withQuit;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // 为按钮点击事件添加监听
        button.onClick.AddListener(OnButtonClick);
        canvasGroup.alpha = 0;
    }

    private void OnButtonClick()
    {
        clickCount++;

        if (clickRoutine == null)
        {
            // 开始计时
            clickRoutine = StartCoroutine(ClickTimeWindow());
        }

        // 检查点击次数是否达到要求
        if (clickCount >= requiredClicks)
        {
            // 重置计数器和协程
            clickCount = 0;
            if (clickRoutine != null)
            {
                StopCoroutine(clickRoutine);
                clickRoutine = null;
            }

            // 触发事件
            TriggerEvent();
        }
    }

    private IEnumerator ClickTimeWindow()
    {
        // 等待指定时间
        yield return new WaitForSeconds(timeWindow);

        // 时间到，重置计数器和协程
        clickCount = 0;
        clickRoutine = null;
    }

    private void TriggerEvent()
    {
        // 在这里实现你的事件逻辑
        Debug.Log("触发事件！");
        clickEvent.Invoke();
        if (withQuit)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else
                Application.Quit();
        }
    }
}