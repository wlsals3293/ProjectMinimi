using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController player)
    {
        player.Hold(transform);
    }
}
