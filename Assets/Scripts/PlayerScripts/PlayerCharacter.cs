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
            Debug.LogError("PlayerController 없음");
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

        // 현재 체력 나타내는 UI는 여기서 업데이트
    }

    public void TakeDamage(int amount)
    {
        int newHP = curHP - amount;

        SetHP(newHP);

        // 데미지 입은 것에 대한 이펙트, UI 처리 등등
    }
}
