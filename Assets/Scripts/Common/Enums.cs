public enum MinimiType
{
    None,
    Block,
    Fire,
    Wind
}


/// <summary>
/// �ӽ�: fsm������ ���  ���±��� x
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
/// Ȱ��Ű(E) �� ���� �ൿ
/// </summary>
public enum UseKeyActionType
{
    None,
    Block,
    Hold,
}