using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSwitch : SwitchBase
{
    public enum SwitchType { Maintain, OnOff }
    public SwitchType _type = SwitchType.OnOff;

    private Coroutine onActivateCort;
    private BoxCollider boxTrigger;

    private LayerMask layerMask;

    private void Awake()
    {
        boxTrigger = GetComponent<BoxCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _thisColor = GetComponent<Renderer>();

        layerMask = LayerMask.GetMask("Minimi", "Player", "Object");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (
           other.gameObject.CompareTag("Player") ||
           other.gameObject.CompareTag("Minimi") ||
           other.gameObject.CompareTag("Object")
           )
        {
            IsActivate = true;

            onActivateCort = StartCoroutine(OnActivate());
        }
    }

    // 임시. 나중에 개편 예정
    private IEnumerator OnActivate()
    {
        while (true)
        {
            Collider[] cols = Physics.OverlapBox(transform.position + boxTrigger.center, boxTrigger.size, transform.rotation, layerMask, QueryTriggerInteraction.Ignore);

            bool isEmpty = true;

            foreach (Collider col in cols)
            {
                if (col.CompareTag("Player") || col.CompareTag("Minimi") || col.CompareTag("Object"))
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
