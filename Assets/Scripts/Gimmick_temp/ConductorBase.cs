using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorBase : MonoBehaviour
{
    public LayerMask overlapLayer;

    public ElementType curElementType;
    public float electricity_Interval = 0;
    public float knockbackForce = 0;

    public ElectricityEventInfo electricityEventInfo;

    [Header("디버그용. 전도 시간 표시")]
    public bool DebugFlag = false;

    protected float curElectricityTime = 0;
    protected Vector3 overlapSize;

    private Renderer rendererComponent;
    private bool isActivate = false;
    private bool isTakeDamaged = false;

    public bool IsActivate
    {
        get { return isActivate; }
        set
        {
            isActivate = value;

            if (isActivate)
            {
                rendererComponent.material.color = Color.yellow;
                curElementType = ElementType.Electricity;
                curElectricityTime = Time.time;
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

    private void Update()
    {
        if (GetComponentInChildren<TextMesh>())
            GetComponentInChildren<TextMesh>().text = electricityEventInfo.EventNum.ToString();
    }

    protected IEnumerator OnActivateElectricity()
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

    private void DetectOtherColliders()
    {
        Collider[] _overlapedCols = Physics.OverlapBox
        (
            transform.position,
            overlapSize / 2,
            transform.rotation,
            overlapLayer
        );

        for (int i = 0; i < _overlapedCols.Length; i++)
        {
            if (_overlapedCols[i].transform == this.transform)
                continue;

            if (_overlapedCols[i].CompareTag("Conductor"))
            {
                ElectricityManager.Instance.TestFlow(this.transform, _overlapedCols[i].transform);
            }
            else if (_overlapedCols[i].gameObject.layer == Layers.Player)
            {
                PlayerCharacter _other = _overlapedCols[i].GetComponent<PlayerCharacter>();

                if (!isTakeDamaged)
                {
                    isTakeDamaged = true;

                    Vector3 forceDir = _other.transform.forward * -1 * knockbackForce;
                    _other.GetComponent<Rigidbody>().AddForce(forceDir, ForceMode.Impulse);
                    _other.TakeDamage(1);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layers.Player)
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
