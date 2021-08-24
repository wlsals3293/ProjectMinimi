using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwallowStone : MonoBehaviour, ISwallowableObject
{
    Rigidbody rb;
    bool isSpitting = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public bool IsContinuous()
    {
        return false;
    }

    public void Spit(Vector3 startPosition, Vector3 spitVelocity)
    {
        gameObject.SetActive(true);
        transform.position = startPosition;
        rb.AddForce(spitVelocity, ForceMode.VelocityChange);
        isSpitting = true;
    }

    public void Swallow()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isSpitting)
        {
            if (collision.gameObject.CompareTag(Tags.DistructibleObject))
            {
                collision.gameObject.SetActive(false);
            }
            //gameObject.SetActive(false);
        }
    }


}
