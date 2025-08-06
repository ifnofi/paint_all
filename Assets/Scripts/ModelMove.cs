using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelMove : MonoBehaviour, IPointerClickHandler
{
    public RectTransform clickableUI;
    public bool keyBoard;
    public bool mouse;
    public float moveSpeed = 5f; // 移动速度
    public float rotationSpeed = 5f; // 旋转速度
    private Vector3 targetDirection;
    private Vector3 targetDirectionOther;
    private Animator _animator;

    private bool walk;

    private bool Walk
    {
        get => walk;
        set
        {
            if (value != walk)
            {
                _animator.SetBool("Walk", value);
            }

            walk = value;
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        // 注册点击事件
        EventTrigger trigger = clickableUI.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }


    void Update()
    {
        KeyBoardController();
        MouseController();
    }


    private bool isMoving;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        // 将UI点击位置转换为世界坐标
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            eventData.pointerPressRaycast.gameObject.GetComponent<RectTransform>(),
            eventData.position,
            Camera.main,
            out localPoint);

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));
        // worldPosition.y = transform.position.y; // 保持角色的Y轴不变

        Debug.Log(worldPosition);
        targetDirectionOther = worldPosition;
        targetDirection = worldPosition;
        Walk = true;
    }

    private void MouseController()
    {
        if (!mouse) return;


        if (Walk)
        {
            var endPos = new Vector3(targetDirection.x, targetDirection.y, 0);
            // 计算方向
            Vector3 rotationDirection = (new Vector3(targetDirection.x, 0, targetDirection.y) - transform.position).normalized;
            rotationDirection.y = 0; // 忽略Y轴的差异
            Vector3 moveDirection = (new Vector3(targetDirection.x, targetDirection.y, 0) - transform.position).normalized;
            moveDirection.z = 0;
            // Debug.Log(rotationDirection);
            if (Vector3.Distance(transform.position, endPos) > 0.01f)
            {
                // 计算目标旋转
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);

                // 平滑旋转
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // 平滑移动
                transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
            }
            else
            {
                Walk = false; // 到达目标位置，停止移动
            }
        }
    }

    private void KeyBoardController()
    {
        if (!keyBoard) return;

        // 获取输入
        float horizontal = Input.GetAxis("Horizontal"); // A和D键
        float vertical = Input.GetAxis("Vertical"); // W和S键

        // 计算移动方向
        Vector3 rotationMent = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 movement = new Vector3(horizontal, vertical, 0).normalized;

        Debug.Log(rotationMent);

        if (movement.magnitude >= 0.1f || rotationMent.magnitude >= 0.1f)
        {
            Walk = true;
            // 计算目标方向
            targetDirection = rotationMent;

            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // 平滑旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 平滑移动
            transform.position += movement * moveSpeed * Time.deltaTime;
        }
        else
        {
            Walk = false;
        }
    }
}