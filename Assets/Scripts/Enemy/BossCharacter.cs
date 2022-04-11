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
            Debug.LogError("PlayerController 없음");
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
                // 짧은 경직 발생
                Vector3 hitDirection = transform.position - extraDamageInfo.hitPoint;
                //controller.ActivateHitDisorder(hitDirection);
            }

            isInvincibility = true;
            Timer.SetTimer(this, () => isInvincibility = false, 0.5f);

            // 데미지 입은 것에 대한 이펙트, UI 처리 등등
        }
    }
}
