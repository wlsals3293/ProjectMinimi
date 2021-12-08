using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectMinimi;

public class PressSwitch : Activator
{
    [SerializeField]
    private LayerMask pressMask;

    [SerializeField]
    private BoxCollider boxCollider;

    [SerializeField]
    private Renderer switchRenderer;

    [SerializeField]
    private Material[] colorTemp;

    private Coroutine onActivateCort;


    private void OnTriggerEnter(Collider other)
    {
        if (pressMask.Contains(other.gameObject.layer) && onActivateCort == null)
        {
            Activate();
            onActivateCort = StartCoroutine(OnActivate());
        }
    }


    private IEnumerator OnActivate()
    {
        if (switchRenderer != null && colorTemp[1] != null)
            switchRenderer.material = colorTemp[1];

        yield return null;

        Vector3 origin = transform.position + transform.TransformDirection(boxCollider.center);
        Vector3 ext = boxCollider.size / 2;
        var delay = new WaitForSeconds(0.3f);

        boxCollider.enabled = false;

        while (true)
        {
            if (!Physics.CheckBox(origin, ext, transform.rotation,
                pressMask, QueryTriggerInteraction.Ignore))
            {
                Deactivate();
                StopCoroutine(onActivateCort);
                onActivateCort = null;
                break;
            }

            yield return delay;
        }

        if (switchRenderer != null && colorTemp[0] != null)
            switchRenderer.material = colorTemp[0];
        boxCollider.enabled = true;
    }
}
