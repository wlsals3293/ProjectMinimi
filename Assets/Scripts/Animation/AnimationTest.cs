using UnityEngine;

/// <summary>
/// 애니메이션 커브 수정용 테스트 객체. 런타임에는 포함 안될것
/// </summary>
public class AnimationTest : MonoBehaviour
{

    public float currentRatio;


    public bool pause;


    public Rigidbody rb;
    public Animator animator;


    public Vector3 startPosition;

    public Vector3 endPosition;

    public AnimCurve3 curves;


    public float movementTime = 1.53f;
    
    public float elapsedTime;

    private float savedTimeScale = 1f;


    private void Awake()
    {
        animator.SetBool("LedgeGrab", true);
        animator.SetBool("LedgeGrabUp", true);
    }

    private void Update()
    {
        if(!pause)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime > movementTime)
            {
                elapsedTime = 0f;
                animator.Play("Girl_hangingUp", 0, 0f);
            }

            currentRatio = elapsedTime / movementTime;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            pause = !pause;

            if (pause)
            {
                savedTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = savedTimeScale;
                animator.Play("Girl_hangingUp", 0, currentRatio);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            float newSpeed = Mathf.Max(0f, Time.timeScale - 0.1f);
            Time.timeScale = newSpeed;
            if (Time.timeScale <= 0f)
                pause = true;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            float newSpeed = Mathf.Min(1f, Time.timeScale + 0.1f);
            Time.timeScale = newSpeed;
            if (pause)
                pause = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentRatio = 0;
            rb.MovePosition(startPosition);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentRatio -= 0.01f;
            if (currentRatio < 0f)
                currentRatio = 0f;
            SetPositionAndAnimation(currentRatio);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentRatio += 0.01f;
            if (currentRatio > 1f)
                currentRatio = 1f;
            SetPositionAndAnimation(currentRatio);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            currentRatio = 1f;
            rb.MovePosition(startPosition);
            return;
        }

        if (pause)
            SetPositionAndAnimation(currentRatio);
        else
            SetPosition(currentRatio);

    }

    private void SetPositionAndAnimation(float t)
    {
        animator.Play("Girl_hangingUp", 0, t);


        float forwardT = curves.z.Evaluate(t);
        float upT = curves.y.Evaluate(t);

        Vector3 newPos = Vector3.Lerp(startPosition, endPosition, forwardT);
        newPos.y = Mathf.Lerp(startPosition.y, endPosition.y, upT);


        rb.MovePosition(newPos);
    }

    private void SetPosition(float t)
    {
        float forwardT = curves.z.Evaluate(t);
        float upT = curves.y.Evaluate(t);

        Vector3 newPos = Vector3.Lerp(startPosition, endPosition, forwardT);
        newPos.y = Mathf.Lerp(startPosition.y, endPosition.y, upT);


        rb.MovePosition(newPos);
    }
}
