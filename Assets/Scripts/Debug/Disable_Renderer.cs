using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable_Renderer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
