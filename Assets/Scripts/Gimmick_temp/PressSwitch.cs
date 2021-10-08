using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSwitch : SwitchBase
{
    public enum SwitchType { Maintain, OnOff }
    public SwitchType _type = SwitchType.OnOff;

    private Coroutine onActivateCort;

    public Transform box;

    // Start is called before the first frame update
    void Start()
    {
        _thisColor = GetComponent<Renderer>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Obj)
        {
            IsActivate = true;

            onActivateCort = StartCoroutine(OnActivate());
        }
    }

    // 임시. 나중에 개편 예정
    private IEnumerator OnActivate()
    {
        yield return null;
        while (true)
        {
            Collider[] cols = Physics.OverlapBox(box.position + (Vector3.up * 0.2f), box.lossyScale / 2, transform.rotation, LayerMasks.PO, QueryTriggerInteraction.Ignore);

            bool isEmpty = true;

            foreach (Collider col in cols)
            {
                if (col.gameObject.layer == Layers.Obj)
                {
                    isEmpty = false;
                    break;
                }
            }

            if (isEmpty)
            {
                IsActivate = false;
                StopCoroutine(onActivateCort);
                break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}
