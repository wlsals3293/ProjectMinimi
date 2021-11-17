using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : MonoBehaviour, IHitable
{
    [Tooltip("�ִ� ü��")]
    [SerializeField]
    private int maxHP = 3;

    [Tooltip("���� ü��")]
    [SerializeField, ReadOnly]
    private int curHP;

    /// <summary>
    /// ���� ���� ���� ����
    /// </summary>
    private bool isInvincibility = false;


    private PlayerController controller = null;


    /// <summary>
    /// ĳ������ HP�� ����Ǿ��� �� ȣ��
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
            Debug.LogError("PlayerController ����");
        }
    }

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

            controller.ChangeState(PlayerState.Dead);
        }

        curHP = newHP;

        // ���� ü�� ��Ÿ���� UI�� ���⼭ ������Ʈ
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

            // ������ ���� �Ϳ� ���� ����Ʈ, UI ó�� ���
        }
    }

    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo)
    {
        if (!isInvincibility)
        {
            int newHP = curHP - amount;

            SetHP(newHP);

            // ª�� ���� �߻�
            Vector3 hitDirection = transform.position - extraDamageInfo.hitPoint;
            controller.ActivateHitDisorder(hitDirection);

            isInvincibility = true;
            Timer.SetTimer(this, () => isInvincibility = false, 0.5f);

            // ������ ���� �Ϳ� ���� ����Ʈ, UI ó�� ���
        }
    }
}
