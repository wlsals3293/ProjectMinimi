using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour, IHitable
{

    private float cloudHeight = 8f;


    private RainyCloud cloud = null;


    public void TakeDamage(int amount)
    {
        // �Ϲ� ���ش� ����
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if (extraDamageInfo.elementType == ElementType.Fire)
        {
            if (cloud == null)
            {
                // �򱸸� ����
                cloud = ResourceManager.Instance.CreatePrefab<RainyCloud>(
                    "RainyCloud", PrefabPath.RainyCloud, null);

                cloud.transform.position = transform.position + Vector3.up * cloudHeight;

                // TODO: �ӽ÷� 3���� Ȱ��ȭ, ���߿� ������ �߻�ü�� ���Ⱑ ���� ��ġ��
                //       �ٴٸ��� Ȱ��ȭ �ϵ��� ������ ����
                cloud.Invoke("Activate", 3f);
            }
            else
            {
                // �ܰ� ����
                cloud.IncreaseStep();
            }

        }

    }
}
