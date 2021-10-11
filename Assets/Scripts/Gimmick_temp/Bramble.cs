using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bramble : MonoBehaviour, IHitable
{
    [SerializeField]
    private float burning = 3.0f;

    private bool isBurn = false;
    private bool isDigestion = false;

    private void OnCollisionEnter(Collision collision)
    {
        ExtraDamageInfo extraDamageInfo;

        if (collision.gameObject.layer == Layers.Player)
        {
            PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();

            if (player != null)
            {
                extraDamageInfo = new ExtraDamageInfo(transform.position);
                player.TakeDamage(1, extraDamageInfo);
            }
        }
    }

    private void Update()
    {
        if (isBurn == true)
        {
            burning -= Time.deltaTime;
        }

        if(burning > 0.0f)
        {
            Debug.Log("Create is Water effect & UI processing");
            if (isDigestion == true)
            {
                Destroy(this.gameObject);
            }
        }
        else if (burning < 0.0f)
        {
            Debug.Log("Create is fire effect & UI processing");
            Destroy(this.gameObject);
            isBurn = false;
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