using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterBase : MonoBehaviour, IHitable
{
    [Tooltip("�ִ� ü��")]
    [SerializeField]
    protected int maxHP = 3;

    [Tooltip("���� ü��")]
    [SerializeField, ReadOnly]
    protected int curHP;

    /// <summary>
    /// ���� ���� ���� ����
    /// </summary>
    protected bool isInvincibility = false;

    /// <summary>
    /// ���� ��� ���� ����
    /// </summary>
    protected bool isDead = false;


    /// <summary>
    /// ĳ������ HP�� ����Ǿ��� �� ȣ��
    /// </summary>
    public UnityAction<int> onHpChanged;

    /// <summary>
    /// ĳ������ HP�� 0�̵Ǿ��� �� ȣ��
    /// </summary>
    public UnityAction onHpZero;


    public int MaxHP { get => maxHP; }
    public int CurHP { get => curHP; }

    public bool IsInvincibility { get => isInvincibility; }


    /// <summary>
    /// ���� ü���� �����մϴ�
    /// </summary>
    /// <param name="newHP">������ ü��</param>
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

        // ���� ü�� ��Ÿ���� UI�� ���⼭ ������Ʈ
        onHpChanged?.Invoke(curHP);
    }

    public abstract void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo = null);
}
