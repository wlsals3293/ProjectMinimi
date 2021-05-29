public enum MinimiType
{
    None,
    Block,
    Fire,
    Wind
}


public enum PlayerState
{
    Idle,
    Jump,
    CreateMini, // 즉시 혹은 가이드라인?
    Glider,
    LiftingObject,
    Interacting,
}

public enum ActiveKeyType
{
    Use // key E
}