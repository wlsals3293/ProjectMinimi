/// <summary>
/// �̴Ϲ��� ����
/// </summary>
public enum MinimiType
{
    None,
    Block,
    Fire,
    Wind,
    Max
}

/// <summary>
/// �̴Ϲ��� ����
/// </summary>
public enum MinimiState
{
    None,
    Wait,
    Move,
    ToBag,
    InBag,
    ToHand,
    OnHand,
    Installed
}


/// <summary>
/// �ӽ�: fsm������ ���  ���±��� x
/// </summary>
public enum PlayerState
{
    None,
    Idle,
    Dead,

    Hold,
    Climb,
    Aim,
    Sliding
}

/// <summary>
/// Ȱ��Ű(E) �� ���� �ൿ
/// </summary>
public enum InteractType
{
    None,
    Block,
    Hold,
}


[System.Serializable]
public enum DirectionType
{
    X,
    Y,
    Z,
    All,
}


public enum PrefabPath
{
    Root,
    Camera,
    UI
}