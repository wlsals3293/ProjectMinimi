using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TestBox : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            impulseSource.GenerateImpulse();
        }
    }
}