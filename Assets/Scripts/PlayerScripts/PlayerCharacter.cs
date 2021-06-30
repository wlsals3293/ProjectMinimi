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

    private void Awake()
    {
        curHP = maxHP;

        controller = GetComponent<PlayerController>();
        if(controller == null)
        {
            Debug.LogError("PlayerController ����");
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
}
