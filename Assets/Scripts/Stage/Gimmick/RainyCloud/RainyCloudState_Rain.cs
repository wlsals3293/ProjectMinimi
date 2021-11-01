using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainyCloudState_Rain : RainyCloudState
{
    private Coroutine rainCoroutine = null;


    public override void SetState(RainyCloud inCloud)
    {
        cloudType = CloudType.Rain;
        cloud = inCloud;
    }

    public override void Enter()
    {
        cloud.CloudMaterial.color = new Color(0.7f, 0.7f, 0.7f);
        cloud.SetRainActive(true);

        rainCoroutine = cloud.StartCoroutine(RainProcess());
    }

    public override void Exit()
    {
        cloud.SetRainActive(false);

        if (rainCoroutine != null)
            cloud.StopCoroutine(rainCoroutine);
    }

    private IEnumerator RainProcess()
    {
        var wait = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return wait;

            // �ڽ�������� ���� ����üũ �� �ݶ��̴����� ����
            Collider[] colliders = cloud.GetBoundingBoxObjects();


            // ���� ������Ʈ��� ������ 2����(���̹���) �Ÿ��� ���� ���ʿ� �ִ��� üũ
            foreach (var col in colliders)
            {
                Vector3 distanceVector = col.transform.position - cloud.transform.position;
                distanceVector.y = 0f;

                if (distanceVector.sqrMagnitude <= cloud.EffectRadius * cloud.EffectRadius)
                {
                    ExtraDamageInfo damageInfo = new ExtraDamageInfo(ElementType.Water);

                    // 5�ܰ� �� ������
                    col.GetComponent<IHitable>()?.TakeDamage(5, damageInfo);
                }
            }
        }
    }
}
