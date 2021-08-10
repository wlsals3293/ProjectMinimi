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
        AllSwitchOn = false;
    }

    public override void Activate()
    {
        foreach (LineRenderer laser in lasers)
        {
            laser.enabled = !laser.enabled;
        }

    }

    public override void Deactivate()
    {

        foreach (LineRenderer laser in lasers)
        {
            laser.enabled = !laser.enabled;
        }

    }


    IEnumerator ActiveLaser()
    {
        while (true)
        {
            DrawLaser();
            yield return new WaitForSeconds(0.03f);
        }

    }

    void DrawLaser()
    {
        int i = 1;
        foreach (LineRenderer laser in lasers)
        {
            if (laser.enabled)
            {
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
                laser.SetPosition(1, new Vector3(0, 0, distance));

                if (hit.collider != null && hit.collider.CompareTag(Tags.Player))
                {
                    hit.collider.GetComponent<PlayerCharacter>().TakeDamage(1, -hit.collider.transform.forward);
                }
            }
            i++;
        }
    }
}
