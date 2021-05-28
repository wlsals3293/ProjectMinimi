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
    /// 현재 설치하려고 준비중인 미니미의 종류
    /// </summary>
    private MinimiType onHandMinimiType = MinimiType.None;

    /// <summary>
    /// 현재 설치하려고 준비중인 미니미의 개수
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
    /// 가방에서 미니미를 꺼냄
    /// </summary>
    /// <param name="minimiType">꺼낸 미니미의 종류</param>
    public void TakeOutMinimi(MinimiType minimiType)
    {
        if(onHandMinimiCount > 0)
        {

        }
    }

    /// <summary>
    /// 현재 손에 들고 있는 미니미를 다시 가방에 집어넣음
    /// </summary>
    public void TakeInMinimis()
    {

    }
}
