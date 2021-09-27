using UnityEngine;

public interface IHitable
{
    /// <summary>
    /// ���ظ� �޽��ϴ�
    /// </summary>
    /// <param name="amount">���ط�</param>
    public void TakeDamage(int amount);

    /// <summary>
    /// �߰������� ���Ե� ���ظ� �޽��ϴ�
    /// </summary>
    /// <param name="amount">���ط�</param>
    /// <param name="extraDamageInfo">�߰� ��������</param>
    public void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo);
}
