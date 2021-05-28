using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinimiManager : MonoBehaviour
{
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
    private int onHandMinimiCount = 0;


    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }


    public void RegisterMinimi(Minimi minimi)
    {
        if (minimi == null)
            return;

        if(minimi is BlockMinimi)
        {
            blockMinimis.Add(minimi as BlockMinimi);
        }
        else if(minimi is FireMinimi)
        {
            fireMinimis.Add(minimi as FireMinimi);
        }
        else if(minimi is WindMinimi)
        {
            windMinimis.Add(minimi as WindMinimi);
        }
    }

    /// <summary>
    /// ���濡�� �̴Ϲ̸� ����
    /// </summary>
    /// <param name="minimiType">���� �̴Ϲ��� ����</param>
    public void TakeOutMinimi(MinimiType minimiType)
    {
        if(onHandMinimiCount > 0)
        {

        }
    }

    /// <summary>
    /// ���� �տ� ��� �ִ� �̴Ϲ̸� �ٽ� ���濡 �������
    /// </summary>
    public void TakeInMinimis()
    {

    }
}
