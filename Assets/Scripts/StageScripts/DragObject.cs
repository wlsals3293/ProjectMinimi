using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController player)
    {
        player.Drag(transform);
    }
}
