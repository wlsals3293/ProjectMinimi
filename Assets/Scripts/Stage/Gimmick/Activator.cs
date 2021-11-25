using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [Header("[ Activator ]")]

    [Tooltip("Activator가 속해있는 라디오 그룹")]
    [ReadOnly]
    public ActivatorRadioGroup radioGroup;

    [Tooltip("활성화신호를 보낼 연결된 Activatee들 입니다.")]
    [SerializeField]
    protected List<Activatee> activatees = new List<Activatee>();

    /*[Tooltip("체크되어 있으면 한 번 활성화되면 비활성화되지 않습니다.")]
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
            // 중복 제거
            activatees = activatees.Distinct().ToList();


            /* 기존에 복사된 리스트와 비교 후 바뀐 요소가 있으면 추가된건지
             * 삭제된건지 순서가 바뀐건지 판단 후 연결된 요소에도 적절한 처리
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

