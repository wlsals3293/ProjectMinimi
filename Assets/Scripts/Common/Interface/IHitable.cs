

public interface IHitable
{
    /// <summary>
    /// ���ظ� �޽��ϴ�
    /// </summary>
    /// <param name="amount">���ط�</param>
    /// <param name="extraDamageInfo">�߰� ��������</param>
    void TakeDamage(int amount, ExtraDamageInfo extraDamageInfo = null);
}
