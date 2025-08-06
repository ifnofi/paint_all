using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GesturesInputReturn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("全屏检测")] public bool isQuanPing = false;

    [Header("手指灵敏度")]
    //public float Sensitivity = 0.05f;
    // 手指移动的幅度  大于这个幅度才会触发
    private float fingerActionSensitivity = 20f; //手指动作的敏感度，这里设定为 二十分之一的屏幕宽度.

    //
    private float fingerBeginX;
    private float fingerBeginY;
    private float fingerCurrentX;
    private float fingerCurrentY;
    private float fingerSegmentX;

    private float fingerSegmentY;

    //
    private int fingerTouchState;

    //
    private int FINGER_STATE_NULL = 0;
    private int FINGER_STATE_TOUCH = 1;
    private int FINGER_STATE_ADD = 2;

    /// <summary>
    /// 向上滑动
    /// </summary>
    public UnityEvent gestureToUp;

    /// <summary>
    /// 向下滑动
    /// </summary>
    public UnityEvent gestureToDown;

    /// <summary>
    /// 向右滑动
    /// </summary>
    public UnityEvent gestureToRight;

    /// <summary>
    /// 向左滑动
    /// </summary>
    public UnityEvent gestureToLeft;

    // Use this for initialization
    void Start()
    {
        // fingerActionSensitivity = Screen.width * Sensitivity;

        fingerBeginX = 0;
        fingerBeginY = 0;
        fingerCurrentX = 0;
        fingerCurrentY = 0;
        fingerSegmentX = 0;
        fingerSegmentY = 0;

        fingerTouchState = FINGER_STATE_NULL;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            return;
        }

        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            fingerCurrentX = Input.mousePosition.x;
            fingerCurrentY = Input.mousePosition.y;
            fingerSegmentX = fingerCurrentX - fingerBeginX;
            fingerSegmentY = fingerCurrentY - fingerBeginY;
        }


        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            float fingerDistance = fingerSegmentX * fingerSegmentX + fingerSegmentY * fingerSegmentY;

            if (fingerDistance > (fingerActionSensitivity))
            {
                toAddFingerAction();
            }
        }

        if (!isQuanPing) return;
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }


    public UnityEvent triggerAction;
    public bool isOpen;

    private void toAddFingerAction()
    {
        triggerAction.Invoke();
        fingerTouchState = FINGER_STATE_ADD;

        if (Mathf.Abs(fingerSegmentX) > Mathf.Abs(fingerSegmentY))
        {
            fingerSegmentY = 0;
        }
        else
        {
            fingerSegmentX = 0;
        }

        if (fingerSegmentX == 0)
        {
            if (fingerSegmentY > 0)
            {
                gestureToUp.Invoke();
            }
            else
            {
                gestureToDown.Invoke();
            }
        }
        else if (fingerSegmentY == 0)
        {
            if (fingerSegmentX > 0)
            {
                gestureToRight.Invoke();
            }
            else
            {
                gestureToLeft.Invoke();
            }
        }
    }

    public void StartDrag()
    {
        //if (fingerTouchState == FINGER_STATE_NULL)
        //{
        //    fingerTouchState = FINGER_STATE_TOUCH;

        //    fingerBeginX = Input.mousePosition.x;
        //    fingerBeginY = Input.mousePosition.y;
        //}

        if (fingerTouchState == FINGER_STATE_NULL)
        {
            fingerTouchState = FINGER_STATE_TOUCH;

            fingerBeginX = Input.mousePosition.x;
            fingerBeginY = Input.mousePosition.y;
        }
    }

    public void EndDrag()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            fingerTouchState = FINGER_STATE_NULL;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isQuanPing) StartDrag();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (!isQuanPing) EndDrag();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isQuanPing) EndDrag();
    }
}