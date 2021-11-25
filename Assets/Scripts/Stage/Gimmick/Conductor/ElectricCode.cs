using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricCode : ConductorBase
{
    [Tooltip("전기를 전달할 회로")]
    [ReadOnly]
    public ElectricCircuit circuit;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (circuit != null)
        {
            bool contain = false;
            List<ElectricCode> codes = circuit.Codes;

            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i] == this)
                {
                    contain = true;
                    break;
                }
            }

            if (!contain)
            {
                circuit = null;
            }
        }
    }
#endif


    protected override void Activate()
    {
        base.Activate();

        circuit.Conduct(this);
    }

    protected override void DetectOtherColliders()
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
            LayerMasks.Object,
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
        }
    }
}
