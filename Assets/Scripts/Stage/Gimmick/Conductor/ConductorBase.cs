using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ElectricityEventInfo
{
    public Transform source;
    public int eventNum;
}

public abstract class ConductorBase : MonoBehaviour
{
    /// <summary>
    /// 전도 유지시간
    /// </summary>
    protected const float HOLDING_TIME = 3f;


    /// <summary>
    /// 마지막으로 발생한 이벤트 넘버 (스태틱)
    /// </summary>
    protected static int latestEventNum = 0;

    /// <summary>
    /// 전기 이벤트 정보
    /// </summary>
    public ElectricityEventInfo electricityEventInfo;


    /// <summary>
    /// 활성화(전도) 여부
    /// </summary>
    protected bool isActive = false;

    [Tooltip("전도 범위 (미터단위)")]
    [SerializeField]
    protected float conductionRange = 0.1f;

    /// <summary>
    /// 전도가 시작한 뒤 흐른 시간
    /// </summary>
    protected float elapsedElectricityTime = 0;

    [Tooltip("디버그용. 전도 시간 표시")]
    [SerializeField]
    protected bool debugFlag = false;

    /// <summary>
    /// 활성화 됐을 때 작동하는 코루틴
    /// </summary>
    protected Coroutine onActivateCor;

    protected BoxCollider boxCollider;

    [Tooltip("색깔 바꿀 렌더러")]
    [SerializeField]
    protected Renderer rendererComponent;


    public bool IsActive { get => isActive; }


    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public virtual void Conduct(ConductorBase giver)
    {
        if (electricityEventInfo.eventNum < giver.electricityEventInfo.eventNum)
        {
            electricityEventInfo.eventNum = giver.electricityEventInfo.eventNum;

            Activate();
        }
    }

    protected virtual void Activate()
    {
        elapsedElectricityTime = 0f;
        if (onActivateCor == null)
        {
            onActivateCor = StartCoroutine(OnActivateElectricity());
        }

        if (rendererComponent != null)
            rendererComponent.material.color = Color.yellow;
        isActive = true;
    }

    protected virtual void Deactivate()
    {
        if (onActivateCor != null)
        {
            StopCoroutine(onActivateCor);
            onActivateCor = null;
        }

        if (rendererComponent != null)
            rendererComponent.material.color = Color.white;
        isActive = false;
    }


    protected virtual IEnumerator OnActivateElectricity()
    {
        while (elapsedElectricityTime < HOLDING_TIME)
        {
            elapsedElectricityTime += Time.deltaTime;
            if (debugFlag)
                Debug.LogWarning("Name: " + gameObject.name + "  Electricity Remain: " + (HOLDING_TIME - elapsedElectricityTime));

            DetectOtherColliders();

            yield return null;
        }

        onActivateCor = null;
        Deactivate();
    }

    protected virtual void DetectOtherColliders()
    {
        if (boxCollider == null)
        {
            Debug.LogError($"{gameObject.name}: Conductor는 BoxCollider가 필요합니다.");
            return;
        }

        Collider[] overlapedCols = Physics.OverlapBox
        (
            transform.position,
            boxCollider.size * 0.5f + (Vector3.one * conductionRange),
            transform.rotation,
            LayerMasks.EOP,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < overlapedCols.Length; i++)
        {
            if (overlapedCols[i].transform == transform)
                continue;

            if (overlapedCols[i].CompareTag(Tags.Conductor))
            {
                ConductorBase otherConductor = overlapedCols[i].GetComponent<ConductorBase>();

                if (otherConductor != null)
                    otherConductor.Conduct(this);
            }
            else
            {
                IHitable other = overlapedCols[i].GetComponent<IHitable>();

                if (other != null)
                {
                    ExtraDamageInfo damageInfo = new ExtraDamageInfo(
                        transform.position, ElementType.Electricity);
                    other.TakeDamage(1, damageInfo);
                }

            }
        }
    }
}
