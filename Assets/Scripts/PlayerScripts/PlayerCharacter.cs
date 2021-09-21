using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : MonoBehaviour
{
    [Tooltip("�ִ� ü��")]
    [SerializeField]
    private int maxHP = 3;

    [Tooltip("���� ü��")]
    [SerializeField, ReadOnly]
    private int curHP;

    /// <summary>
    /// ���� ���� �ð�
    /// </summary>
    private float invincibility_Interval = 0f;

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

    private void Update()
    {
        if (isInvincibility)
        {
            invincibility_Interval += Time.deltaTime;

            if (invincibility_Interval > 0.2f)
            {
                isInvincibility = false;
                invincibility_Interval = 0f;
            }
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

    /// <summary>
    /// ���ظ� �޽��ϴ�
    /// </summary>
    /// <param name="amount">���ط�</param>
    public void TakeDamage(int amount)
    {
        if (!isInvincibility)
        {
            int newHP = curHP - amount;

            SetHP(newHP);

            // ������ ���� �Ϳ� ���� ����Ʈ, UI ó�� ���
        }
    }

    /// <summary>
    /// ª�� ������ ���Ե� ���ظ� �޽��ϴ�
    /// </summary>
    /// <param name="amount">���ط�</param>
    /// <param name="hitDirection">���ظ� �ִ� ����</param>
    public void TakeDamage(int amount, Vector3 hitDirection)
    {
        if (!isInvincibility)
        {
            int newHP = curHP - amount;

            SetHP(newHP);

            controller.ActivateHitDisorder(hitDirection);

            isInvincibility = true;
        }
    }
}
