using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable_Renderer : MonoBehaviour
{
    MeshRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
        render.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
