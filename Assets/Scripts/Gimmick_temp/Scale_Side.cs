using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Side : MonoBehaviour
{
    public float BaseWeight;
    [HideInInspector]
    public float TotalWeight;
    [HideInInspector]
    public bool isEmpty;
    public float otherWeight;

    private Coroutine onActivateCort;
    private LayerMask layerMask;
    public List<Weight> OBJs;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Minimi", "Player", "Object");
    }

    // Update is called once per frame
    void Update()
    {
        float Total_otherWeight = 0;
        if (OBJs.Count > 0)
        {
            foreach (Weight obj in OBJs)
            {
                Total_otherWeight += obj.weight;
            }
        }
        TotalWeight = BaseWeight + Total_otherWeight;
        otherWeight = Total_otherWeight;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (
           other.gameObject.CompareTag("Player") ||
           other.gameObject.CompareTag("Minimi") ||
           other.gameObject.CompareTag("Object")
           )
        {
            onActivateCort = StartCoroutine(OnActivate());
        }
    }

    private IEnumerator OnActivate()
    {
        while (true)
        {
            Vector3 center = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            Collider[] cols = Physics.OverlapBox(center, transform.lossyScale / 2, transform.rotation, layerMask, QueryTriggerInteraction.Ignore);
            

            bool isEmpty = true;
            OBJs.Clear();

            foreach (Collider col in cols)
            {
                if (col.CompareTag("Player") || col.CompareTag("Minimi") || col.CompareTag("Object"))
                {
                    isEmpty = false;
                    GameObject temp = col.gameObject;
                    if(temp.gameObject.GetComponent<Weight>() != null)
                    {
                        OBJs.Add(temp.gameObject.GetComponent<Weight>());
                    }
                    
                }
            }

            if (isEmpty)
            {
                OBJs.Clear();
                StopCoroutine(onActivateCort);
                break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}
