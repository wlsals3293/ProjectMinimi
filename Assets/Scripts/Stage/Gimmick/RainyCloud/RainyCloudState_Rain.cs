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

            // 박스모양으로 먼저 물리체크 후 콜라이더들을 얻어옴
            Collider[] colliders = cloud.GetBoundingBoxObjects();


            // 얻어온 오브젝트들과 구름의 2차원(높이무시) 거리로 범위 안쪽에 있는지 체크
            foreach (var col in colliders)
            {
                Vector3 distanceVector = col.transform.position - cloud.transform.position;
                distanceVector.y = 0f;

                if (distanceVector.sqrMagnitude <= cloud.EffectRadius * cloud.EffectRadius)
                {
                    ExtraDamageInfo damageInfo = new ExtraDamageInfo(ElementType.Water);

                    // 5단계 물 데미지
                    col.GetComponent<IHitable>()?.TakeDamage(5, damageInfo);
                }
            }
        }
    }
}
