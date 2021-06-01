using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MinimiManager : MonoBehaviour
{
    
    public const int MAX_STACK_COUNT = 3;
    public const float MERGE_DISTANCE = 1.0f;


    public static MinimiManager _instance = null;


    /// <summary>
    /// �ʻ� �ִ� ��� �̴Ϲ��� ����Ʈ
    /// </summary>
    private Dictionary<MinimiType, List<Minimi>> allMinimiLists = new Dictionary<MinimiType, List<Minimi>>();
    
    /// <summary>
    /// �÷��̾ �����ϰ� �ִ� �̴Ϲ��� ����Ʈ
    /// </summary>
    private Dictionary<MinimiType, List<Minimi>> ownMinimiLists = new Dictionary<MinimiType, List<Minimi>>();

    /// <summary>
    /// �տ� ��� �ִ� �̴Ϲ��� ����Ʈ
    /// </summary>
    private List<Minimi> onHandMinimiList = new List<Minimi>();

    /// <summary>
    /// ���� ��ġ�Ϸ��� �غ����� �̴Ϲ��� ����
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
    /// ���濡�� �̴Ϲ̸� ����
    /// </summary>
    /// <param name="minimiType">���� �̴Ϲ��� ����</param>
    /// <returns>���� ����</returns>
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
    /// ���� �տ� ��� �ִ� �̴Ϲ̸� ��� �ٽ� ���濡 �������
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
    /// ������ ��ġ���� ��ġ�Ⱑ ������ ���� ����� �̴Ϲ̸� ��ȯ�մϴ�. ���� ��� null
    /// </summary>
    /// <param name="origin">ã�� ��ġ</param>
    /// <param name="radius">ã�� ����</param>
    /// <returns>ã�� �̴Ϲ�</returns>
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
                    Debug.LogWarning("Tag, Component Ȯ�� �ʿ�: " + minimis[i].gameObject.name);
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
