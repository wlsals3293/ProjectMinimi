using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class ProjectileBase : MonoBehaviour
{
    [Tooltip("투사체 지속 시간")]
    [SerializeField]
    protected float lifetime = 20f;

    [Tooltip("충돌할 레이어")]
    [SerializeField]
    protected LayerMask collisionMask;

    protected float speed;

    protected bool oneShot = true;

    


    protected Rigidbody rb;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!oneShot || !collisionMask.Contains(other.gameObject.layer))
            return;

        oneShot = false;

        Collide(other);
    }


    public virtual void Set(Vector3 inVelocity)
    {
        rb.velocity = inVelocity;
        speed = inVelocity.magnitude;
        Invoke(nameof(LifetimeEnd), lifetime);
    }

    protected virtual void Collide(Collider other)
    {
        Debug.Log("발사체 충돌");
    }

    protected virtual void LifetimeEnd()
    {
        Destroy(gameObject);
    }
}
