using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : Switchs_Ctrl
{ 
    private CapsuleCollider coll;

    float Invincibility_Interval = 0f;

    bool isInvincibility = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.SwallowableObject) && !isInvincibility)
        {
            isInvincibility = true;
            if (!isActivate) isActivate = true;
            else isActivate = false;
        }
    }

    private void Update()
    {
        if (isInvincibility)
        {
            Invincibility_Interval += Time.deltaTime;

            if (Invincibility_Interval > 0.5f)
            {
                isInvincibility = false;
                Invincibility_Interval = 0f;
            }
        }
    }


    void Start()
    {
        _thisColor = GetComponent<Renderer>();
        _thisColor.material = _color_DontTouch[1];
    }
}
