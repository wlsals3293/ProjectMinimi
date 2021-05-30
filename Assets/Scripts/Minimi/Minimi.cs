using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimi : MonoBehaviour
{
    /// 필드
    
    protected MinimiType minimiType;
    protected bool isInstalled;


    /// <summary>
    /// 합쳐진 자식 미니미들의 리스트
    /// </summary>
    protected List<Minimi> childMinimis = new List<Minimi>();

    /// <summary>
    /// 처음 설치된 부모 미니미
    /// </summary>
    protected Minimi parentMinimi = null;



    /// 프로퍼티


    public MinimiType MinimiType { get => minimiType; }
    public bool IsInstalled { get => IsInstalled; }

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



    /// <summary>
    /// 미니미 설치
    /// </summary>
    /// <param name="targetPosition">설치될 위치</param>
    /// <param name="targetRotation">설치될 방향</param>
    public virtual void Install(Vector3 targetPosition, Quaternion targetRotation)
    {
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        isInstalled = true;
    }

    /// <summary>
    /// 미니미 설치 해제
    /// </summary>
    public virtual void Uninstall()
    {
        isInstalled = false;
    }

    /// <summary>
    /// 현재 합쳐진 미니미의 개수를 기반으로 현재 상태 업데이트. (부모 미니미에서만 실행)
    /// </summary>
    public virtual void UpdateStatus()
    {
        Debug.LogWarning(gameObject.name + ": 현재 상태 업데이트. 오버라이딩 필요");
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
    /// 지정한 위치에서 합치기가 가능한 가장 가까운 미니미를 반환합니다. 없을 경우 null
    /// </summary>
    /// <param name="origin">찾는 위치</param>
    /// <param name="radius">찾을 범위</param>
    /// <param name="checkType">찾는 미니미의 종류</param>
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
                    Debug.LogWarning("Tag, Component 확인 필요: " + minimis[i].gameObject.name);
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
