using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectMinimi;

public class ElectricSwitch : Activator
{

    [SerializeField]
    private Renderer switchRenderer;


    public override bool Activate()
    {
        if (!base.Activate())
            return false;

        if (switchRenderer != null)
            switchRenderer.material.color = Color.yellow;

        return true;
    }

    public override bool Deactivate()
    {
        if (!base.Deactivate())
            return false;

        if (switchRenderer != null)
            switchRenderer.material.color = Color.white;

        return true;
    }
}
