using UnityEngine;
using UnityEngine.Events;

public class UpdateTimerInstance : TimerInstance
{
    /// <summary>
    /// �� ������ ������Ʈ �� ������ �޼���
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
    /// Ÿ�̸Ӹ� �۵���ŵ�ϴ�.
    /// </summary>
    /// <param name="inCaller">�޼��带 ������ �ִ� ������Ʈ</param>
    /// <param name="updateAction">�� ������ ������Ʈ �� ������ �޼���</param>
    /// <param name="finishAction">Ÿ�̸� ���� �� ������ �޼���</param>
    /// <param name="time">��ǥ�ð�</param>
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
