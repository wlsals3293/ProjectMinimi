using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class SpiritTest : MonoBehaviour
{
    private BehaviorTree bt;

    // Start is called before the first frame update
    void Start()
    {
        bt = GetComponent<BehaviorTree>();
    }

    public void SetTransform()
    {
        bt.SetVariableValue("PlayerTransform", PlayerManager.Instance.PlayerChar.gameObject);
    }

}
