using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{

    /// <summary>
    /// ���������� üũ����Ʈ���� ��ϵǾ� �ִ� ����Ʈ
    /// </summary>
    [Tooltip("üũ����Ʈ ���. ������������ ���� ���ü��� ����Ʈ�� �տ� �ְ� ����")]
    public List<Checkpoint> checkpoints = new List<Checkpoint>();

    /// <summary>
    /// �÷��̾�� ���� ������ ������Ʈ���� �ڵ����� ����ϴ� ����
    /// </summary>
    public float globalKillY = 0.0f;

}
