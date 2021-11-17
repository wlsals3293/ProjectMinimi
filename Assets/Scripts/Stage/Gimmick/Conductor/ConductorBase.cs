using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorBase : MonoBehaviour
{
    [HideInInspector] public LayerMask overlapLayer;
    public float electricity_Interval = 0;

    public ElectricityEventInfo electricityEventInfo;

    [Header("디버그용. 전도 시간 표시")]
    public bool DebugFlag = false;

    protected float curElectricityTime = 0;
    protected Vector3 overlapSize;
    protected Renderer rendererComponent;

    public bool isActivate = false;

    public bool IsActivate
    {
        get { return isActivate; }
        set
        {
            isActivate = value;

            if (isActivate)
            {
                rendererComponent.material.color = Color.yellow;
                curElectricityTime = Time.time;
            }
            else
            {
                rendererComponent.material.color = Color.white;
            }
        }
    }

    protected virtual void Awake()
    {
        rendererComponent = GetComponentInChildren<Renderer>();
    }

    protected virtual IEnumerator OnActivateElectricity()
    {
        IsActivate = true;

        while (true)
        {
            if (DebugFlag)
                Debug.LogWarning("Name: " + this.gameObject.name + "  Electricity Remain: " + (Time.time - curElectricityTime));

            if (Time.time - curElectricityTime >= electricity_Interval)
            {
                IsActivate = false;
                break;
            }

            DetectOtherColliders();

            yield return null;
        }
    }

    protected virtual void DetectOtherColliders()
    {
        Collider[] _overlapedCols = Physics.OverlapBox
        (
            transform.position,
            overlapSize / 2,
            transform.rotation,
            overlapLayer,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < _overlapedCols.Length; i++)
        {
            if (_overlapedCols[i].transform == transform)
                continue;

            if (_overlapedCols[i].CompareTag(Tags.Conductor))
            {
                ConductorBase otherConductor = _overlapedCols[i].GetComponent<ConductorBase>();
                if(otherConductor != null)
                    ElectricityManager.Instance.ElectricityProcess(this, otherConductor);
            }
            else if (_overlapedCols[i].gameObject.layer == Layers.Player)
            {
                PlayerCharacter _other = _overlapedCols[i].GetComponent<PlayerCharacter>();

                ExtraDamageInfo damageInfo = new ExtraDamageInfo(transform.position);
                _other.TakeDamage(1, damageInfo);
            }
        }
    }
}
