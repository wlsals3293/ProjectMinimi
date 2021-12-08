using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectMinimi
{
    public class Activatee : MonoBehaviour
    {
        [Header("[ Activatee ]")]

        [Tooltip("활성화되었을 때 실행할 이벤트")]
        public UnityEvent activationEvent;

        [Tooltip("비활성화되었을 때 실행할 이벤트")]
        public UnityEvent deactivationEvent;

        [BeginReadOnlyGroup]
        [Tooltip("연결되어 있는 Activator들 입니다.")]
        [SerializeField]
        protected List<Activator> activators = new List<Activator>();
        [EndReadOnlyGroup]

        [Tooltip("논리 조건. And일 경우 연결된 모든 Activator가 활성화 신호를 보냈을 때 활성화." +
            " Or일 경우 하나 이상의 활성화 신호를 받았을 때 활성화.")]
        [SerializeField]
        protected LogicCondition condition;

        [Tooltip("체크되어 있으면 게임시작시 활성화됩니다.")]
        [SerializeField]
        protected bool activateOnStart;

        [Tooltip("체크되어 있으면 한 번 활성화되면 비활성화되지 않습니다.")]
        [SerializeField]
        protected bool permanent;


        protected bool isActive;


        public List<Activator> Activators { get => activators; }

        public bool IsActive { get => isActive; }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (activators != null)
            {
                for (int i = activators.Count - 1; i >= 0; i--)
                {
                    if (activators[i] == null || !activators[i].Activatees.Contains(this))
                        activators.RemoveAt(i);
                }
            }
        }
#endif

        public void AddActivator(Activator activator)
        {
            if (!activators.Contains(activator))
                activators.Add(activator);
        }

        public void RemoveActivator(Activator activator)
        {
            if (activators.Contains(activator))
                activators.Remove(activator);
        }

        public void ReceiveSignal(bool signal)
        {
            if (signal && !isActive)
            {
                if (condition == LogicCondition.And)
                {
                    foreach (Activator acti in activators)
                    {
                        if (!acti.IsActive)
                        {
                            return;
                        }
                    }

                    Activate();
                }
                else if (condition == LogicCondition.Or)
                {
                    Activate();
                }
            }
            else if (!signal && isActive && !permanent)
            {
                Deactivate();
            }
        }

        protected virtual bool Activate()
        {
            if (isActive)
                return false;

            isActive = true;
            activationEvent?.Invoke();

            return true;
        }

        protected virtual bool Deactivate()
        {
            if (!isActive)
                return false;

            isActive = false;
            deactivationEvent?.Invoke();

            return true;
        }

    }
}
