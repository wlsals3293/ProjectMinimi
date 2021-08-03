using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwallowTestBox : MonoBehaviour, ISwallowableObject
{
    private Rigidbody rb;



    private void Awake()
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
        
    }

    public void Swallow()
    {
        gameObject.SetActive(false);
    }
}
