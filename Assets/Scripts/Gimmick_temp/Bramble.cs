using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bramble : MonoBehaviour, IHitable
{
    [SerializeField]
    private float burningTime = 3.0f;

    private TimerInstance burning;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Layers.Player)
        {
            PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();

            if (player != null)
            {
                ExtraDamageInfo extraDamageInfo = new ExtraDamageInfo(transform.position);
                player.TakeDamage(1, extraDamageInfo);
            }
        }
    }

    public void TakeDamage(int amount)
    {
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if (extraDamageInfo.elementType == ElementType.Fire)
        {
            Debug.Log("Create is fire effect & UI processing");

            burning = Timer.SetTimer(this, Burning, 3.0f);
        }
        else if (extraDamageInfo.elementType == ElementType.Water)
        {
            Debug.Log("Create is water effect & UI processing");

            if (burning != null)
            {
                burning.Cancel();
                Burning();
            }
        }
    }

    private void Burning()
    {
        burning = null;
        Destroy(this.gameObject);
    }
}