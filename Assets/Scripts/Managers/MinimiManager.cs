using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MinimiManager : MonoBehaviour
{
    
    public const int MAX_STACK_COUNT = 3;
    public const float MERGE_DISTANCE = 1.0f;


    public static MinimiManager _instance = null;

    private List<BlockMinimi> blockMinimis = new List<BlockMinimi>();
    private List<FireMinimi> fireMinimis = new List<FireMinimi>();
    private List<WindMinimi> windMinimis = new List<WindMinimi>();

    /// <summary>
    /// ���� ��ġ�Ϸ��� �غ����� �̴Ϲ��� ����
    /// </summary>
    private MinimiType onHandMinimiType = MinimiType.None;

    /// <summary>
    /// ���� ��ġ�Ϸ��� �غ����� �̴Ϲ��� ����
    /// </summary>
    public int onHandMinimiCount = 0;



    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public void CreateMinimi(MinimiType minimiType)
    {
        Minimi newMinimi = null;

        switch(minimiType)
        {
            case MinimiType.Block:
                newMinimi = ResourceManager.Instance.CreatePrefab<BlockMinimi>(PrefabNames.Minimi_Dump);
                blockMinimis.Add(newMinimi as BlockMinimi);
                break;
            case MinimiType.Fire:
                //newMinimi = ResourceManager.Instance.CreatePrefab<FireMinimi>(PrefabNames.Minimi_Dump);
                //fireMinimis.Add(newMinimi as FireMinimi);
                break;
            case MinimiType.Wind:
                //newMinimi = ResourceManager.Instance.CreatePrefab<WindMinimi>(PrefabNames.Minimi_Dump);
                //windMinimis.Add(newMinimi as WindMinimi);
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// ���濡�� �̴Ϲ̸� ����
    /// </summary>
    /// <param name="minimiType">���� �̴Ϲ��� ����</param>
    public void TakeOutMinimi(MinimiType minimiType)
    {
        if (minimiType != onHandMinimiType)
        {
            onHandMinimiCount = 0;
            onHandMinimiType = minimiType;
        }

        if (onHandMinimiCount < MAX_STACK_COUNT)
        {
            onHandMinimiCount++;
        }
    }

    /// <summary>
    /// ���� �տ� ��� �ִ� �̴Ϲ̸� ��� �ٽ� ���濡 �������
    /// </summary>
    public void PutInMinimis()
    {
        onHandMinimiCount = 0;
        onHandMinimiType = MinimiType.None;
    }


    public void InstallMinimi(Vector3 position)
    {
        if (onHandMinimiType == MinimiType.None || onHandMinimiCount <= 0)
            return;

        Minimi parent = null;
        int targetCount = onHandMinimiCount;

        onHandMinimiCount = 0;
        onHandMinimiType = MinimiType.None;


        for (int i=0; i<blockMinimis.Count; i++)
        {
            if(!blockMinimis[i].IsInstalled)
            {
                if(parent == null)
                {
                    parent = blockMinimis[i];
                }
                else
                {
                    parent.AddChild(blockMinimis[i]);
                }

                if(--targetCount <= 0)
                {
                    break;
                }
            }
        }

        parent.Install(position, Quaternion.identity);

    }

}
