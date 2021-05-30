using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimi : MonoBehaviour
{
    /// �ʵ�
    
    protected MinimiType minimiType;
    protected bool isInstalled;


    /// <summary>
    /// ������ �ڽ� �̴Ϲ̵��� ����Ʈ
    /// </summary>
    protected List<Minimi> childMinimis = new List<Minimi>();

    /// <summary>
    /// ó�� ��ġ�� �θ� �̴Ϲ�
    /// </summary>
    protected Minimi parentMinimi = null;



    /// ������Ƽ


    public MinimiType MinimiType { get => minimiType; }
    public bool IsInstalled { get => IsInstalled; }

    /// <summary>
    /// ������ �ڽ� �̴Ϲ��� ����
    /// </summary>
    public int ChildCount
    {
        get { return childMinimis.Count; }
    }

    public bool IsParent
    {
        get { return (parentMinimi == null && childMinimis.Count > 0); }
    }



    /// <summary>
    /// �̴Ϲ� ��ġ
    /// </summary>
    /// <param name="targetPosition">��ġ�� ��ġ</param>
    /// <param name="targetRotation">��ġ�� ����</param>
    public virtual void Install(Vector3 targetPosition, Quaternion targetRotation)
    {
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        isInstalled = true;
    }

    /// <summary>
    /// �̴Ϲ� ��ġ ����
    /// </summary>
    public virtual void Uninstall()
    {
        isInstalled = false;
    }

    /// <summary>
    /// ���� ������ �̴Ϲ��� ������ ������� ���� ���� ������Ʈ. (�θ� �̴Ϲ̿����� ����)
    /// </summary>
    public virtual void UpdateStatus()
    {
        Debug.LogWarning(gameObject.name + ": ���� ���� ������Ʈ. �������̵� �ʿ�");
    }

    public void SetParent(Minimi minimi)
    {
        parentMinimi = minimi;
    }

    public void AddChild(Minimi minimi)
    {
        childMinimis.Add(minimi);
        minimi.SetParent(this);
    }

    public void ClearChild()
    {
        foreach (var child in childMinimis)
        {
            child.SetParent(null);
        }
        childMinimis.Clear();
    }


    protected void Merge(Minimi parent)
    {
        parent.AddChild(this);

        foreach(var myChild in childMinimis)
        {
            parent.AddChild(myChild);
        }
        childMinimis.Clear();

        parent.UpdateStatus();
    }

    /// <summary>
    /// ������ ��ġ���� ��ġ�Ⱑ ������ ���� ����� �̴Ϲ̸� ��ȯ�մϴ�. ���� ��� null
    /// </summary>
    /// <param name="origin">ã�� ��ġ</param>
    /// <param name="radius">ã�� ����</param>
    /// <param name="checkType">ã�� �̴Ϲ��� ����</param>
    /// <returns></returns>
    protected Minimi GetMergeableMinimi(Vector3 origin, float radius, MinimiType checkType)
    {
        Collider[] minimis = Physics.OverlapSphere(origin, radius, Layers.minimi);
        float nearestDistanceSqr = 99999.0f;
        Minimi nearestMinimi = null;

        for (int i = 0; i < minimis.Length; i++)
        {
            if (minimis[i].gameObject.CompareTag(Tags.minimi))
            {
                Minimi curMinimi = minimis[i].gameObject.GetComponent<Minimi>();

                if (curMinimi == null)
                {
                    Debug.LogWarning("Tag, Component Ȯ�� �ʿ�: " + minimis[i].gameObject.name);
                    continue;
                }

                if (curMinimi.ChildCount + ChildCount >= MinimiManager.MAX_STACK_COUNT - 1)
                {
                    continue;
                }

                if (curMinimi.MinimiType == checkType)
                {
                    float distanceSqr = (curMinimi.transform.position - origin).sqrMagnitude;
                    if(distanceSqr <= nearestDistanceSqr)
                    {
                        nearestDistanceSqr = distanceSqr;
                        nearestMinimi = curMinimi;
                    }
                }
            }
        }

        return nearestMinimi;
    }
}
