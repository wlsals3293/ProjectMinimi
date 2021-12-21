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

        // 박스모양으로 먼저 물리체크 후 콜라이더들을 얻어옴
        Collider[] colliders = cloud.GetBoundingBoxObjects();
        float closestDistance = 10000f;
        Vector3 lightningPoint = Vector3.zero;
        bool empty = true;

        foreach (Collider col in colliders)
        {
            // 전도체가 아니라면 다음으로
            if (!col.CompareTag(Tags.Conductor) || col.GetComponent<IHitable>() == null)
                continue;

            Vector3 closestPoint = col.ClosestPoint(cloud.transform.position);
            Vector3 distanceVector = closestPoint - cloud.transform.position;
            Vector2 distanceVector2D = new Vector2(distanceVector.x, distanceVector.z);

            // 오브젝트와 구름사이 2차원(높이무시) 거리로 범위 안쪽에 있는지 체크
            if (distanceVector2D.sqrMagnitude <= cloud.EffectRadius * cloud.EffectRadius)
            {
                // 그중 가장 가까운 오브젝트를 구해서 저장
                if (distanceVector.sqrMagnitude <= closestDistance)
                {
                    closestDistance = distanceVector.sqrMagnitude;
                    lightningPoint = closestPoint;
                    empty = false;
                }
            }
        }

        // 오브젝트가 하나도 없으므로 중앙에 번개가 침
        if (empty)
        {
            if (Physics.Raycast(cloud.transform.position, Vector3.down, out RaycastHit hit,
                100f, LayerMasks.All, QueryTriggerInteraction.Ignore))
            {
                lightningPoint = hit.point;
            }
            // 부딪히는 것이 아무것도 없으면 중단
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


        // 임시 번개 표시
        cloud.temp_LineRenderer.SetPosition(0, cloud.transform.position);
        cloud.temp_LineRenderer.SetPosition(1, lightningPoint);


        foreach (Collider col in colliders)
        {
            // 10단계 전기 데미지
            col.GetComponent<IHitable>()?.TakeDamage(10, damageInfo);
        }


        yield return new WaitForSeconds(0.3f);

        // 임시 번개 표시
        cloud.temp_LineRenderer.SetPosition(0, cloud.transform.position);
        cloud.temp_LineRenderer.SetPosition(1, cloud.transform.position);


        cloud.ChangeState(CloudType.White);
        cloud.DecreaseStep();
    }
}
