using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Timer
{
    /// <summary>
    /// ���� �ε���
    /// </summary>
    private static int curIndex = 0;

    /// <summary>
    /// Ÿ�̸� �ν��Ͻ� ���
    /// </summary>
    private static List<TimerInstance> timers = new List<TimerInstance>();

    /// <summary>
    /// �� ������ ������Ʈ�� ȣ��
    /// </summary>
    public static UnityAction<float> onUpdate;


    /// <summary>
    /// Ÿ�̸� �ν��Ͻ����� ���� �ʱ�ȭ��ŵ�ϴ�. ���� ������� �� ȣ���ϸ� �˴ϴ�.
    /// ���� GameManager���� ȣ���ϰ� �����Ƿ� �ٸ� ������ ȣ������ �� ��.
    /// </summary>
    public static void Initialize()
    {
        curIndex = 0;
        onUpdate = null;
        timers.Clear();
        timers.Add(new TimerInstance());
    }

    /// <summary>
    /// �����ð� �� ������ �޼��带 �����Ű�� Ÿ�̸Ӹ� �۵��մϴ�.
    /// </summary>
    /// <param name="caller">�޼��带 ������ �ִ� ������Ʈ</param>
    /// <param name="callback">������ �޼���</param>
    /// <param name="time">��ǥ�ð�</param>
    /// <returns>�۵��ϴ� Ÿ�̸� �ν��Ͻ�</returns>
    public static TimerInstance SetTimer(MonoBehaviour caller, UnityAction callback, float time)
    {
        if (caller == null)
        {
            Debug.LogError("caller�� null�� �� �����ϴ�");
            return null;
        }

        bool found = false;
        int first = curIndex;

        // ���������� ����� �ε������� ��� ���� �ν��Ͻ��� ���� Ž��
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

        // ��� ���� Ÿ�̸� �ν��Ͻ��� �ϳ��� ���� ���, �� �ν��Ͻ��� ����
        if (!found)
        {
            timers.Add(new TimerInstance());
            curIndex = timers.Count - 1;
        }

        // ã�� �ν��Ͻ��� Ÿ�̸� �۵�
        timers[curIndex].Run(caller, callback, time);

        return timers[curIndex];
    }

    /// <summary>
    /// ���� �۵����� Ÿ�̸� �ν��Ͻ����� ������Ʈ �մϴ�.
    /// ���� GameManager���� ȣ���ϰ� �����Ƿ� �ٸ� ������ ȣ������ �� ��.
    /// </summary>
    /// <param name="deltaTime">��Ÿ Ÿ��</param>
    public static void UpdateTimers(float deltaTime)
    {
        onUpdate?.Invoke(deltaTime);
    }

}

public class TimerInstance
{
    /// <summary>
    /// �۵� ����
    /// </summary>
    private bool isRunning;

    /// <summary>
    /// ����ð�
    /// </summary>
    private float elapsedTime;

    /// <summary>
    /// ��ǥ�ð�
    /// </summary>
    private float targetTime;

    /// <summary>
    /// ��ǥ�ð��� �������� �� ������ �޼���
    /// </summary>
    private UnityAction onComplete;

    /// <summary>
    /// �޼��带 ������ �ִ� ������Ʈ
    /// </summary>
    private MonoBehaviour caller;



    /// <summary>
    /// �۵� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsRunning { get => isRunning; }

    /// <summary>
    /// ����ð��� ��ȯ�մϴ�.
    /// </summary>
    public float ElapsedTime { get => elapsedTime; }

    /// <summary>
    /// ��ǥ�ð��� ��ȯ�մϴ�.
    /// </summary>
    public float TargetTime { get => targetTime; }



    /// <summary>
    /// Ÿ�̸Ӹ� �۵���ŵ�ϴ�.
    /// </summary>
    /// <param name="inCaller">�޼��带 ������ �ִ� ������Ʈ</param>
    /// <param name="callback">��ǥ�ð��� �������� �� ������ �޼���</param>
    /// <param name="time">��ǥ�ð�</param>
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
    /// Ÿ�̸Ӹ� ������Ʈ ��ŵ�ϴ�. GameManager�� ���� �ڵ����� ȣ��Ǿ� ó���˴ϴ�.
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
    /// Ÿ�̸��� �۵��� �ߴ��մϴ�.
    /// </summary>
    public void Cancel()
    {
        Timer.onUpdate -= Update;
        onComplete = null;
        caller = null;
        isRunning = false;
    }

    /// <summary>
    /// ����ð��� 0���� ����� Ÿ�̸Ӹ� ó������ �ٽý����մϴ�. (����)
    /// </summary>
    public void Renew()
    {
        elapsedTime = 0f;
    }

    /// <summary>
    /// Ÿ�̸Ӱ� ��ǥ�ð��� �������� �� ó���� ���� ȣ���ϴ� �޼����Դϴ�.
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
