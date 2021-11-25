using UnityEngine;
using UnityEngine.Events;

public class UpdateTimerInstance : TimerInstance
{
    /// <summary>
    /// 매 프레임 업데이트 시 실행할 메서드
    /// </summary>
    public UnityAction onUpdate;


    public override void Cancel()
    {
        onUpdate = null;
        base.Cancel();
    }

    public override void Finish()
    {
        base.Finish();
        onUpdate = null;
    }

    /// <summary>
    /// 타이머를 작동시킵니다.
    /// </summary>
    /// <param name="inCaller">메서드를 가지고 있는 컴포넌트</param>
    /// <param name="updateAction">매 프레임 업데이트 시 실행할 메서드</param>
    /// <param name="finishAction">타이머 종료 시 실행할 메서드</param>
    /// <param name="time">목표시간</param>
    public void Run(MonoBehaviour inCaller, UnityAction updateAction,
        UnityAction finishAction, float time)
    {
        isRunning = true;
        caller = inCaller;
        onUpdate = updateAction;
        onFinish = finishAction;
        targetTime = time;
        elapsedTime = 0f;
    }
}
