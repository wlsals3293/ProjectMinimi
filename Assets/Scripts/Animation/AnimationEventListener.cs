using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    public delegate void OnEventDelegate();
    public OnEventDelegate[] OnEventEmitted;
    public int emitterCount = 1;


    private void Awake()
    {
        OnEventEmitted = new OnEventDelegate[emitterCount];
    }

    public void EmitEvent(int eventNum)
    {
        if (OnEventEmitted == null)
            return;

        if (OnEventEmitted[eventNum] != null)
            OnEventEmitted[eventNum].Invoke();
    }

}
