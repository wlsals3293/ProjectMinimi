using UnityEngine;

public class ShoulderCamController : CameraController
{


    [Tooltip("카메라 상하회전 최소, 최대값")]
    public Vector2 pitchMinMax = new Vector2(-70.0f, 89.9f);


    /// <summary>
    /// 카메라의 현재 회전값
    /// </summary>
    private Quaternion currentRotation;



    public Transform Target
    {
        get => virtualCamera.Follow;
        set => virtualCamera.Follow = value;
    }



    /// <summary>
    /// 카메라가 활성화 되었을 때 매 프레임 실행
    /// </summary>
    public override void CameraUpdate()
    {
        ApplyFinalChanges();
    }

    /// <summary>
    /// 이전 카메라의 회전값을 받아 적용합니다
    /// </summary>
    /// <param name="rotation"></param>
    public override void ApplyPreMovement(Quaternion rotation)
    {
        pitch = rotation.eulerAngles.x;
        yaw = rotation.eulerAngles.y;

        if (pitch > 180.0f) pitch -= 360.0f;
        else if (pitch < -180.0f) pitch += 360.0f;
    }

    /// <summary>
    /// 카메라 회전
    /// </summary>
    public override void Rotate(float deltaX, float deltaY)
    {
        yaw += deltaX;
        pitch -= deltaY;

        if (yaw < -180.0f)
            yaw += 360.0f;
        else if (yaw > 180.0f)
            yaw -= 360.0f;

        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }

    /// <summary>
    /// 카메라의 최종 위치와 회전값을 실제 트랜스폼에 적용
    /// </summary>
    protected override void ApplyFinalChanges()
    {
        Target.rotation = currentRotation;
    }
}
