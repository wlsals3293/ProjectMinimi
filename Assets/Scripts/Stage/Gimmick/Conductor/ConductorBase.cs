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
    /// ���� �����ð�
    /// </summary>
    protected const float HOLDING_TIME = 3f;


    /// <summary>
    /// ���������� �߻��� �̺�Ʈ �ѹ� (����ƽ)
    /// </summary>
    protected static int latestEventNum = 0;

    /// <summary>
    /// ���� �̺�Ʈ ����
    /// </summary>
    public ElectricityEventInfo electricityEventInfo;


    /// <summary>
    /// Ȱ��ȭ(����) ����
    /// </summary>
    protected bool isActive = false;

    [Tooltip("���� ���� (���ʹ���)")]
    [SerializeField]
    protected float conductionRange = 0.1f;

    /// <summary>
    /// ������ ������ �� �帥 �ð�
    /// </summary>
    protected float elapsedElectricityTime = 0;

    [Tooltip("����׿�. ���� �ð� ǥ��")]
    [SerializeField]
    protected bool debugFlag = false;

    /// <summary>
    /// Ȱ��ȭ ���� �� �۵��ϴ� �ڷ�ƾ
    /// </summary>
    protected Coroutine onActivateCor;

    protected BoxCollider boxCollider;

    [Tooltip("���� �ٲ� ������")]
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
            Debug.LogError($"{gameObject.name}: Conductor�� BoxCollider�� �ʿ��մϴ�.");
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
