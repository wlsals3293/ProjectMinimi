public enum MinimiType
{
    None,
    Block,
    Fire,
    Wind
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