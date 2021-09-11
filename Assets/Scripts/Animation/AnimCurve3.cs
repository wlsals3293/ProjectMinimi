using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimCurve3
{
    public AnimationCurve x;
    public AnimationCurve y;
    public AnimationCurve z;

    public AnimCurve3(bool is3D = true)
    {
        if (is3D)
        {
            x = new AnimationCurve();
        }
        y = new AnimationCurve();
        z = new AnimationCurve();
    }
}
