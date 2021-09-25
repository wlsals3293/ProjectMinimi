using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritProjectile : ProjectileBase
{

    protected override void ProjectileCollision()
    {
        Debug.Log("발사체 충돌");
        Destroy(gameObject);
    }
}
