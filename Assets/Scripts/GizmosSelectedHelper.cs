using System;
using UnityEngine;

public class GizmosSelectedHelper : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        // 遍历父节点 得到有 BirdMove 组件的节点
        var birdMove = transform.GetComponentInParent<BirdMove>();
        if (birdMove != null)
        {
            birdMove.OnDrawGizmosSelected();
        }
    }
}
