using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Timer
{
    private static int curIndex = 0;


    private static List<TimerInstance> timers = new List<TimerInstance>();


    public static UnityAction<float> onUpdate;


    public static void Initialize()
    {
        curIndex = 0;
        onUpdate = null;
        timers.Clear();
        timers.Add(new TimerInstance());
    }

    public static TimerInstance SetTimer(MonoBehaviour caller, UnityAction callback, float time)
    {
        if (caller == null)
        {
            Debug.LogError("inCaller는 null일 수 없습니다");
            return null;
        }

        bool found = false;
        int first = curIndex;

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

        if (!found)
        {
            timers.Add(new TimerInstance());
            curIndex = timers.Count - 1;
        }

        timers[curIndex].Run(caller, callback, time);

        return timers[curIndex];
    }

    public static void UpdateTimers(float deltaTime)
    {
        onUpdate?.Invoke(deltaTime);
    }

}

public class TimerInstance
{
    private bool isRunning;
    private float elapsedTime;
    private float targetTime;

    private UnityAction onComplete;

    private MonoBehaviour caller;


    public bool IsRunning { get => isRunning; }
    public float ElapsedTime { get => elapsedTime; }
    public float TargetTime { get => targetTime; }


    public void Run(MonoBehaviour inCaller, UnityAction callback, float time)
    {
        isRunning = true;
        caller = inCaller;
        onComplete = callback;
        targetTime = time;
        elapsedTime = 0f;
        Timer.onUpdate += Update;
    }

    public void Update(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (elapsedTime >= targetTime)
        {
            Finish();
        }
    }

    public void Cancel()
    {
        Timer.onUpdate -= Update;
        onComplete = null;
        caller = null;
        isRunning = false;
    }

    public void Renew()
    {
        elapsedTime = 0f;
    }

    private void Finish()
    {
        Timer.onUpdate -= Update;

        if(caller != null && caller.isActiveAndEnabled)
            onComplete?.Invoke();

        onComplete = null;
        caller = null;
        isRunning = false;
    }
}
