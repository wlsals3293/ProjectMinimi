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
    Sliding,
    Drag,
    LedgeGrab
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
    UI,
    RainyCloud
}

public enum SoundType
{
    Music,
    Sfx,
    PlayerVoice,
    Ambient,
    UI
}

public enum ElementType
{
    None,
    Fire,
    Water,
    Electricity
}

public enum ElectricSwitchDurationType
{
    Continuation,
    OnOff,
}

public enum LogicCondition
{
    And,
    Or
}