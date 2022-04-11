using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterBase : MonoBehaviour, IHitable
{
    [Tooltip("최대 체력")]
    [SerializeField]
    protected int maxHP = 3;

    [Tooltip("현재 체력")]
    [SerializeField, ReadOnly]
    protected int curHP;

    /// <summary>
    /// 현재 무적 상태 여부
    /// </summary>
    protected bool isInvincibility = false;

    /// <summary>
    /// 현재 사망 상태 여부
    /// </summary>
    protected bool isDead = false;


    /// <summary>
    /// 캐릭터의 HP가 변경되었을 때 호출
    /// </summary>
    public UnityAction<int> onHpChanged;

    /// <summary>
    /// 캐릭터의 HP가 0이되었을 때 호출
    /// </summary>
    public UnityAction onHpZero;


    public int MaxHP { get => maxHP; }
    public int CurHP { get => curHP; }

    public bool IsInvincibility { get => isInvincibility; }


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
            isDead = true;

            onHpZero?.Invoke();
        }

        curHP = newHP;

        // 현재 체력 나타내는 UI는 여기서 업데이트
        onHpChanged?.Invoke(curHP);
    }

    public abstract void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo = null);
}
