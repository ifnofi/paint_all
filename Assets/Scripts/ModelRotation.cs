using UnityEngine;

public class ModelRotation : MonoBehaviour
{
    public float rotationSpeed = 5f; // 旋转速度
    public Transform target;
    private Vector3 targetDirection;


    public void SetEndPos(Vector3 endPos)
    {
        targetDirection = endPos;
    }


    void Update()
    {
        Vector3 rotationDirection = (new Vector3(targetDirection.x, targetDirection.y, 0) - target.position).normalized;
        // rotationDirection.y = 0; // 忽略Y轴的差异
        rotationDirection.z = 0; // 忽略Y轴的差异
        // 计算目标旋转
        Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);

        // 平滑旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}