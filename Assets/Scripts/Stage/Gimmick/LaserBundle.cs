using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBundle : Activatee
{
    private LineRenderer[] lasers;
    //private Transform[] startPoints;

    private Coroutine activeCoroutine;

    // Start is called before the first frame update
    private void Start()
    {
        lasers = GetComponentsInChildren<LineRenderer>();
        //startPoints = GetComponentsInChildren<Transform>();
        //ConnectingSwitch();

        //AllSwitchOn = false;
        if (ActivateOnStart)
            Activate();
    }

    protected override void Activate()
    {
        base.Activate();

        foreach (LineRenderer laser in lasers)
        {
            laser.enabled = true;
        }

        activeCoroutine = StartCoroutine(ActiveLaser());
    }

    protected override void Deactivate()
    {
        base.Deactivate();

        StopCoroutine(activeCoroutine);
        activeCoroutine = null;

        foreach (LineRenderer laser in lasers)
        {
            laser.enabled = false;
        }
    }

    IEnumerator ActiveLaser()
    {
        var delay = new WaitForSeconds(0.03f);

        while (true)
        {
            foreach (LineRenderer laser in lasers)
            {
                bool result = Physics.Raycast(
                    laser.transform.position,
                    laser.transform.forward,
                    out RaycastHit hit,
                    40f,
                    LayerMasks.PGO,
                    QueryTriggerInteraction.Ignore
                    );

                float distance = result ?
                    Vector3.Distance(laser.transform.position, hit.point)
                    : 40f;

                laser.SetPosition(0, Vector3.zero);
                laser.SetPosition(1, new Vector3(0, 0, distance));

                if (hit.collider != null && hit.transform.gameObject.layer == Layers.Player)
                {
                    Vector3 proj = Vector3.Project(
                        hit.transform.position - laser.transform.position,
                        laser.transform.forward);
                    Vector3 hitPosition = laser.transform.position + proj;

                    ExtraDamageInfo extraDamageInfo
                        = new ExtraDamageInfo(hitPosition);

                    hit.transform.GetComponent<PlayerCharacter>().TakeDamage(1, extraDamageInfo);
                }
            }

            yield return delay;
        }
    }
}
