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
    
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Minimi", "Player", "Object");
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
            

            otherWeight = 0;

            foreach (Collider col in cols)
            {
                if (col.CompareTag("Player") || col.CompareTag("Minimi") || col.CompareTag("Object"))
                {
                    isEmpty = false;
                    otherWeight += col.gameObject.GetComponent<Rigidbody>().mass;
                }
            }

            
            
            TotalWeight = BaseWeight + otherWeight;
            

            if (isEmpty)
            {
                otherWeight = 0;
                StopCoroutine(onActivateCort);
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
