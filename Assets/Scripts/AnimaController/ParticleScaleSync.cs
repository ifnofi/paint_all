using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleScaleSync : MonoBehaviour
{
    private ParticleSystem ps;
    private float baseLifetime;
    private float baseSpeed;
    private float baseSize;
    private Vector3 baseScale;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        baseLifetime = main.startLifetime.constant;
        baseSpeed = main.startSpeed.constant;
        baseSize = main.startSize.constant;
        baseScale = transform.localScale;
    }

    void Update()
    {
        float scaleFactor = transform.localScale.x / baseScale.x; // 只用x轴比例作为缩放因子
        var main = ps.main;
        main.startLifetime = baseLifetime * scaleFactor;
        main.startSpeed = baseSpeed * scaleFactor;
        main.startSize = baseSize * scaleFactor;
    }
}
