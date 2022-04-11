using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ProjectMinimi;

public class Scaffold : Activatee
{
    public enum ScaffoldType { Continuous, Temporary }

    public ScaffoldType moveType;

    [SerializeField]
    private float speed = 4.0f;

    [SerializeField]
    private float waitTime = 1.0f;


    private bool goToEnd = true;

    private Vector3 startPosition;

    public Vector3 endPosition;

    private Vector3 targetPosition;

    private Rigidbody rb;


    private Coroutine coroutine;




    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = endPosition;

        if (activateOnStart)
        {
            Activate();
        }
    }

    private void Reset()
    {
        endPosition = transform.position + Vector3.up;
    }

    protected override bool Activate()
    {
        if (!base.Activate())
            return false;

        switch (moveType)
        {
            case ScaffoldType.Continuous:
                coroutine = StartCoroutine(OnReciprocateMoving());
                break;

            case ScaffoldType.Temporary:
                goToEnd = true;
                targetPosition = endPosition;
                coroutine = StartCoroutine(OnReturnMoving());
                break;

            default:
                break;
        }

        return true;
    }

    protected override bool Deactivate()
    {
        if (!base.Deactivate())
            return false;

        switch (moveType)
        {
            case ScaffoldType.Continuous:
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
                break;

            case ScaffoldType.Temporary:
                goToEnd = false;
                targetPosition = startPosition;
                if (coroutine == null)
                {
                    coroutine = StartCoroutine(OnReturnMoving());
                }
                break;

            default:
                break;
        }

        return true;
    }

    private IEnumerator OnReciprocateMoving()
    {
        var delay = new WaitForFixedUpdate();
        var wait = new WaitForSeconds(waitTime);

        while (true)
        {
            Vector3 movePosition = Vector3.MoveTowards(
                rb.position, targetPosition, speed * Time.deltaTime);
            rb.MovePosition(movePosition);

            float distanceSqr = (targetPosition - movePosition).sqrMagnitude;
            if (distanceSqr < 0.0001f)
            {
                // 시작점과 도착점 반전
                goToEnd = !goToEnd;
                targetPosition = goToEnd ? endPosition : startPosition;

                if (waitTime > 0.01f)
                {
                    yield return wait;
                }
            }

            yield return delay;
        }
    }

    private IEnumerator OnReturnMoving()
    {
        var delay = new WaitForFixedUpdate();

        while (true)
        {
            float distanceSqr = (targetPosition - rb.position).sqrMagnitude;

            if (distanceSqr < 0.0001f)
            {
                coroutine = null;
                yield break;
            }

            Vector3 movePosition = Vector3.MoveTowards(
                rb.position, targetPosition, speed * Time.deltaTime);
            rb.MovePosition(movePosition);

            yield return delay;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPos = EditorApplication.isPlaying ? startPosition : transform.position;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPos, endPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPos, 0.3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(endPosition, 0.3f);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 startPos = EditorApplication.isPlaying ? startPosition : transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPosition);
    }
}
