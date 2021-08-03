using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSlope : MonoBehaviour
{
    PlayerController pc = null;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(Tags.Player))
        {
            pc = collision.gameObject.GetComponent<PlayerController>();

            pc.SetSliding(this);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag(Tags.Player))
        {
            pc.ChangeState(PlayerState.Idle);

            pc = null;
        }
    }
}
