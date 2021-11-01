using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSlope_Obstacle : MonoBehaviour
{
    /// <summary>
    /// ��ֹ��� �ε����� �÷��̾�� �� ������
    /// </summary>
    [SerializeField] private int damage = 1;

    private Collider col = null;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Player))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            player.CrashObstacle(damage);

            col.enabled = false;

            Destroy(gameObject);
        }
    }
}
