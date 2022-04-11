using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacter : EnemyCharacter
{



    private void Awake()
    {
        curHP = maxHP;

        /*controller = GetComponent<PlayerController>();
        if (controller == null)
        {
            Debug.LogError("PlayerController ����");
        }*/

        //onHpZero += () => controller.ChangeState(PlayerState.Dead);
    }

    public override void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo = null)
    {
        if (!isInvincibility)
        {
            int newHP = curHP - amount;

            SetHP(newHP);

            if (isDead)
                return;

            if (extraDamageInfo != null)
            {
                // ª�� ���� �߻�
                Vector3 hitDirection = transform.position - extraDamageInfo.hitPoint;
                //controller.ActivateHitDisorder(hitDirection);
            }

            isInvincibility = true;
            Timer.SetTimer(this, () => isInvincibility = false, 0.5f);

            // ������ ���� �Ϳ� ���� ����Ʈ, UI ó�� ���
        }
    }
}
