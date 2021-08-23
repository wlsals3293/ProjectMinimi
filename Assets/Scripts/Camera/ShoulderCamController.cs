using UnityEngine;

public class ShoulderCamController : CameraController
{


    [Tooltip("ī�޶� ����ȸ�� �ּ�, �ִ밪")]
    public Vector2 pitchMinMax = new Vector2(-70.0f, 89.9f);


    /// <summary>
    /// ī�޶��� ���� ȸ����
    /// </summary>
    private Quaternion currentRotation;



    public Transform Target
    {
        get => virtualCamera.Follow;
        set => virtualCamera.Follow = value;
    }



    /// <summary>
    /// ī�޶� Ȱ��ȭ �Ǿ��� �� �� ������ ����
    /// </summary>
    public override void CameraUpdate()
    {
        ApplyFinalChanges();
    }

    /// <summary>
    /// ���� ī�޶��� ȸ������ �޾� �����մϴ�
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
    /// ī�޶� ȸ��
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
    /// ī�޶��� ���� ��ġ�� ȸ������ ���� Ʈ�������� ����
    /// </summary>
    protected override void ApplyFinalChanges()
    {
        Target.rotation = currentRotation;
    }
}
