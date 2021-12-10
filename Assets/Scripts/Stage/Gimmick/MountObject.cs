using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MountObject : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Renderer emissionRenderer;

    private Rigidbody rbody;

    private MountSwitch mountedSwitch;

    private bool mountable = true;


    public bool Mountable { get => mountable; }


    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();

        if (emissionRenderer != null)
            emissionRenderer.material.DisableKeyword("_EMISSION");
    }

    public void Interact(PlayerController player)
    {
        player.Hold(transform);
        UnMount();
    }

    public void Mount(MountSwitch targetSwitch)
    {
        mountedSwitch = targetSwitch;
        rbody.isKinematic = true;

        Vector3 mountPos = targetSwitch.transform.position;
        mountPos += targetSwitch.transform.up * 0.9f;

        transform.position = mountPos;
        mountable = false;

        if (emissionRenderer != null)
            emissionRenderer.material.EnableKeyword("_EMISSION");
    }

    private void UnMount()
    {
        if (mountable)
            return;

        rbody.isKinematic = false;

        if (mountedSwitch != null)
        {
            mountedSwitch.Deactivate();
        }
        mountedSwitch = null;

        Timer.SetTimer(this, () => mountable = true, 1f);

        if (emissionRenderer != null)
            emissionRenderer.material.DisableKeyword("_EMISSION");
    }
}
