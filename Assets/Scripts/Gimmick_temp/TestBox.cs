using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour, IHitable
{
    public void TakeDamage(int amount)
    {
        Debug.Log(gameObject.name + ": damaged=" + amount);
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        Debug.Log($"{gameObject.name}, damage:{amount} hitPoint:{extraDamageInfo.hitPoint} element:{extraDamageInfo.elementType}");
    }
}