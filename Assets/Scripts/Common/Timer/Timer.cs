using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Timer
{
    /// <summary>
    /// 현재 SimpleTimer 인덱스
    /// </summary>
    private static int curSimpleTimerIndex = 0;

    /// <summary>
    /// 현재 UpdateTimer 인덱스
    /// </summary>
    private static int curUpdateTimerIndex = 0;

    /// <summary>
    /// 타이머 인스턴스 목록
    /// </summary>
    private static List<SimpleTimerInstance> simpleTimers = new List<SimpleTimerInstance>();
    private static List<UpdateTimerInstance> updateTimers = new List<UpdateTimerInstance>();


    /// <summary>
    /// 타이머 인스턴스들을 비우고 초기화시킵니다. 씬이 변경됐을 때 호출하면 됩니다.
    /// 현재 GameManager에서 호출하고 있으므로 다른 곳에서 호출하지 말 것.
    /// </summary>
    public static void Initialize()
    {
        curSimpleTimerIndex = 0;
        curUpdateTimerIndex = 0;
        simpleTimers.Clear();
        simpleTimers.Add(new SimpleTimerInstance());
        updateTimers.Clear();
        updateTimers.Add(new UpdateTimerInstance());
    }

    /// <summary>
    /// 일정시간 후 지정한 메서드를 실행시키는 타이머를 작동합니다.
    /// </summary>
    /// <param name="caller">메서드를 가지고 있는 컴포넌트</param>
    /// <param name="finishAction">실행할 메서드</param>
    /// <param name="time">목표시간</param>
    /// <returns>작동하는 타이머 인스턴스</returns>
    public static TimerInstance SetTimer(MonoBehaviour caller, UnityAction finishAction,
        float time)
    {
        if (caller == null)
        {
            Debug.LogError("caller는 null일 수 없습니다");
            return null;
        }

        bool found = false;
        int first = curSimpleTimerIndex;

        // 마지막으로 사용한 인덱스부터 대기 중인 인스턴스를 순차 탐색
        do
        {
            if (!simpleTimers[curSimpleTimerIndex].IsRunning)
            {
                found = true;
                break;
            }
            if (++curSimpleTimerIndex >= simpleTimers.Count)
            {
                curSimpleTimerIndex = 0;
            }
        }
        while (curSimpleTimerIndex != first);

        // 대기 중인 타이머 인스턴스가 하나도 없을 경우, 새 인스턴스를 만듬
        if (!found)
        {
            simpleTimers.Add(new SimpleTimerInstance());
            curSimpleTimerIndex = simpleTimers.Count - 1;
        }

        // 찾은 인스턴스로 타이머 작동
        simpleTimers[curSimpleTimerIndex].Run(caller, finishAction, time);

        return simpleTimers[curSimpleTimerIndex];
    }

    /// <summary>
    /// 일정 시간 동안 매 프레임마다 그리고 종료 시
    /// 각각 지정한 메서드를 실행시키는 타이머를 작동합니다.
    /// </summary>
    /// <param name="caller">메서드를 가지고 있는 컴포넌트</param>
    /// <param name="updateAction">매 프레임 업데이트 시 실행할 메서드</param>
    /// <param name="finishAction">타이머 종료 시 실행할 메서드</param>
    /// <param name="time">목표시간</param>
    /// <returns>작동하는 타이머 인스턴스</returns>
    public static TimerInstance SetTimer(MonoBehaviour caller, UnityAction updateAction,
        UnityAction finishAction, float time)
    {
        if (caller == null)
        {
            Debug.LogError("caller는 null일 수 없습니다");
            return null;
        }

        bool found = false;
        int first = curUpdateTimerIndex;

        // 마지막으로 사용한 인덱스부터 대기 중인 인스턴스를 순차 탐색
        do
        {
            if (!updateTimers[curUpdateTimerIndex].IsRunning)
            {
                found = true;
                break;
            }
            if (++curUpdateTimerIndex >= updateTimers.Count)
            {
                curUpdateTimerIndex = 0;
            }
        }
        while (curUpdateTimerIndex != first);

        // 대기 중인 타이머 인스턴스가 하나도 없을 경우, 새 인스턴스를 만듬
        if (!found)
        {
            updateTimers.Add(new UpdateTimerInstance());
            curUpdateTimerIndex = updateTimers.Count - 1;
        }

        // 찾은 인스턴스로 타이머 작동
        updateTimers[curUpdateTimerIndex].Run(caller, updateAction, finishAction, time);

        return updateTimers[curUpdateTimerIndex];
    }

    /// <summary>
    /// 현재 작동중인 타이머 인스턴스들을 업데이트 합니다.
    /// 현재 GameManager에서 호출하고 있으므로 다른 곳에서 호출하지 말 것.
    /// </summary>
    /// <param name="deltaTime">델타 타임</param>
    public static void UpdateTimers(float deltaTime)
    {
        // SimpleTimer
        for (int i = 0; i < simpleTimers.Count; i++)
        {
            if (simpleTimers[i].IsRunning)
            {
                simpleTimers[i].ElapsedTime += deltaTime;
                if (simpleTimers[i].ElapsedTime >= simpleTimers[i].TargetTime)
                {
                    simpleTimers[i].Finish();
                }
            }
        }

        // UpdateTimer
        for (int i = 0; i < updateTimers.Count; i++)
        {
            if (updateTimers[i].IsRunning)
            {
                updateTimers[i].ElapsedTime += deltaTime;
                if (updateTimers[i].ElapsedTime >= updateTimers[i].TargetTime)
                {
                    updateTimers[i].Finish();
                    continue;
                }

                updateTimers[i].onUpdate?.Invoke();
            }
        }
    }

}