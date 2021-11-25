using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBundle : Activatee
{
    private LineRenderer[] lasers;

    private Coroutine activeCoroutine;


    // Start is called before the first frame update
    private void Start()
    {
        lasers = GetComponentsInChildren<LineRenderer>();

        if (activateOnStart)
            Activate();
    }

    protected override bool Activate()
    {
        if (!base.Activate())
            return false;

        foreach (LineRenderer laser in lasers)
        {
            laser.enabled = true;
        }

        activeCoroutine = StartCoroutine(ActiveLaser());

        return true;
    }

    protected override bool Deactivate()
    {
        if (!base.Deactivate())
            return false;

        StopCoroutine(activeCoroutine);
        activeCoroutine = null;

        foreach (LineRenderer laser in lasers)
        {
            laser.enabled = false;
        }

        return true;
    }

    IEnumerator ActiveLaser()
    {
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

                if (hit.collider != null && hit.collider.gameObject.layer == Layers.Player)
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

            yield return null;
        }
    }
}
