using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwallowableObject
{
    /// <summary>
    /// �߻��� �� ���ӿ��θ� ��ȯ�մϴ�. ex) ��
    /// </summary>
    /// <returns>���ӿ���</returns>
    public bool IsContinuous();

    /// <summary>
    /// ������Ʈ�� ���� �� ����
    /// </summary>
    /// <param name="startPosition">���� ��ġ</param>
    /// <param name="spitVelocity">����� �ӵ�</param>
    public void Spit(Vector3 startPosition, Vector3 spitVelocity);

    /// <summary>
    /// ������Ʈ�� ��ų �� ����
    /// </summary>
    public void Swallow();
}
