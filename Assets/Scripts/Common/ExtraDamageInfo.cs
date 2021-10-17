using UnityEngine;

public struct ExtraDamageInfo
{
    /// <summary>
    /// 피해가 발생한 지점
    /// </summary>
    public Vector3 hitPoint;

    /// <summary>
    /// 원소종류
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
