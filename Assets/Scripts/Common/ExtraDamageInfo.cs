using UnityEngine;

public struct ExtraDamageInfo
{
    /// <summary>
    /// ���ذ� �߻��� ����
    /// </summary>
    public Vector3 hitPoint;

    /// <summary>
    /// ��������
    /// </summary>
    public ElementType elementType;

    public Transform damageCauser;

    public ExtraDamageInfo(Vector3 inHitPoint, ElementType inElementType = ElementType.None)
    {
        hitPoint = inHitPoint;
        elementType = inElementType;
        damageCauser = null;
    }

    public ExtraDamageInfo(ElementType inElementType)
    {
        hitPoint = Vector3.zero;
        elementType = inElementType;
        damageCauser = null;
    }

    public ExtraDamageInfo(Vector3 inHitPoint, ElementType inElementType, Transform inDamageCauser)
    {
        hitPoint = inHitPoint;
        elementType = inElementType;
        damageCauser = inDamageCauser;
    }
}
