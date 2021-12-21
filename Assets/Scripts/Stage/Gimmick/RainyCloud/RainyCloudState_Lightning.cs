using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainyCloudState_Lightning : RainyCloudState
{
    private Coroutine lightningCoroutine = null;


    public override void SetState(RainyCloud inCloud)
    {
        cloudType = CloudType.Lightning;
        cloud = inCloud;
    }

    public override void Enter()
    {
        cloud.CloudMaterial.color = new Color(0.2f, 0.2f, 0.2f);

        lightningCoroutine = cloud.StartCoroutine(LightningProcess());
    }

    public override void Exit()
    {
        if (lightningCoroutine != null)
            cloud.StopCoroutine(lightningCoroutine);
    }

    private IEnumerator LightningProcess()
    {
        yield return new WaitForSeconds(0.3f);

        // �ڽ�������� ���� ����üũ �� �ݶ��̴����� ����
        Collider[] colliders = cloud.GetBoundingBoxObjects();
        float closestDistance = 10000f;
        Vector3 lightningPoint = Vector3.zero;
        bool empty = true;

        foreach (Collider col in colliders)
        {
            // ����ü�� �ƴ϶�� ��������
            if (!col.CompareTag(Tags.Conductor) || col.GetComponent<IHitable>() == null)
                continue;

            Vector3 closestPoint = col.ClosestPoint(cloud.transform.position);
            Vector3 distanceVector = closestPoint - cloud.transform.position;
            Vector2 distanceVector2D = new Vector2(distanceVector.x, distanceVector.z);

            // ������Ʈ�� �������� 2����(���̹���) �Ÿ��� ���� ���ʿ� �ִ��� üũ
            if (distanceVector2D.sqrMagnitude <= cloud.EffectRadius * cloud.EffectRadius)
            {
                // ���� ���� ����� ������Ʈ�� ���ؼ� ����
                if (distanceVector.sqrMagnitude <= closestDistance)
                {
                    closestDistance = distanceVector.sqrMagnitude;
                    lightningPoint = closestPoint;
                    empty = false;
                }
            }
        }

        // ������Ʈ�� �ϳ��� �����Ƿ� �߾ӿ� ������ ħ
        if (empty)
        {
            if (Physics.Raycast(cloud.transform.position, Vector3.down, out RaycastHit hit,
                100f, LayerMasks.All, QueryTriggerInteraction.Ignore))
            {
                lightningPoint = hit.point;
            }
            // �ε����� ���� �ƹ��͵� ������ �ߴ�
            else
            {
                cloud.ChangeState(CloudType.White);
                cloud.DecreaseStep();
                yield break;
            }
        }

        ExtraDamageInfo damageInfo = new ExtraDamageInfo(lightningPoint, ElementType.Electricity);

        colliders = Physics.OverlapSphere(lightningPoint, cloud.LightningExplosionRadius,
            LayerMasks.EOP, QueryTriggerInteraction.Ignore);


        // �ӽ� ���� ǥ��
        cloud.temp_LineRenderer.SetPosition(0, cloud.transform.position);
        cloud.temp_LineRenderer.SetPosition(1, lightningPoint);


        foreach (Collider col in colliders)
        {
            // 10�ܰ� ���� ������
            col.GetComponent<IHitable>()?.TakeDamage(10, damageInfo);
        }


        yield return new WaitForSeconds(0.3f);

        // �ӽ� ���� ǥ��
        cloud.temp_LineRenderer.SetPosition(0, cloud.transform.position);
        cloud.temp_LineRenderer.SetPosition(1, cloud.transform.position);


        cloud.ChangeState(CloudType.White);
        cloud.DecreaseStep();
    }
}
