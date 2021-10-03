using System.Collections;
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
    private CloudType currentType = CloudType.None;

    private RainyCloudState currentState = null;

    private Dictionary<CloudType, RainyCloudState> states = new Dictionary<CloudType, RainyCloudState>();

    private int step = 0;


    private Material cachedMaterial;

    public Material CloudMaterial
    {
        get
        {
            if(cachedMaterial == null)
            {
                return GetComponent<Renderer>().material;
            }
            else
            {
                return cachedMaterial;
            }
        }
    }


    private void Awake()
    {
        AddState(CloudType.White, new RainyCloudState_White());
        AddState(CloudType.Rain, new RainyCloudState_Rain());
        AddState(CloudType.Lightning, new RainyCloudState_Lightning());

        foreach(var state in states)
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


    public void TakeDamage(int amount)
    {
        // 일반 피해는 무시
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        ElementType damageElement = extraDamageInfo.elementType;

        switch (currentType)
        {
            case CloudType.White:
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

                // 테스트로 용 임시
                if (damageElement == ElementType.Fire)
                {
                    // 흰구름으로 변화
                    ChangeState(CloudType.White);
                }
                break;
        }
    }



    /// <summary>
    /// 구름의 단계를 증가시킵니다
    /// </summary>
    public void IncreaseStep()
    {
        if (currentType != CloudType.White)
            return;

        // TODO: 구름 크기 증가
    }

    public void DecreaseStep()
    {

    }

    public void Activate()
    {
        transform.localScale = new Vector3(5f, 1f, 5f);

        ChangeState(CloudType.White);
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
