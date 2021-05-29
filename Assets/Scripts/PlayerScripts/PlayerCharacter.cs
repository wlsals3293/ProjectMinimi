using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private int maxHP = 3;
    private int curHP;

    private void Awake()
    {
        curHP = maxHP;
    }
}
