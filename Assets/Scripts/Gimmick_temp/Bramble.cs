using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bramble : MonoBehaviour, IHitable
{
    [SerializeField]
    private float burningTime = 3.0f;

    private Coroutine burning;

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

    public void TakeDamage(int amount)
    {
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if (extraDamageInfo.elementType == ElementType.Fire)
        {
            Debug.Log("Create is fire effect & UI processing");

            burning = StartCoroutine(Burning());
        }
        else if (extraDamageInfo.elementType == ElementType.Water)
        {
            Debug.Log("Create is water effect & UI processing");

            if (burning != null)
            {
                StopCoroutine(burning);
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator Burning()
    {
        yield return new WaitForSeconds(burningTime);

        Destroy(this.gameObject);
    }
}