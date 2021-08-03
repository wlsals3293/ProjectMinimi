using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingSlope_Obstacle : MonoBehaviour
{
    /// <summary>
    /// 장애물에 부딪히면 플레이어에게 줄 데미지
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
