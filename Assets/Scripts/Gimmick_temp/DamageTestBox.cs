using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTestBox : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == Layers.Player)
        {
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();

            if(player != null)
            {
                Vector3 hitDirection = other.transform.position - transform.position;

                player.TakeDamage(1, hitDirection);
            }

        }
    }
}
