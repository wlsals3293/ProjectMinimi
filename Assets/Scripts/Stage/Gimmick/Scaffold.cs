using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectMinimi;

public class Scaffold : Activatee
{
    public enum ScaffoldType { Continuous, Temporary }

    public ScaffoldType moveType;


    [SerializeField]
    private Transform endPoint;

    [SerializeField]
    private float speed = 4.0f;

    [SerializeField]
    private float waitTime = 1.0f;


    private bool goToEnd = true;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;

    private Rigidbody rb;


    private Coroutine coroutine;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startPosition = rb.position;
        endPosition = endPoint.position;
        targetPosition = endPoint.position;

        if (activateOnStart)
        {
            Activate();
        }
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

                if (waitTime > 0.1f)
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
}
