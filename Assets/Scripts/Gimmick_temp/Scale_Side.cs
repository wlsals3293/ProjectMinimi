using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Side : MonoBehaviour
{
    public float baseWeight;
    [HideInInspector] public float totalWeight = 0f;
    private float otherWeight;
    private Coroutine onActivateCort;
 
    // Start is called before the first frame update
    void Start()
    {
        
        onActivateCort = StartCoroutine(OnActivate());
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
        yield return null;

        while (true)
        {
            Vector3 center = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            Collider[] cols = Physics.OverlapBox(center, transform.lossyScale / 2, transform.rotation, LayerMasks.PO, QueryTriggerInteraction.Ignore);
            
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

            totalWeight = baseWeight + otherWeight;

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
