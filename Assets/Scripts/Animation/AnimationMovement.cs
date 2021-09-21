using UnityEngine;

/// <summary>
/// �ִϸ��̼ǵ��� ĳ���Ͱ� �ִϸ��̼� Ŀ�긦 ���� �̵��ϴ� ����� ����ϴ� Ŭ����.
/// ��Ʈ��� ��� ���̸� ��Ȳ�� ���� �������� ó���� �� �ְ� Ȯ��� �� ����
/// </summary>
public class AnimationMovement
{
    /// <summary>
    /// �ִϸ��̼� �̵��� Ȱ��ȭ ����
    /// </summary>
    private bool active = false;

    /// <summary>
    /// �̵��� �Ϸ�� ������ �ɸ��� �ð�
    /// </summary>
    private float movementTime;

    /// <summary>
    /// ��� �ð�
    /// </summary>
    private float elapsedTime;

    /// <summary>
    /// ������������ ���������� ���ϴ� ����
    /// </summary>
    private Vector3 moveVector;

    /// <summary>
    /// ���� �����ӿ��� ���� �̵� ����
    /// </summary>
    private Vector3 moveVectorPre;


    /// <summary>
    /// �̵��� ��ü�� ������ �ٵ�
    /// </summary>
    private Rigidbody rigidbody;

    /// <summary>
    /// �󸶳� �̵��ϴ��� ������ ��� Ŀ�� 3��. (x, y, z�� ����)
    /// </summary>
    private AnimCurve3 curves;


    public bool IsActive { get => active; }


    public AnimationMovement(Rigidbody inRigidBody)
    {
        rigidbody = inRigidBody;
    }

    /// <summary>
    /// �ִϸ��̼� �̵��� �����մϴ�.
    /// </summary>
    /// <param name="startPosition">���� ��ġ</param>
    /// <param name="targetPosition">��ǥ ��ġ</param>
    /// <param name="animationTime">�̵��� �Ϸ�� ������ �ɸ��� �ð�</param>
    /// <param name="inCurve">�ִϸ��̼� �̵�Ŀ��</param>
    public void StartMovement(Vector3 startPosition, Vector3 targetPosition, float animationTime, AnimCurve3 inCurve)
    {
        movementTime = animationTime;
        elapsedTime = 0f;
        curves = inCurve;

        moveVector = rigidbody.transform.InverseTransformDirection(targetPosition - startPosition);
        moveVectorPre = Vector3.zero;

        active = true;
    }

    /// <summary>
    /// �ִϸ��̼� �̵��� Ȱ��ȭ�Ǿ��� �� ���� ��ŭ ��ü�� �̵���ŵ�ϴ�.
    /// </summary>
    /// <returns>�̵� �Ϸ� ����</returns>
    public bool UpdatePosition()
    {
        bool complete = false;
        float t;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= movementTime)
        {
            complete = true;
            t = 1f;
        }
        else
            t = elapsedTime / movementTime;


        float forwardT = curves.z.Evaluate(t);
        float upT = curves.y.Evaluate(t);
        float rightT = curves.x.Evaluate(t);

        Vector3 next;
        next.z = moveVector.z * forwardT;
        next.y = moveVector.y * upT;
        next.x = moveVector.x * rightT;

        Vector3 newPos = rigidbody.position;
        newPos += rigidbody.transform.TransformDirection(next - moveVectorPre);
        rigidbody.MovePosition(newPos);

        moveVectorPre = next;


        if (complete)
        {
            active = false;
            return true;
        }
        else
            return false;
    }
}
