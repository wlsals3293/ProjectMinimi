using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class ProjectileBase : MonoBehaviour
{
    [Tooltip("����ü ���� �ð�")]
    [SerializeField]
    protected float lifetime = 20f;

    [Tooltip("�浹�� ���̾�")]
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
        Debug.Log("�߻�ü �浹");
    }

    protected virtual void LifetimeEnd()
    {
        Destroy(gameObject);
    }
}
