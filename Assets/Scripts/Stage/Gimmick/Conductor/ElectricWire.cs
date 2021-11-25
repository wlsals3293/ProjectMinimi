using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWire : MonoBehaviour
{
    [SerializeField]
    private Renderer[] wireRenderers;


    public void Activate()
    {
        foreach (Renderer wire in wireRenderers)
        {
            wire.material.color = Color.yellow;
        }
    }

    public void Deactivate()
    {
        foreach (Renderer wire in wireRenderers)
        {
            wire.material.color = Color.white;
        }
    }
}
