using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectMinimi
{
    public class Activatee : MonoBehaviour
    {
        [Header("[ Activatee ]")]

        [Tooltip("Ȱ��ȭ�Ǿ��� �� ������ �̺�Ʈ")]
        public UnityEvent activationEvent;

        [Tooltip("��Ȱ��ȭ�Ǿ��� �� ������ �̺�Ʈ")]
        public UnityEvent deactivationEvent;

        [BeginReadOnlyGroup]
        [Tooltip("����Ǿ� �ִ� Activator�� �Դϴ�.")]
        [SerializeField]
        protected List<Activator> activators = new List<Activator>();
        [EndReadOnlyGroup]

        [Tooltip("�� ����. And�� ��� ����� ��� Activator�� Ȱ��ȭ ��ȣ�� ������ �� Ȱ��ȭ." +
            " Or�� ��� �ϳ� �̻��� Ȱ��ȭ ��ȣ�� �޾��� �� Ȱ��ȭ.")]
        [SerializeField]
        protected LogicCondition condition;

        [Tooltip("üũ�Ǿ� ������ ���ӽ��۽� Ȱ��ȭ�˴ϴ�.")]
        [SerializeField]
        protected bool activateOnStart;

        [Tooltip("üũ�Ǿ� ������ �� �� Ȱ��ȭ�Ǹ� ��Ȱ��ȭ���� �ʽ��ϴ�.")]
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
