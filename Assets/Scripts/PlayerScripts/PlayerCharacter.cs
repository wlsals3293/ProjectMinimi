using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private int maxHP = 3;

    [SerializeField, ReadOnly]
    private int curHP;

    private PlayerController controller = null;

    float Invincibility_Interval = 0f;

    bool isInvincibility = false;

    public int MaxHP { get => maxHP; }
    public int CurHP { get => curHP; }


    private void Awake()
    {
        curHP = maxHP;

        controller = GetComponent<PlayerController>();
        if(controller == null)
        {
            Debug.LogError("PlayerController ����");
        }
    }

    private void Update()
    {
        if (isInvincibility)
        {
            Invincibility_Interval += Time.deltaTime;

            if(Invincibility_Interval > 0.2f)
            {
                isInvincibility = false;
                Invincibility_Interval = 0f;
            }
        }
    }

    public void SetHP(int newHP)
    {
        newHP = Mathf.Min(newHP, maxHP);

        if(newHP <= 0)
        {
            newHP = 0;

            controller.ChangeState(PlayerState.Dead);
        }

        curHP = newHP;

        // ���� ü�� ��Ÿ���� UI�� ���⼭ ������Ʈ
    }

    public void TakeDamage(int amount)
    {
        int newHP = curHP - amount;

        SetHP(newHP);

        // ������ ���� �Ϳ� ���� ����Ʈ, UI ó�� ���
    }

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
