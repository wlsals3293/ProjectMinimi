using UnityEngine;

[CreateAssetMenu(fileName = "NewMountObjectConfig", menuName = "Config/MountObjectConfig", order = 1)]
public class MountObjectConfig : ScriptableObject
{
    [Tooltip("목표 위치까지 도달하는 시간. 값이 클수록 느리게 이동.")]
    public float mountSmoothTime = 0.5f;

    [Tooltip("둥둥 떠 있는 상태로 전환되는 시간.")]
    public float mountBlendTime = 2f;

    [Tooltip("떠 있는 기준 높이.")]
    public float floatingHeight = 1.1f;

    [Tooltip("위아래로 왕복하는 거리.")]
    public float floatingRange = 0.2f;

    [Tooltip("왕복 속도.")]
    public float floatingSpeed = 1f;

    [Tooltip("회전 속도.")]
    public Vector3 rotationSpeed = new Vector3(0f, 20f, 0f);
}
