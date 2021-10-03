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
        // �Ϲ� ���ش� ����
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        ElementType damageElement = extraDamageInfo.elementType;

        switch (currentType)
        {
            case CloudType.White:
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

                // �׽�Ʈ�� �� �ӽ�
                if (damageElement == ElementType.Fire)
                {
                    // �򱸸����� ��ȭ
                    ChangeState(CloudType.White);
                }
                break;
        }
    }



    /// <summary>
    /// ������ �ܰ踦 ������ŵ�ϴ�
    /// </summary>
    public void IncreaseStep()
    {
        if (currentType != CloudType.White)
            return;

        // TODO: ���� ũ�� ����
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
