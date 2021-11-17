/// <summary>
/// 미니미의 종류
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
/// 미니미의 상태
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
/// 임시: fsm에서는 모든  상태구현 x
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