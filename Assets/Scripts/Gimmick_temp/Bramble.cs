using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bramble : MonoBehaviour, IHitable
{
    [SerializeField]
    private float burning = 3.0f;

    private bool isBurn = false;
    private bool isDigestion = false;

    private void OnTriggerEnter(Collider other)
    {
        SpiritProjectile projectile = other.gameObject.GetComponent<SpiritProjectile>();

        ExtraDamageInfo extraDamageInfo;

        if (other.gameObject.layer == Layers.Player)
        {
            PlayerCharacter player = other.gameObject.GetComponent<PlayerCharacter>();

            if (player != null)
            {
                extraDamageInfo = new ExtraDamageInfo(transform.position);
                player.TakeDamage(1, extraDamageInfo);
            }
        }
        else if (other.gameObject == projectile.gameObject)
        {
            if (projectile != null)
            {
                if(other.gameObject.name == "FireSpiritProjectile(Clone)")
                {
                    Debug.Log("Create is fire effect & UI processing");
                    extraDamageInfo = new ExtraDamageInfo(ElementType.Fire);
                    this.TakeDamage(0, extraDamageInfo);
                }
                else if (other.gameObject.name == "WaterSpiritProjectile(Clone)")
                {
                    Debug.Log("Create is water effect & UI processing");
                    extraDamageInfo = new ExtraDamageInfo(ElementType.Water);
                    this.TakeDamage(0, extraDamageInfo);
                }

                Destroy(projectile.gameObject);
            }
        }
    }

    private void Update()
    {
        if (isBurn)
        {
            burning -= Time.deltaTime;
        }

        if (burning > 0.0f)
        {
            if (isDigestion)
            {
                Destroy(this.gameObject);
            }
        }
        else if (burning < 0.0f)
        {
            Destroy(this.gameObject);
            isBurn = false;
            isDigestion = false;
        }
    }

    public void TakeDamage(int amount)
    {
    }
    
    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if (extraDamageInfo.elementType == ElementType.Fire)
        {
            isBurn = true;
        }
        else if (extraDamageInfo.elementType == ElementType.Water)
        {
            isDigestion = true;
        }
    }
}