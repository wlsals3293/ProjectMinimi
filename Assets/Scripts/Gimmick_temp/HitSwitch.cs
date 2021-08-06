using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : Switchs_Ctrl
{ 
    private CapsuleCollider coll;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.SwallowableObject))
        {
            if (!isActivate) isActivate = true;
            else isActivate = false;
        }
    }

    void Start()
    {
        _thisColor = GetComponent<Renderer>();
        _thisColor.material = _color_DontTouch[1];
    }
}
