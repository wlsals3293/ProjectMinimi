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

    public ExtraDamageInfo(Vector3 inHitPoint, ElementType inElementType = ElementType.None)
    {
        hitPoint = inHitPoint;
        elementType = inElementType;
    }

}
