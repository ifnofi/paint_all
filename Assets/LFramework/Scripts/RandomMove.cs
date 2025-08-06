using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class RandomMove : MonoBehaviour
{
    public float delay;

    public float Speed = 5;

    public AnimationCurve curve_Speed_Dis; //曲线 速度与距离

    public SpeedType speedType;

    public float Dis = 20; //距离

    [SerializeField] Vector3 vector3;

    public bool X;
    public bool Y;
    public bool Z;

    private void Awake()
    {
        vector3 = transform.localPosition;
    }

    private void Start()
    {
        vector3 = transform.localPosition;
    }

    private void OnEnable()
    {
        transform.DOLocalMove(vector3, delay)
            .OnComplete(Move);
    }


    void Move()
    {
        float x = vector3.x;
        float y = vector3.y;
        float z = vector3.z;
        if (X)
        {
            x = Random.Range(vector3.x + Dis, vector3.x - Dis);
        }

        if (Y)
        {
            y = Random.Range(vector3.y + Dis, vector3.y - Dis);
        }

        if (Z)
        {
            z = Random.Range(vector3.z + Dis, vector3.z - Dis);
        }

        DoMove(x, y, z);
    }

    void DoMove(float x, float y, float z)
    {
        Vector3 vector = new Vector3(x, y, z);
        if (speedType == SpeedType.速度)
        {
            gameObject.transform.DOLocalMove(vector, Speed, false).SetSpeedBased().SetEase(curve_Speed_Dis).OnComplete(Move);
        }
        else
        {
            gameObject.transform.DOLocalMove(vector, Speed, false).SetEase(curve_Speed_Dis).OnComplete(Move);
        }
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }

    public enum SpeedType
    {
        速度,
        时间
    }
}