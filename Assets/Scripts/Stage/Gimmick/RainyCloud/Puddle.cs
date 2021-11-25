using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour, IHitable
{
    [Tooltip("������ ���� ���� ����")]
    [SerializeField]
    private float cloudHeight = 10f;


    private RainyCloud cloud = null;


    private Collider cachedCollider = null;
    public Collider CachedCollider { get => cachedCollider; }


    private void Awake()
    {
        cachedCollider = GetComponent<Collider>();
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo = null)
    {
        if (extraDamageInfo.elementType == ElementType.Fire)
        {
            if (cloud == null)
            {
                // �򱸸� ����
                cloud = ResourceManager.Instance.CreatePrefab<RainyCloud>(
                    "RainyCloud", PrefabPath.RainyCloud, null);

                cloud.transform.position = transform.position + Vector3.up * cloudHeight;
                cloud.parentPuddle = this;

                // TODO: �ӽ÷� 2���� Ȱ��ȭ, ���߿� ������ �߻�ü�� ���Ⱑ ���� ��ġ��
                //       �ٴٸ��� Ȱ��ȭ �ϵ��� ������ ����
                Timer.SetTimer(cloud, cloud.Activate, 2f);
            }
            else
            {
                // �ܰ� ����
                cloud.IncreaseStep();
            }

        }

    }
}
