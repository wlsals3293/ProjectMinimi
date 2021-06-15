using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{

    [SerializeField] private List<Checkpoint> checkpoints = new List<Checkpoint>();

    private void Start()
    {
        StageManager.Instance.InitializeCheckpoints(checkpoints);
    }
}
