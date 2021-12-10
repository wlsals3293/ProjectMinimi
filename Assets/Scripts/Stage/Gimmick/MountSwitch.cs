using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectMinimi;

public class MountSwitch : Activator
{
    [SerializeField]
    private Renderer emissionRenderer;



    private void Awake()
    {
        if (emissionRenderer != null && emissionRenderer.materials[1] != null)
            emissionRenderer.materials[1].DisableKeyword("_EMISSION");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive && other.gameObject.layer == Layers.Obj)
        {
            MountObject mountObject = other.GetComponent<MountObject>();

            if (mountObject != null)
            {
                if (mountObject.Mountable)
                {
                    mountObject.Mount(this);
                    Activate();
                }
            }
        }
    }

    public override bool Activate()
    {
        if (!base.Activate())
            return false;

        if (emissionRenderer != null && emissionRenderer.materials[1] != null)
            emissionRenderer.materials[1].EnableKeyword("_EMISSION");

        return true;
    }

    public override bool Deactivate()
    {
        if (!base.Deactivate())
            return false;

        if (emissionRenderer != null && emissionRenderer.materials[1] != null)
            emissionRenderer.materials[1].DisableKeyword("_EMISSION");

        return true;
    }
}
