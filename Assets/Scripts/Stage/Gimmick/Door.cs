using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectMinimi;


public class Door : Activatee
{
    [Tooltip("문 속도")]
    [SerializeField]
    private float doorSpeed = 4.0f;

    [Tooltip("이동 거리")]
    [SerializeField]
    private float moveDistance = 10f;

    private Vector3 startPosition;
    private Vector3 targetPosition;


    private Coroutine coroutine;


    private void Start()
    {
        startPosition = transform.position;

        if (activateOnStart)
            Activate();
    }


    protected override bool Activate()
    {
        if (!base.Activate())
            return false;

        targetPosition = startPosition + Vector3.down * moveDistance;
        if (coroutine == null)
        {
            coroutine = StartCoroutine(OnMove());
        }

        return true;
    }

    protected override bool Deactivate()
    {
        if (!base.Deactivate())
            return false;

        targetPosition = startPosition;
        if (coroutine == null)
        {
            coroutine = StartCoroutine(OnMove());
        }

        return true;
    }

    private IEnumerator OnMove()
    {
        while (true)
        {
            float distanceSqr = (targetPosition - transform.position).sqrMagnitude;

            if (distanceSqr < 0.0001f)
            {
                coroutine = null;
                yield break;
            }

            transform.position =
                Vector3.MoveTowards(transform.position, targetPosition, doorSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
