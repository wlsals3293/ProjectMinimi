using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimi : MonoBehaviour
{
    public const string SEND_SETPIVOT = "SetPivotPosition";


    /// �ʵ�
    
    [SerializeField] private Transform pivot = null;

    [SerializeField] private GameObject bigMesh = null;
    [SerializeField] private GameObject smallMesh = null;

    [Space]
    [SerializeField] private float mass = 2f;

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
    public Minimi Parent { get => parentMinimi; }
    public float Mass { get => mass; }
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
    

    public virtual void Initialize()
    {
        // �ӽ÷� �������ڸ��� �ٷ� ��������
        MinimiManager._instance.GainMinimi(this);
        GoIn();
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
        gameObject.SetActive(true);
        SetBigState();
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

        SetSmallState();

        Debug.Log(gameObject.name + " ȸ��");

        GoIn();
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
        //gameObject.SetActive(true);
        minimiState = MinimiState.OnHand;
    }

    /// <summary>
    /// �տ� ���� �� �������� ��
    /// </summary>
    public virtual void GoIn()
    {
        // �ӽ÷� �ִϸ��̼��̳� �̵����� ����
        transform.position = Vector3.zero;
        minimiState = MinimiState.InBag;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �̴Ϲ̸� Ŀ�� ���·� ����
    /// </summary>
    public virtual void SetBigState()
    {
        if(bigMesh == null || smallMesh == null)
        {
            Debug.LogError("�޽� ���� �ȵ�");
            return;
        }
        smallMesh.SetActive(false);
        bigMesh.SetActive(true);
    }

    /// <summary>
    /// �̴Ϲ̸� �۾��� ���·� ����
    /// </summary>
    public virtual void SetSmallState()
    {
        if (bigMesh == null || smallMesh == null)
        {
            Debug.LogError("�޽� ���� �ȵ�");
            return;
        }
        bigMesh.SetActive(false);
        smallMesh.SetActive(true);
    }

    // �ӽ�
    public void SetPivotPosition(Transform trans)
    {
        if(childMinimis.Count > 0)
        {
            childMinimis[childMinimis.Count - 1].SetPivotPosition(trans);
            return;
        }

        if (pivot != null && trans != null)
        {
            trans.position = pivot.position;
        }
    }
}
