using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritProjectile : ProjectileBase
{

    protected override void ProjectileCollision(Collider other)
    {
        Debug.Log("�߻�ü �浹");
        Destroy(gameObject);
    }
}
