using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MinimiManager : MonoBehaviour
{
    
    public const int MAX_STACK_COUNT = 3;
    public const float MERGE_DISTANCE = 1.0f;


    public static MinimiManager _instance = null;


    /// <summary>
    /// 맵상에 있는 모든 미니미의 리스트
    /// </summary>
    private Dictionary<MinimiType, List<Minimi>> allMinimiLists = new Dictionary<MinimiType, List<Minimi>>();
    
    /// <summary>
    /// 플레이어가 소유하고 있는 미니미의 리스트
    /// </summary>
    private Dictionary<MinimiType, List<Minimi>> ownMinimiLists = new Dictionary<MinimiType, List<Minimi>>();

    /// <summary>
    /// 손에 들고 있는 미니미의 리스트
    /// </summary>
    private List<Minimi> onHandMinimiList = new List<Minimi>();

    /// <summary>
    /// 현재 설치하려고 준비중인 미니미의 종류
    /// </summary>
    private MinimiType onHandMinimiType = MinimiType.None;




    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        for(int i=1; i<(int)MinimiType.Max; i++)
        {
            allMinimiLists.Add((MinimiType)i, new List<Minimi>());
            ownMinimiLists.Add((MinimiType)i, new List<Minimi>());
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="minimiType"></param>
    /// <returns></returns>
    public Minimi CreateMinimi(MinimiType minimiType)
    {
        Minimi newMinimi = null;

        switch(minimiType)
        {
            case MinimiType.Block:
                newMinimi = ResourceManager.Instance.CreatePrefab<BlockMinimi>(PrefabNames.Minimi_Dump);
                break;
            case MinimiType.Fire:
                //newMinimi = ResourceManager.Instance.CreatePrefab<FireMinimi>(PrefabNames.Minimi_Dump);
                break;
            case MinimiType.Wind:
                //newMinimi = ResourceManager.Instance.CreatePrefab<WindMinimi>(PrefabNames.Minimi_Dump);
                break;
            default:
                break;
        }

        if(newMinimi == null)
            return null;

        allMinimiLists[minimiType].Add(newMinimi);
        return newMinimi;
    }

    /// <summary>
    /// 가방에서 미니미를 꺼냄
    /// </summary>
    /// <param name="minimiType">꺼낸 미니미의 종류</param>
    /// <returns>성공 여부</returns>
    public bool TakeOutMinimi(MinimiType minimiType)
    {
        if (minimiType != onHandMinimiType)
        {
            onHandMinimiType = minimiType;

            foreach(var m in onHandMinimiList)
            {
                m.GoIn();
            }

            onHandMinimiList.Clear();
        }

        if (onHandMinimiList.Count >= MAX_STACK_COUNT)
            return false;


        foreach (var minimi in ownMinimiLists[onHandMinimiType])
        {
            if (minimi.State == MinimiState.InBag)
            {
                onHandMinimiList.Add(minimi);
                minimi.GoOut();
                return true;
            }
        }


        return false;
    }

    /// <summary>
    /// 현재 손에 들고 있는 미니미를 모두 다시 가방에 집어넣음
    /// </summary>
    public void PutInAllMinimis()
    {
        foreach (var minimi in onHandMinimiList)
        {
            minimi.GoIn();
        }
        onHandMinimiList.Clear();
        onHandMinimiType = MinimiType.None;
    }


    public void InstallMinimi(Vector3 position, Quaternion rotation)
    {
        if (onHandMinimiType == MinimiType.None || onHandMinimiList.Count < 1)
            return;


        Minimi parent = GetMergeableMinimi(position, MERGE_DISTANCE);
        
        if(parent == null)
        {
            parent = onHandMinimiList[0];

            for (int i = 1; i < onHandMinimiList.Count; i++)
            {
                parent.AddChild(onHandMinimiList[i]);
            }

            parent.Install(position, rotation);
        }
        else
        {
            for (int i = 0; i < onHandMinimiList.Count; i++)
            {
                parent.AddChild(onHandMinimiList[i]);
            }
        }

        parent.UpdateStatus();

        onHandMinimiList.Clear();
        onHandMinimiType = MinimiType.None;
    }


    /// <summary>
    /// 지정한 위치에서 합치기가 가능한 가장 가까운 미니미를 반환합니다. 없을 경우 null
    /// </summary>
    /// <param name="origin">찾는 위치</param>
    /// <param name="radius">찾을 범위</param>
    /// <returns>찾은 미니미</returns>
    public Minimi GetMergeableMinimi(Vector3 origin, float radius)
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

                if (curMinimi.State != MinimiState.Installed || !curMinimi.IsParent ||
                    curMinimi.ChildCount + onHandMinimiList.Count >= MAX_STACK_COUNT)
                {
                    continue;
                }

                if (curMinimi.Type == onHandMinimiType)
                {
                    float distanceSqr = (curMinimi.transform.position - origin).sqrMagnitude;
                    if (distanceSqr <= nearestDistanceSqr)
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
