using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{

    /// <summary>
    /// 스테이지의 체크포인트들이 등록되어 있는 리스트
    /// </summary>
    [Tooltip("체크포인트 목록. 스테이지에서 먼저 나올수록 리스트의 앞에 있게 설정")]
    public List<Checkpoint> checkpoints = new List<Checkpoint>();

    /// <summary>
    /// 플레이어와 적을 포함한 오브젝트들이 자동으로 사망하는 높이
    /// </summary>
    public float globalKillY = 0.0f;

}
