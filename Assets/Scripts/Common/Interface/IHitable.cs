using UnityEngine;

public interface IHitable
{
    /// <summary>
    /// 피해를 받습니다
    /// </summary>
    /// <param name="amount">피해량</param>
    public void TakeDamage(int amount);

    /// <summary>
    /// 추가정보가 포함된 피해를 받습니다
    /// </summary>
    /// <param name="amount">피해량</param>
    /// <param name="extraDamageInfo">추가 피해정보</param>
    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo);
}
