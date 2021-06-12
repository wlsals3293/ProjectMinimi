using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimi : MonoBehaviour
{
    public const string SEND_SETPIVOT = "SetPivotPosition";


    /// 필드
    
    [SerializeField] private Transform pivot = null;

    [SerializeField] private GameObject bigMesh = null;
    [SerializeField] private GameObject smallMesh = null;

    [Space]
    [SerializeField] private float mass = 2f;

    protected MinimiType minimiType;
    protected MinimiState minimiState = MinimiState.None;


    /// <summary>
    /// 합쳐진 자식 미니미들의 리스트
    /// </summary>
    protected List<Minimi> childMinimis = new List<Minimi>();

    /// <summary>
    /// 처음 설치된 부모 미니미
    /// </summary>
    protected Minimi parentMinimi = null;

    


    /// 프로퍼티

    public MinimiType Type { get => minimiType; }
    public MinimiState State { get => minimiState; }
    public Minimi Parent { get => parentMinimi; }
    public float Mass { get => mass; }
    /// <summary>
    /// 합쳐진 자식 미니미의 개수
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
        // 임시로 생성하자마자 바로 가방으로
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
    /// 미니미 설치
    /// </summary>
    /// <param name="targetPosition">설치될 위치</param>
    /// <param name="targetRotation">설치될 방향</param>
    public virtual void Install(Vector3 targetPosition, Quaternion targetRotation)
    {
        // 임시로 애니메이션이나 이동과정 생략
        gameObject.SetActive(true);
        SetBigState();
        transform.SetPositionAndRotation(targetPosition, targetRotation);
        minimiState = MinimiState.Installed;
    }

    /// <summary>
    /// 미니미 설치 해제
    /// </summary>
    public virtual void Uninstall()
    {
        // 임시로 애니메이션이나 이동과정 생략

        foreach(var child in childMinimis)
        {
            child.Uninstall();
            child.SetParent(null);
        }
        childMinimis.Clear();

        SetSmallState();

        Debug.Log(gameObject.name + " 회수");

        GoIn();
    }

    /// <summary>
    /// 현재 합쳐진 미니미의 개수를 기반으로 현재 상태 업데이트. (부모 미니미에서만 실행)
    /// </summary>
    public virtual void UpdateStatus()
    {
        Debug.LogWarning(gameObject.name + ": 현재 상태 업데이트. 오버라이딩 필요");
    }

    /// <summary>
    /// 가방에 있을 때 손으로 나옴
    /// </summary>
    public virtual void GoOut()
    {
        // 임시로 애니메이션이나 이동과정 생략
        //gameObject.SetActive(true);
        minimiState = MinimiState.OnHand;
    }

    /// <summary>
    /// 손에 있을 때 가방으로 들어감
    /// </summary>
    public virtual void GoIn()
    {
        // 임시로 애니메이션이나 이동과정 생략
        transform.position = Vector3.zero;
        minimiState = MinimiState.InBag;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 미니미를 커짐 상태로 설정
    /// </summary>
    public virtual void SetBigState()
    {
        if(bigMesh == null || smallMesh == null)
        {
            Debug.LogError("메쉬 설정 안됨");
            return;
        }
        smallMesh.SetActive(false);
        bigMesh.SetActive(true);
    }

    /// <summary>
    /// 미니미를 작아짐 상태로 설정
    /// </summary>
    public virtual void SetSmallState()
    {
        if (bigMesh == null || smallMesh == null)
        {
            Debug.LogError("메쉬 설정 안됨");
            return;
        }
        bigMesh.SetActive(false);
        smallMesh.SetActive(true);
    }

    // 임시
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
