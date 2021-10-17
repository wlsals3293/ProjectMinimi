using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronObjectBase : MonoBehaviour
{
    public Renderer rendererComponent;
    public Vector3 overlapCenter;
    public Vector3 overlapSize;
    public LayerMask overlapLayer;

    public ElementType curElementType;
    public float electricity_Interval = 0;

    public ElectricityEventInfo electricityEventInfo;

    [SerializeField]
    bool isActivate = false;
    bool isTakeDamaged = false;

    public bool DebugFlag = false;

    protected float curElectricityTime = 0;

    public bool IsActivate
    {
        get { return isActivate; }
        set
        {
            isActivate = value;

            if (isActivate)
            {
                curElectricityTime = Time.time;
                rendererComponent.material.color = Color.yellow;
                curElementType = ElementType.Electricity;
            }
            else
            {
                rendererComponent.material.color = Color.white;
                curElementType = ElementType.None;
                isTakeDamaged = false;
            }
        }
    }

    protected virtual void Awake()
    {
        rendererComponent = GetComponentInChildren<Renderer>();
        curElementType = ElementType.None;
    }

    protected IEnumerator OnActivateElectricity()
    {
        IsActivate = true;

        while (true)
        {
            if(DebugFlag)
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

    void DetectOtherColliders()
    {
        Collider[] _overlapedCols = Physics.OverlapBox
        (
            transform.position,
            overlapSize / 2,
            Quaternion.identity,
            overlapLayer
        );

        for (int i = 0; i < _overlapedCols.Length; i++)
        {
            if (_overlapedCols[i].transform == this.transform)
                continue;

            if (_overlapedCols[i].GetComponent<IronObjectBase>())
            {
                ElectricityManager.Instance.TestFlow(this.transform, _overlapedCols[i].transform);
            }
            else if (_overlapedCols[i].GetComponent<PlayerCharacter>())
            {
                PlayerCharacter _other = _overlapedCols[i].GetComponent<PlayerCharacter>();

                if (!isTakeDamaged)
                {
                    isTakeDamaged = true;
                    _other.TakeDamage(1);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            isTakeDamaged = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, overlapSize);
    }

}
