using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimiProjectile : ProjectileBase
{
    [Tooltip("��������")]
    [SerializeField]
    private ElementType elementType = ElementType.None;

    [Tooltip("���߹���")]
    [SerializeField]
    private float explosionRadius = 1f;

    [Tooltip("���ط�")]
    [SerializeField]
    private int damage = 0;


    protected override void Collide(Collider other)
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, explosionRadius,
            LayerMasks.EO, QueryTriggerInteraction.Ignore);

        ExtraDamageInfo extraDamageInfo = new ExtraDamageInfo(Vector3.zero, elementType);

        foreach (Collider hitedObject in overlaps)
        {
            hitedObject.GetComponent<IHitable>()?.TakeDamage(damage, extraDamageInfo);
        }

        Destroy(gameObject);
    }
}
