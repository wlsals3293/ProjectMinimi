using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwallowableObject
{
    /// <summary>
    /// 발사할 때 지속여부를 반환합니다. ex) 물
    /// </summary>
    /// <returns>지속여부</returns>
    public bool IsContinuous();

    /// <summary>
    /// 오브젝트를 뱉을 때 동작
    /// </summary>
    /// <param name="startPosition">시작 위치</param>
    /// <param name="spitVelocity">방향과 속도</param>
    public void Spit(Vector3 startPosition, Vector3 spitVelocity);

    /// <summary>
    /// 오브젝트를 삼킬 때 동작
    /// </summary>
    public void Swallow();
}
