using UnityEngine;

/// <summary>
/// 애니메이션도중 캐릭터가 애니메이션 커브를 따라 이동하는 기능을 담당하는 클래스.
/// 루트모션 대신 쓰이며 상황에 따라 동적으로 처리할 수 있게 확장될 수 있음
/// </summary>
public class AnimationMovement
{
    /// <summary>
    /// 애니메이션 이동의 활성화 여부
    /// </summary>
    private bool active = false;

    /// <summary>
    /// 이동이 완료될 때까지 걸리는 시간
    /// </summary>
    private float movementTime;

    /// <summary>
    /// 경과 시간
    /// </summary>
    private float elapsedTime;

    /// <summary>
    /// 시작지점에서 끝지점으로 향하는 벡터
    /// </summary>
    private Vector3 moveVector;

    /// <summary>
    /// 이전 프레임에서 계산된 이동 벡터
    /// </summary>
    private Vector3 moveVectorPre;


    /// <summary>
    /// 이동할 객체의 리지드 바디
    /// </summary>
    private Rigidbody rigidbody;

    /// <summary>
    /// 얼마나 이동하는지 정보가 담긴 커브 3개. (x, y, z축 각각)
    /// </summary>
    private AnimCurve3 curves;


    public bool IsActive { get => active; }


    public AnimationMovement(Rigidbody inRigidBody)
    {
        rigidbody = inRigidBody;
    }

    /// <summary>
    /// 애니메이션 이동을 시작합니다.
    /// </summary>
    /// <param name="startPosition">시작 위치</param>
    /// <param name="targetPosition">목표 위치</param>
    /// <param name="animationTime">이동이 완료될 때까지 걸리는 시간</param>
    /// <param name="inCurve">애니메이션 이동커브</param>
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
    /// 애니메이션 이동이 활성화되었을 때 계산된 만큼 객체를 이동시킵니다.
    /// </summary>
    /// <returns>이동 완료 여부</returns>
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
