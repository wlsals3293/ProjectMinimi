using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Timer
{
    /// <summary>
    /// ���� SimpleTimer �ε���
    /// </summary>
    private static int curSimpleTimerIndex = 0;

    /// <summary>
    /// ���� UpdateTimer �ε���
    /// </summary>
    private static int curUpdateTimerIndex = 0;

    /// <summary>
    /// Ÿ�̸� �ν��Ͻ� ���
    /// </summary>
    private static List<SimpleTimerInstance> simpleTimers = new List<SimpleTimerInstance>();
    private static List<UpdateTimerInstance> updateTimers = new List<UpdateTimerInstance>();


    /// <summary>
    /// Ÿ�̸� �ν��Ͻ����� ���� �ʱ�ȭ��ŵ�ϴ�. ���� ������� �� ȣ���ϸ� �˴ϴ�.
    /// ���� GameManager���� ȣ���ϰ� �����Ƿ� �ٸ� ������ ȣ������ �� ��.
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
    /// �����ð� �� ������ �޼��带 �����Ű�� Ÿ�̸Ӹ� �۵��մϴ�.
    /// </summary>
    /// <param name="caller">�޼��带 ������ �ִ� ������Ʈ</param>
    /// <param name="finishAction">������ �޼���</param>
    /// <param name="time">��ǥ�ð�</param>
    /// <returns>�۵��ϴ� Ÿ�̸� �ν��Ͻ�</returns>
    public static TimerInstance SetTimer(MonoBehaviour caller, UnityAction finishAction,
        float time)
    {
        if (caller == null)
        {
            Debug.LogError("caller�� null�� �� �����ϴ�");
            return null;
        }

        bool found = false;
        int first = curSimpleTimerIndex;

        // ���������� ����� �ε������� ��� ���� �ν��Ͻ��� ���� Ž��
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

        // ��� ���� Ÿ�̸� �ν��Ͻ��� �ϳ��� ���� ���, �� �ν��Ͻ��� ����
        if (!found)
        {
            simpleTimers.Add(new SimpleTimerInstance());
            curSimpleTimerIndex = simpleTimers.Count - 1;
        }

        // ã�� �ν��Ͻ��� Ÿ�̸� �۵�
        simpleTimers[curSimpleTimerIndex].Run(caller, finishAction, time);

        return simpleTimers[curSimpleTimerIndex];
    }

    /// <summary>
    /// ���� �ð� ���� �� �����Ӹ��� �׸��� ���� ��
    /// ���� ������ �޼��带 �����Ű�� Ÿ�̸Ӹ� �۵��մϴ�.
    /// </summary>
    /// <param name="caller">�޼��带 ������ �ִ� ������Ʈ</param>
    /// <param name="updateAction">�� ������ ������Ʈ �� ������ �޼���</param>
    /// <param name="finishAction">Ÿ�̸� ���� �� ������ �޼���</param>
    /// <param name="time">��ǥ�ð�</param>
    /// <returns>�۵��ϴ� Ÿ�̸� �ν��Ͻ�</returns>
    public static TimerInstance SetTimer(MonoBehaviour caller, UnityAction updateAction,
        UnityAction finishAction, float time)
    {
        if (caller == null)
        {
            Debug.LogError("caller�� null�� �� �����ϴ�");
            return null;
        }

        bool found = false;
        int first = curUpdateTimerIndex;

        // ���������� ����� �ε������� ��� ���� �ν��Ͻ��� ���� Ž��
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

        // ��� ���� Ÿ�̸� �ν��Ͻ��� �ϳ��� ���� ���, �� �ν��Ͻ��� ����
        if (!found)
        {
            updateTimers.Add(new UpdateTimerInstance());
            curUpdateTimerIndex = updateTimers.Count - 1;
        }

        // ã�� �ν��Ͻ��� Ÿ�̸� �۵�
        updateTimers[curUpdateTimerIndex].Run(caller, updateAction, finishAction, time);

        return updateTimers[curUpdateTimerIndex];
    }

    /// <summary>
    /// ���� �۵����� Ÿ�̸� �ν��Ͻ����� ������Ʈ �մϴ�.
    /// ���� GameManager���� ȣ���ϰ� �����Ƿ� �ٸ� ������ ȣ������ �� ��.
    /// </summary>
    /// <param name="deltaTime">��Ÿ Ÿ��</param>
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