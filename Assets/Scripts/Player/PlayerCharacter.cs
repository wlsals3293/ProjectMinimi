using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : MonoBehaviour, IHitable
{
    [Tooltip("최대 체력")]
    [SerializeField]
    private int maxHP = 3;

    [Tooltip("현재 체력")]
    [SerializeField, ReadOnly]
    private int curHP;

    /// <summary>
    /// 현재 무적 상태 여부
    /// </summary>
    private bool isInvincibility = false;


    private PlayerController controller = null;


    /// <summary>
    /// 캐릭터의 HP가 변경되었을 때 호출
    /// </summary>
    public UnityAction<int> onHpChanged;


    public int MaxHP { get => maxHP; }
    public int CurHP { get => curHP; }


    private void Awake()
    {
        curHP = maxHP;

        controller = GetComponent<PlayerController>();
        if (controller == null)
        {
            Debug.LogError("PlayerController 없음");
        }
    }

    /// <summary>
    /// 현재 체력을 설정합니다
    /// </summary>
    /// <param name="newHP">설정할 체력</param>
    public void SetHP(int newHP)
    {
        newHP = Mathf.Min(newHP, maxHP);

        if (newHP <= 0)
        {
            newHP = 0;

            controller.ChangeState(PlayerState.Dead);
        }

        curHP = newHP;

        // 현재 체력 나타내는 UI는 여기서 업데이트
        onHpChanged?.Invoke(curHP);
    }


    public void TakeDamage(int amount)
    {
        if (!isInvincibility)
        {
            int newHP = curHP - amount;

            SetHP(newHP);

            isInvincibility = true;
            Timer.SetTimer(this, () => isInvincibility = false, 0.5f);

            // 데미지 입은 것에 대한 이펙트, UI 처리 등등
        }
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if (!isInvincibility)
        {
            int newHP = curHP - amount;

            SetHP(newHP);

            // 짧은 경직 발생
            Vector3 hitDirection = transform.position - extraDamageInfo.hitPoint;
            controller.ActivateHitDisorder(hitDirection);

            isInvincibility = true;
            Timer.SetTimer(this, () => isInvincibility = false, 0.5f);

            // 데미지 입은 것에 대한 이펙트, UI 처리 등등
        }
    }
}
