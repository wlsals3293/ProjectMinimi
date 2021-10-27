using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Timer
{
    /// <summary>
    /// 현재 인덱스
    /// </summary>
    private static int curIndex = 0;

    /// <summary>
    /// 타이머 인스턴스 목록
    /// </summary>
    private static List<TimerInstance> timers = new List<TimerInstance>();

    /// <summary>
    /// 매 프레임 업데이트시 호출
    /// </summary>
    public static UnityAction<float> onUpdate;


    /// <summary>
    /// 타이머 인스턴스들을 비우고 초기화시킵니다. 씬이 변경됐을 때 호출하면 됩니다.
    /// 현재 GameManager에서 호출하고 있으므로 다른 곳에서 호출하지 말 것.
    /// </summary>
    public static void Initialize()
    {
        curIndex = 0;
        onUpdate = null;
        timers.Clear();
        timers.Add(new TimerInstance());
    }

    /// <summary>
    /// 일정시간 후 지정한 메서드를 실행시키는 타이머를 작동합니다.
    /// </summary>
    /// <param name="caller">메서드를 가지고 있는 컴포넌트</param>
    /// <param name="callback">실행할 메서드</param>
    /// <param name="time">목표시간</param>
    /// <returns>작동하는 타이머 인스턴스</returns>
    public static TimerInstance SetTimer(MonoBehaviour caller, UnityAction callback, float time)
    {
        if (caller == null)
        {
            Debug.LogError("caller는 null일 수 없습니다");
            return null;
        }

        bool found = false;
        int first = curIndex;

        // 마지막으로 사용한 인덱스부터 대기 중인 인스턴스를 순차 탐색
        do
        {
            if (!timers[curIndex].IsRunning)
            {
                found = true;
                break;
            }
            if (++curIndex >= timers.Count)
            {
                curIndex = 0;
            }
        }
        while (curIndex != first);

        // 대기 중인 타이머 인스턴스가 하나도 없을 경우, 새 인스턴스를 만듬
        if (!found)
        {
            timers.Add(new TimerInstance());
            curIndex = timers.Count - 1;
        }

        // 찾은 인스턴스로 타이머 작동
        timers[curIndex].Run(caller, callback, time);

        return timers[curIndex];
    }

    /// <summary>
    /// 현재 작동중인 타이머 인스턴스들을 업데이트 합니다.
    /// 현재 GameManager에서 호출하고 있으므로 다른 곳에서 호출하지 말 것.
    /// </summary>
    /// <param name="deltaTime">델타 타임</param>
    public static void UpdateTimers(float deltaTime)
    {
        onUpdate?.Invoke(deltaTime);
    }

}

public class TimerInstance
{
    /// <summary>
    /// 작동 여부
    /// </summary>
    private bool isRunning;

    /// <summary>
    /// 경과시간
    /// </summary>
    private float elapsedTime;

    /// <summary>
    /// 목표시간
    /// </summary>
    private float targetTime;

    /// <summary>
    /// 목표시간에 도달했을 때 실행할 메서드
    /// </summary>
    private UnityAction onComplete;

    /// <summary>
    /// 메서드를 가지고 있는 컴포넌트
    /// </summary>
    private MonoBehaviour caller;



    /// <summary>
    /// 작동 여부를 반환합니다.
    /// </summary>
    public bool IsRunning { get => isRunning; }

    /// <summary>
    /// 경과시간을 반환합니다.
    /// </summary>
    public float ElapsedTime { get => elapsedTime; }

    /// <summary>
    /// 목표시간을 반환합니다.
    /// </summary>
    public float TargetTime { get => targetTime; }



    /// <summary>
    /// 타이머를 작동시킵니다.
    /// </summary>
    /// <param name="inCaller">메서드를 가지고 있는 컴포넌트</param>
    /// <param name="callback">목표시간에 도달했을 때 실행할 메서드</param>
    /// <param name="time">목표시간</param>
    public void Run(MonoBehaviour inCaller, UnityAction callback, float time)
    {
        isRunning = true;
        caller = inCaller;
        onComplete = callback;
        targetTime = time;
        elapsedTime = 0f;
        Timer.onUpdate += Update;
    }

    /// <summary>
    /// 타이머를 업데이트 시킵니다. GameManager를 통해 자동으로 호출되어 처리됩니다.
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (elapsedTime >= targetTime)
        {
            Finish();
        }
    }

    /// <summary>
    /// 타이머의 작동을 중단합니다.
    /// </summary>
    public void Cancel()
    {
        Timer.onUpdate -= Update;
        onComplete = null;
        caller = null;
        isRunning = false;
    }

    /// <summary>
    /// 경과시간을 0으로 만들어 타이머를 처음부터 다시시작합니다. (갱신)
    /// </summary>
    public void Renew()
    {
        elapsedTime = 0f;
    }

    /// <summary>
    /// 타이머가 목표시간에 도달했을 때 처리를 위해 호출하는 메서드입니다.
    /// </summary>
    private void Finish()
    {
        Timer.onUpdate -= Update;

        if (caller != null && caller.isActiveAndEnabled)
            onComplete?.Invoke();

        onComplete = null;
        caller = null;
        isRunning = false;
    }
}
