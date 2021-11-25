using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour, IHitable
{
    [Tooltip("웅덩이 기준 구름 높이")]
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
                // 흰구름 생성
                cloud = ResourceManager.Instance.CreatePrefab<RainyCloud>(
                    "RainyCloud", PrefabPath.RainyCloud, null);

                cloud.transform.position = transform.position + Vector3.up * cloudHeight;
                cloud.parentPuddle = this;

                // TODO: 임시로 2초후 활성화, 나중에 일종의 발사체인 연기가 구름 위치에
                //       다다르면 활성화 하도록 변경할 예정
                Timer.SetTimer(cloud, cloud.Activate, 2f);
            }
            else
            {
                // 단계 증가
                cloud.IncreaseStep();
            }

        }

    }
}
