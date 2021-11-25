using UnityEngine;
using UnityEngine.Events;

public abstract class TimerInstance
{
    /// <summary>
    /// �۵� ����
    /// </summary>
    protected bool isRunning;

    /// <summary>
    /// ����ð�
    /// </summary>
    protected float elapsedTime;

    /// <summary>
    /// ��ǥ�ð�
    /// </summary>
    protected float targetTime;

    /// <summary>
    /// Ÿ�̸� ���� �� ������ �޼���
    /// </summary>
    protected UnityAction onFinish;

    /// <summary>
    /// �޼��带 ������ �ִ� ������Ʈ
    /// </summary>
    protected MonoBehaviour caller;



    /// <summary>
    /// �۵� ����
    /// </summary>
    public bool IsRunning { get => isRunning; }

    /// <summary>
    /// ����ð�
    /// </summary>
    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }

    /// <summary>
    /// ��ǥ�ð�
    /// </summary>
    public float TargetTime { get => targetTime; }



    /// <summary>
    /// Ÿ�̸��� �۵��� �ߴ��մϴ�.
    /// </summary>
    public virtual void Cancel()
    {
        onFinish = null;
        caller = null;
        isRunning = false;
    }

    /// <summary>
    /// (����) Ÿ�̸Ӹ� ó������ �ٽý����մϴ�. 
    /// �̹� ����� Ÿ�̸Ӱ� �ٽ� �۵������� �ʽ��ϴ�.
    /// </summary>
    /// <param name="newTargetTime">���ο� ��ǥ�ð�</param>
    public void Restart(float newTargetTime = 0f)
    {
        elapsedTime = 0f;

        if (newTargetTime > 0f)
        {
            targetTime = newTargetTime;
        }
    }

    /// <summary>
    /// Ÿ�̸Ӱ� ��ǥ�ð��� �������� �� ó���� ���� ȣ���ϴ� �޼����Դϴ�.
    /// ��ǥ�ð��� �ѱ�� �ڵ����� ȣ��˴ϴ�.
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
