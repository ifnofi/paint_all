using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMe : MonoBehaviour
{
    public void Despawn()
    {
        PoolManagerControl.Instance.Despawnfab(transform);
    }
}
