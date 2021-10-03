using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour, IHitable
{

    private float cloudHeight = 8f;


    private RainyCloud cloud = null;


    public void TakeDamage(int amount)
    {
        // 일반 피해는 무시
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if (extraDamageInfo.elementType == ElementType.Fire)
        {
            if (cloud == null)
            {
                // 흰구름 생성
                cloud = ResourceManager.Instance.CreatePrefab<RainyCloud>(
                    "RainyCloud", PrefabPath.RainyCloud, null);

                cloud.transform.position = transform.position + Vector3.up * cloudHeight;

                // TODO: 임시로 3초후 활성화, 나중에 일종의 발사체인 연기가 구름 위치에
                //       다다르면 활성화 하도록 변경할 예정
                cloud.Invoke("Activate", 3f);
            }
            else
            {
                // 단계 증가
                cloud.IncreaseStep();
            }

        }

    }
}
