using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold_Ctrl : Activatee
{
    public enum ScaffoldType { Reciprocate, Return }

    public ScaffoldType moveType;
    public Transform scaffold_trfm;
    public Rigidbody scaffold_rgd;
    public Transform start;
    public Transform end;

    public float speed = 4.0f;
    public float waitTime = 1.0f;

    private bool goToEnd = true;

    private Vector3 targetPosition;


    private Coroutine coroutine;


    private void Start()
    {
        scaffold_trfm.position = start.position; //씬 시작 시 시작점으로 순간이동
        targetPosition = end.position;

        if (ActivateOnStart)
        {
            Activate();
        }
    }

    protected override void Activate()
    {
        base.Activate();

        switch (moveType)
        {
            case ScaffoldType.Reciprocate:
                coroutine = StartCoroutine(OnReciprocateMoving());
                break;

            case ScaffoldType.Return:
                goToEnd = true;
                targetPosition = end.position;
                coroutine = StartCoroutine(OnReturnMoving());
                break;

            default:
                break;
        }
    }

    protected override void Deactivate()
    {
        base.Deactivate();

        switch (moveType)
        {
            case ScaffoldType.Reciprocate:
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
                break;

            case ScaffoldType.Return:
                goToEnd = false;
                targetPosition = start.position;
                if (coroutine == null)
                {
                    coroutine = StartCoroutine(OnReturnMoving());
                }
                break;

            default:
                break;
        }
    }

    private IEnumerator OnReciprocateMoving()
    {
        var delay = new WaitForFixedUpdate();
        var wait = new WaitForSeconds(waitTime);

        while (true)
        {
            Vector3 movePosition = Vector3.MoveTowards(
                scaffold_rgd.position, targetPosition, speed * Time.deltaTime);
            scaffold_rgd.MovePosition(movePosition);

            float distanceSqr = (targetPosition - movePosition).sqrMagnitude;
            if (distanceSqr < 0.0001f)
            {
                // 시작점과 도착점 반전
                goToEnd = !goToEnd;
                targetPosition = goToEnd ? end.position : start.position;

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
            float distanceSqr = (targetPosition - scaffold_rgd.position).sqrMagnitude;

            if (distanceSqr < 0.0001f)
            {
                coroutine = null;
                yield break;
            }

            Vector3 movePosition = Vector3.MoveTowards(
                scaffold_rgd.position, targetPosition, speed * Time.deltaTime);
            scaffold_rgd.MovePosition(movePosition);

            yield return delay;
        }
    }
}
