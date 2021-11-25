using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [Header("[ Activator ]")]

    [Tooltip("Activator�� �����ִ� ���� �׷�")]
    [ReadOnly]
    public ActivatorRadioGroup radioGroup;

    [Tooltip("Ȱ��ȭ��ȣ�� ���� ����� Activatee�� �Դϴ�.")]
    [SerializeField]
    protected List<Activatee> activatees = new List<Activatee>();

    /*[Tooltip("üũ�Ǿ� ������ �� �� Ȱ��ȭ�Ǹ� ��Ȱ��ȭ���� �ʽ��ϴ�.")]
    [SerializeField]
    protected bool permanent;
    */
    protected bool isActive;


#if UNITY_EDITOR
    private List<Activatee> activatees_copy = new List<Activatee>();
#endif



    public List<Activatee> Activatees { get => activatees; }

    public bool IsActive { get => isActive; }



#if UNITY_EDITOR
    private void OnValidate()
    {
        if (activatees != null)
        {
            // �ߺ� ����
            activatees = activatees.Distinct().ToList();


            /* ������ ����� ����Ʈ�� �� �� �ٲ� ��Ұ� ������ �߰��Ȱ���
             * �����Ȱ��� ������ �ٲ���� �Ǵ� �� ����� ��ҿ��� ������ ó��
             */
            bool changed = false;
            int index = activatees.Count - 1;
            int copyIndex = activatees_copy.Count - 1;

            while (index >= 0 || copyIndex >= 0)
            {
                if (copyIndex < 0)
                {
                    if (activatees[index] != null)
                        activatees[index].AddActivator(this);
                    else
                        activatees.RemoveAt(index);
                    index--;
                    changed = true;
                    continue;
                }
                if (index < 0)
                {
                    activatees_copy[copyIndex--].RemoveActivator(this);
                    changed = true;
                    continue;
                }

                if (activatees_copy[copyIndex] == null)
                {
                    copyIndex--;
                    changed = true;
                    continue;
                }
                if (activatees[index] == null)
                {
                    activatees.RemoveAt(index--);
                    changed = true;
                    continue;
                }

                if (activatees[index] == activatees_copy[copyIndex])
                {
                    index--;
                    copyIndex--;
                    continue;
                }

                changed = true;

                if (activatees_copy.Contains(activatees[index]))
                {
                    if (activatees.Contains(activatees_copy[copyIndex]))
                    {
                        index--;
                        copyIndex--;
                        continue;
                    }
                    else
                    {
                        activatees_copy[copyIndex--].RemoveActivator(this);
                    }
                }
                else
                {
                    activatees[index--].AddActivator(this);
                }
            }

            if (changed)
            {
                activatees_copy.Clear();
                activatees_copy.AddRange(activatees);
            }
        }
    }
#endif


    public virtual bool Activate()
    {
        if (isActive)
            return false;

        isActive = true;

        for (int i = 0; i < activatees.Count; i++)
        {
            if (activatees[i] != null)
                activatees[i].ReceiveSignal(true);
        }
        if (radioGroup != null)
        {
            radioGroup.ActivateOnly(this);
        }

        return true;
    }

    public virtual bool Deactivate()
    {
        if (!isActive)
            return false;

        isActive = false;

        for (int i = 0; i < activatees.Count; i++)
        {
            if (activatees[i] != null)
                activatees[i].ReceiveSignal(false);
        }

        return true;
    }
}

