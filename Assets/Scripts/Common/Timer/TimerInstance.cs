using UnityEngine;
using UnityEngine.Events;

public abstract class TimerInstance
{
    /// <summary>
    /// 작동 여부
    /// </summary>
    protected bool isRunning;

    /// <summary>
    /// 경과시간
    /// </summary>
    protected float elapsedTime;

    /// <summary>
    /// 목표시간
    /// </summary>
    protected float targetTime;

    /// <summary>
    /// 타이머 종료 시 실행할 메서드
    /// </summary>
    protected UnityAction onFinish;

    /// <summary>
    /// 메서드를 가지고 있는 컴포넌트
    /// </summary>
    protected MonoBehaviour caller;



    /// <summary>
    /// 작동 여부
    /// </summary>
    public bool IsRunning { get => isRunning; }

    /// <summary>
    /// 경과시간
    /// </summary>
    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }

    /// <summary>
    /// 목표시간
    /// </summary>
    public float TargetTime { get => targetTime; }



    /// <summary>
    /// 타이머의 작동을 중단합니다.
    /// </summary>
    public virtual void Cancel()
    {
        onFinish = null;
        caller = null;
        isRunning = false;
    }

    /// <summary>
    /// (갱신) 타이머를 처음부터 다시시작합니다. 
    /// 이미 종료된 타이머가 다시 작동하지는 않습니다.
    /// </summary>
    /// <param name="newTargetTime">새로운 목표시간</param>
    public void Restart(float newTargetTime = 0f)
    {
        elapsedTime = 0f;

        if (newTargetTime > 0f)
        {
            targetTime = newTargetTime;
        }
    }

    /// <summary>
    /// 타이머가 목표시간에 도달했을 때 처리를 위해 호출하는 메서드입니다.
    /// 목표시간을 넘기면 자동으로 호출됩니다.
    /// </summary>
    public virtual void Finish()
    {
        if (caller != null && caller.isActiveAndEnabled)
            onFinish?.Invoke();

        onFinish = null;
        caller = null;
        isRunning = false;
    }
}
