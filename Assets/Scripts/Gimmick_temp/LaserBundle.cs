using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBundle : Switch_C_OBJ
{

    private LineRenderer[] lasers;
    private Transform[] startPoints;
    private Coroutine active;
    private LayerMask layerMask;
    private bool alreadyActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground", "Object", "Player");
        lasers = GetComponentsInChildren<LineRenderer>();
        startPoints = GetComponentsInChildren<Transform>();
        ConnectingSwitch();
        active = StartCoroutine(ActiveLaser());
    }

    public override void WhenAllSwitchOn()
    {
        if (!alreadyActive)
        {
            active = StartCoroutine(ActiveLaser());
            alreadyActive = true;
        }
    }


    IEnumerator ActiveLaser()
    {
        yield return null;

        while (AllSwitchOn)
        {
            int i = 1;
            foreach (LineRenderer laser in lasers)
            {
                laser.enabled = true;
                RaycastHit hit;
                Physics.Raycast(
                    startPoints[i].position,
                    startPoints[i].forward,
                    out hit,
                    40f,
                    layerMask,
                    QueryTriggerInteraction.Ignore
                    );

                float distance = Vector3.Distance(startPoints[i].position, hit.point);

                laser.SetPosition(0, Vector3.zero);
                laser.SetPosition(1, new Vector3(0,0, distance));

                if (hit.collider != null && hit.collider.CompareTag(Tags.Player))
                {
                    hit.collider.GetComponent<PlayerCharacter>().TakeDamage(1, -hit.collider.transform.forward);
                }

                i++;

            }
            yield return new WaitForSeconds(0.03f);
        }
        if (!AllSwitchOn)
        {
            foreach (LineRenderer laser in lasers)
            {
                laser.enabled = false;
            }
            alreadyActive = false;
            
            StopCoroutine(active);
        }
    }
}
