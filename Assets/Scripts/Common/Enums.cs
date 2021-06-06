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
    Run,

    Jump_Start,
    Air,
    Jump_End,
    Dead,

    Hold_Start,
    Holding,
    Hold_End,

    Climb,
}

/// <summary>
/// 활용키(E) 에 따른 행동
/// </summary>
public enum UseKeyActionType
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
