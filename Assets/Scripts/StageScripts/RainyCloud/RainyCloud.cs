using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;


public enum CloudType
{
    None,
    White,
    Rain,
    Lightning
}

public class RainyCloud : MonoBehaviour, IHitable
{
    [Header("Cloud")]

    [Tooltip("���� ���� ����")]
    [SerializeField, ReadOnly]
    private CloudType currentType = CloudType.None;

    [Tooltip("ȿ���� ��ġ�� ������ ����. ���� ũ�⿡ ���")]
    [SerializeField, Range(0f, 1.5f)]
    private float effectRangeRate = 1f;

    [Tooltip("����ũ�� ����ð�")]
    [SerializeField, Range(0f, 5f)]
    private float sizeTransitionTime = 1f;

    [Tooltip("������ �ܰ躰 ������ (X, Z �� ����)")]
    [SerializeField]
    private float[] cloudSizeList;

    

    

    private RainyCloudState currentState = null;

    private Dictionary<CloudType, RainyCloudState> states = new Dictionary<CloudType, RainyCloudState>();

    /// <summary>
    /// ���� ������ �ܰ� (-1�ܰ�� Ȱ��ȭ���� ����)
    /// </summary>
    private int step = -1;

    /// <summary>
    /// �θ� ������
    /// </summary>
    private Puddle parentPuddle = null;



    [Header("Rain")]

    [Tooltip("ǥ�ÿ� �ӽ� �� ��. ���߿� ����Ʈ �����ɷ� ��ü")]
    [SerializeField]
    private GameObject temp_rain;


    [Header("Lightning")]

    [Tooltip("���� ���� ����")]
    [SerializeField]
    private float lightningExplosionRadius = 3f;



    public Puddle ParentPuddle { get => parentPuddle; set => parentPuddle = value; }

    public float CloudSize { get => cloudSizeList[step]; }

    private float effectRadius;
    public float EffectRadius { get => effectRadius; }

    public float LightningExplosionRadius { get => lightningExplosionRadius; }


    private Collider cachedCollider;
    public Collider CloudCollider { get => cachedCollider; }

    private Material cachedMaterial;
    public Material CloudMaterial
    {
        get
        {
            if (cachedMaterial == null)
            {
                cachedMaterial = GetComponent<Renderer>().material;
            }
            return cachedMaterial;
        }
    }


    private void Awake()
    {
        cachedCollider = GetComponent<Collider>();

        AddState(CloudType.White, new RainyCloudState_White());
        AddState(CloudType.Rain, new RainyCloudState_Rain());
        AddState(CloudType.Lightning, new RainyCloudState_Lightning());

        foreach (var state in states)
        {
            state.Value.SetState(this);
        }

        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (currentState != null && currentState.useUpdate)
            currentState.Update();
    }

    private void OnValidate()
    {
        if (step >= 0 && step < cloudSizeList.Length)
            effectRadius = cloudSizeList[step] * effectRangeRate * 0.5f;
    }


    public void TakeDamage(int amount)
    {
        // �Ϲ� ���ش� ����
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        ElementType damageElement = extraDamageInfo.elementType;

        switch (currentType)
        {
            case CloudType.White:
                if (damageElement == ElementType.Fire)
                {
                    DecreaseStep();
                }
                if (damageElement == ElementType.Water)
                {
                    // �񱸸����� ��ȭ
                    ChangeState(CloudType.Rain);
                }
                else if (damageElement == ElementType.Electricity)
                {
                    // ������������ ��ȭ
                    ChangeState(CloudType.Lightning);
                }
                break;

            case CloudType.Rain:
                if (damageElement == ElementType.Fire)
                {
                    // �򱸸����� ��ȭ
                    ChangeState(CloudType.White);
                }
                else if (damageElement == ElementType.Electricity)
                {
                    // ������������ ��ȭ
                    ChangeState(CloudType.Lightning);
                }
                break;

            case CloudType.Lightning:
                // ���������϶��� ���ع���
                break;
        }
    }


    /// <summary>
    /// ������ �ܰ踦 �ø��ϴ�
    /// </summary>
    public void IncreaseStep()
    {
        if (currentType != CloudType.White)
            return;

        // ������ �ܰ谡 �ִ�� ����
        if (step + 1 >= cloudSizeList.Length)
            return;

        step++;

        // ȿ������ ����
        effectRadius = cloudSizeList[step] * effectRangeRate * 0.5f;

        // ũ�� ����
        Vector3 cloudSize = new Vector3(cloudSizeList[step], 1f, cloudSizeList[step]);
        transform.DOScale(cloudSize, sizeTransitionTime).SetEase(Ease.OutElastic);
    }

    /// <summary>
    /// ������ �ܰ踦 �����ϴ�
    /// </summary>
    public void DecreaseStep()
    {
        // ������ �ܰ谡 �ּҸ� ����
        if (step <= 0)
            return;

        step--;

        // ȿ������ ����
        effectRadius = cloudSizeList[step] * effectRangeRate * 0.5f;

        // ũ�� ����
        Vector3 cloudSize = new Vector3(cloudSizeList[step], 1f, cloudSizeList[step]);
        transform.DOScale(cloudSize, sizeTransitionTime).SetEase(Ease.OutElastic);
    }

    public void SetRainActive(bool active)
    {
        temp_rain.SetActive(active);
    }

    /// <summary>
    /// ù Ȱ��ȭ�� �����
    /// </summary>
    public void Activate()
    {
        ChangeState(CloudType.White);
        IncreaseStep();
    }

    public Collider[] GetBoundingBoxObjects()
    {
        // ������ �ݶ��̴� ��� ��Ȱ��ȭ
        parentPuddle.CachedCollider.enabled = false;


        Vector3 boxHalfExt = new Vector3(effectRadius, 10f, effectRadius);
        Vector3 boxCenter = transform.position + Vector3.down * (boxHalfExt.y + 1f);

        Collider[] results =
            Physics.OverlapBox(boxCenter, boxHalfExt, transform.rotation, LayerMasks.Object);


        // ������ �ݶ��̴� �ٽ� Ȱ��ȭ
        parentPuddle.CachedCollider.enabled = true;

        return results;
    }

    public void ChangeState(CloudType nextState)
    {
        // ���� ���¸� ����
        if (currentType == nextState)
            return;

        // Exit
        currentState?.Exit();


        // ���� ���� ����
        currentType = nextState;

        if (states.ContainsKey(nextState))
            currentState = states[nextState];
        else
            currentState = null;


        // Enter
        currentState?.Enter();
    }

    private void AddState(CloudType type, RainyCloudState state)
    {
        if (states.ContainsKey(type))
            states[type] = state;
        else
            states.Add(type, state);
    }


}
