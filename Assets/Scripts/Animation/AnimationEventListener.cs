using UnityEngine;

/// <summary>
/// �ִϸ��̼ǿ��� �߻��ϴ� �̺�Ʈ�� �ٸ� �Լ��� �������ִ� ������ �ϴ� ������Ʈ
/// </summary>
public class AnimationEventListener : MonoBehaviour
{
    public delegate void OnEventDelegate();
    public OnEventDelegate[] OnEventEmitted;
    public int emitterCount = 1;


    private void Awake()
    {
        OnEventEmitted = new OnEventDelegate[emitterCount];
    }

    /// <summary>
    /// �ִϸ��̼� �̺�Ʈ�� �߻���ŵ�ϴ�.
    /// </summary>
    /// <param name="eventNum">�߻��� �̺�Ʈ�� ������ ����</param>
    public void EmitEvent(int eventNum)
    {
        if (OnEventEmitted == null)
            return;

        if (OnEventEmitted[eventNum] != null)
            OnEventEmitted[eventNum].Invoke();
    }

}
