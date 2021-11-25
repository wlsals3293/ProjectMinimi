using UnityEngine;
using UnityEngine.Events;

public class SimpleTimerInstance : TimerInstance
{
    /// <summary>
    /// Ÿ�̸Ӹ� �۵���ŵ�ϴ�.
    /// </summary>
    /// <param name="inCaller">�޼��带 ������ �ִ� ������Ʈ</param>
    /// <param name="finishAction">Ÿ�̸� ���� �� ������ �޼���</param>
    /// <param name="time">��ǥ�ð�</param>
    public void Run(MonoBehaviour inCaller, UnityAction finishAction, float time)
    {
        isRunning = true;
        caller = inCaller;
        onFinish = finishAction;
        targetTime = time;
        elapsedTime = 0f;
    }
}
