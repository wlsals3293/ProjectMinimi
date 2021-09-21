using UnityEngine;

/// <summary>
/// 애니메이션에서 발생하는 이벤트를 다른 함수로 연결해주는 역할을 하는 컴포넌트
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
    /// 애니메이션 이벤트를 발생시킵니다.
    /// </summary>
    /// <param name="eventNum">발생할 이벤트의 지정된 숫자</param>
    public void EmitEvent(int eventNum)
    {
        if (OnEventEmitted == null)
            return;

        if (OnEventEmitted[eventNum] != null)
            OnEventEmitted[eventNum].Invoke();
    }

}
