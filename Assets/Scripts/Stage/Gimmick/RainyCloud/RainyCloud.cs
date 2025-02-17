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

    [Tooltip("현재 구름 유형")]
    [SerializeField, ReadOnly]
    private CloudType currentType = CloudType.None;

    [Tooltip("효과가 미치는 범위의 비율. 구름 크기에 비례")]
    [SerializeField, Range(0f, 1.5f)]
    private float effectRangeRate = 1f;

    [Tooltip("구름크기 변경시간")]
    [SerializeField, Range(0f, 5f)]
    private float sizeTransitionTime = 1f;

    [Tooltip("구름의 단계별 사이즈 (X, Z 축 통합)")]
    [SerializeField]
    private float[] cloudSizeList;

    

    private RainyCloudState currentState = null;

    private Dictionary<CloudType, RainyCloudState> states = new Dictionary<CloudType, RainyCloudState>();

    /// <summary>
    /// 현재 구름의 단계 (-1단계는 활성화되지 않음)
    /// </summary>
    private int step = -1;

    /// <summary>
    /// 부모 웅덩이
    /// </summary>
    [HideInInspector]
    public Puddle parentPuddle = null;



    [Header("Rain")]

    [Tooltip("표시용 임시 비 모델. 나중에 이펙트 같은걸로 교체")]
    [SerializeField]
    private GameObject temp_rain;


    [Header("Lightning")]

    [Tooltip("번개 명중 범위")]
    [SerializeField]
    private float lightningExplosionRadius = 3f;


    [Tooltip("임시 번개 표시용 라인 렌더러")]
    [HideInInspector]
    public LineRenderer temp_LineRenderer;



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
        temp_LineRenderer = GetComponent<LineRenderer>();

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

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo = null)
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
                    // 비구름으로 변화
                    ChangeState(CloudType.Rain);
                }
                else if (damageElement == ElementType.Electricity)
                {
                    // 번개구름으로 변화
                    ChangeState(CloudType.Lightning);
                }
                break;

            case CloudType.Rain:
                if (damageElement == ElementType.Fire)
                {
                    // 흰구름으로 변화
                    ChangeState(CloudType.White);
                }
                else if (damageElement == ElementType.Electricity)
                {
                    // 번개구름으로 변화
                    ChangeState(CloudType.Lightning);
                }
                break;

            case CloudType.Lightning:
                // 번개구름일때는 피해무시
                break;
        }
    }


    /// <summary>
    /// 구름의 단계를 올립니다
    /// </summary>
    public void IncreaseStep()
    {
        if (currentType != CloudType.White)
            return;

        // 구름의 단계가 최대면 리턴
        if (step + 1 >= cloudSizeList.Length)
            return;

        step++;

        // 효과범위 변경
        effectRadius = cloudSizeList[step] * effectRangeRate * 0.5f;

        // 크기 변경
        Vector3 cloudSize = new Vector3(cloudSizeList[step], 1f, cloudSizeList[step]);
        transform.DOScale(cloudSize, sizeTransitionTime).SetEase(Ease.OutElastic);
    }

    /// <summary>
    /// 구름의 단계를 내립니다
    /// </summary>
    public void DecreaseStep()
    {
        // 구름의 단계가 최소면 리턴
        if (step <= 0)
            return;

        step--;

        // 효과범위 변경
        effectRadius = cloudSizeList[step] * effectRangeRate * 0.5f;

        // 크기 변경
        Vector3 cloudSize = new Vector3(cloudSizeList[step], 1f, cloudSizeList[step]);
        transform.DOScale(cloudSize, sizeTransitionTime).SetEase(Ease.OutElastic);
    }

    /// <summary>
    /// 비 내리는 이펙트 활성화. 일단 임시
    /// </summary>
    /// <param name="active"></param>
    public void SetRainActive(bool active)
    {
        temp_rain.SetActive(active);
    }

    /// <summary>
    /// 첫 활성화시 실행됨
    /// </summary>
    public void Activate()
    {
        ChangeState(CloudType.White);
        IncreaseStep();
    }

    public Collider[] GetBoundingBoxObjects()
    {
        // 웅덩이 콜라이더 잠시 비활성화
        parentPuddle.CachedCollider.enabled = false;


        Vector3 boxHalfExt = new Vector3(effectRadius, 10f, effectRadius);
        Vector3 boxCenter = transform.position + Vector3.down * (boxHalfExt.y + 1f);

        Collider[] results =
            Physics.OverlapBox(boxCenter, boxHalfExt, transform.rotation,
            LayerMasks.Object, QueryTriggerInteraction.Ignore);


        // 웅덩이 콜라이더 다시 활성화
        parentPuddle.CachedCollider.enabled = true;

        return results;
    }

    public void ChangeState(CloudType nextState)
    {
        // 같은 상태면 리턴
        if (currentType == nextState)
            return;

        // Exit
        currentState?.Exit();


        // 현재 상태 변경
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
