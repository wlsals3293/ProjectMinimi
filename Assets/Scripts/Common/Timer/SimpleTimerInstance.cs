using UnityEngine;
using UnityEngine.Events;

public class SimpleTimerInstance : TimerInstance
{
    /// <summary>
    /// 타이머를 작동시킵니다.
    /// </summary>
    /// <param name="inCaller">메서드를 가지고 있는 컴포넌트</param>
    /// <param name="finishAction">타이머 종료 시 실행할 메서드</param>
    /// <param name="time">목표시간</param>
    public void Run(MonoBehaviour inCaller, UnityAction finishAction, float time)
    {
        isRunning = true;
        caller = inCaller;
        onFinish = finishAction;
        targetTime = time;
        elapsedTime = 0f;
    }
}
