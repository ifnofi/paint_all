using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            PoolManagerControl.Instance.GetRefab("预制体");
            yield return new WaitForSeconds(0.21f);
             PoolManagerControl.Instance.GetRefab("预制体1");
            yield return new WaitForSeconds(0.81f);
        }
    }
}