using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class ProjectileBase : MonoBehaviour
{

    [SerializeField]
    protected float lifetime = 20f;

    protected float speed;

    [SerializeField]
    protected LayerMask collisionMask;


    protected Rigidbody rb;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!Lib.IsInLayerMask(other.gameObject, collisionMask))
            return;

        ProjectileCollision(other);
    }


    public virtual void Set(Vector3 inVelocity)
    {
        rb.velocity = inVelocity;
        speed = inVelocity.magnitude;
        Invoke(nameof(LifetimeEnd), lifetime);
    }

    protected virtual void ProjectileCollision(Collider other)
    {
        Debug.Log("발사체 충돌");
    }

    protected virtual void LifetimeEnd()
    {
        Destroy(gameObject);
    }
}
