using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimi : MonoBehaviour
{
    /// �ʵ�
    
    protected MinimiType minimiType;
    protected MinimiState minimiState = MinimiState.None;


    /// <summary>
    /// ������ �ڽ� �̴Ϲ̵��� ����Ʈ
    /// </summary>
    protected List<Minimi> childMinimis = new List<Minimi>();

    /// <summary>
    /// ó�� ��ġ�� �θ� �̴Ϲ�
    /// </summary>
    protected Minimi parentMinimi = null;



    /// ������Ƽ

    public MinimiType Type { get => minimiType; }
    public MinimiState State { get => minimiState; }

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
    /// �̴Ϲ� ��ġ
    /// </summary>
    /// <param name="targetPosition">��ġ�� ��ġ</param>
    /// <param name="targetRotation">��ġ�� ����</param>
    public virtual void Install(Vector3 targetPosition, Quaternion targetRotation)
    {
        // �ӽ÷� �ִϸ��̼��̳� �̵����� ����
        transform.SetPositionAndRotation(targetPosition, targetRotation);
        minimiState = MinimiState.Installed;
    }

    /// <summary>
    /// �̴Ϲ� ��ġ ����
    /// </summary>
    public virtual void Uninstall()
    {
        // �ӽ÷� �ִϸ��̼��̳� �̵����� ����

        foreach(var child in childMinimis)
        {
            child.Uninstall();
            child.SetParent(null);
        }
        childMinimis.Clear();


        minimiState = MinimiState.InBag;
    }

    /// <summary>
    /// ���� ������ �̴Ϲ��� ������ ������� ���� ���� ������Ʈ. (�θ� �̴Ϲ̿����� ����)
    /// </summary>
    public virtual void UpdateStatus()
    {
        Debug.LogWarning(gameObject.name + ": ���� ���� ������Ʈ. �������̵� �ʿ�");
    }

    /// <summary>
    /// ���濡 ���� �� ������ ����
    /// </summary>
    public virtual void GoOut()
    {
        // �ӽ÷� �ִϸ��̼��̳� �̵����� ����
        minimiState = MinimiState.OnHand;
    }

    /// <summary>
    /// �տ� ���� �� �������� ��
    /// </summary>
    public virtual void GoIn()
    {
        // �ӽ÷� �ִϸ��̼��̳� �̵����� ����
        minimiState = MinimiState.InBag;
    }
}
