using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : SwitchBase
{ 
    private CapsuleCollider coll;

    float invincibility_Interval = 0f;

    bool isInvincibility = false;

    private void OnTriggerEnter(Collider other)
    {
            isInvincibility = true;
            if (!IsActivate) IsActivate = true;
            else IsActivate = false;
    }

    private void Update()
    {
        if (isInvincibility)
        {
            invincibility_Interval += Time.deltaTime;

            if (invincibility_Interval > 0.5f)
            {
                isInvincibility = false;
                invincibility_Interval = 0f;
            }
        }
    }


    void Start()
    {
        _thisColor = GetComponent<Renderer>();
        _thisColor.material = _color_DontTouch[1];
    }
}
