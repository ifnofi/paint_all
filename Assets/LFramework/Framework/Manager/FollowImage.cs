using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowImage : MonoBehaviour
{
    public RawImage target;
    public RawImage self;

    private void FixedUpdate()
    {
        self.texture = target.texture;
    }
}