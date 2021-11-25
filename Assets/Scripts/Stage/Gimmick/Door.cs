using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door : Activatee
{
    [Tooltip("¹® ¼Óµµ")]
    public float doorSpeed = 4.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 targetPosition;


    private Coroutine coroutine;


    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition - new Vector3(0, transform.lossyScale.y, 0);

        if (activateOnStart)
            Activate();
    }


    protected override bool Activate()
    {
        if (!base.Activate())
            return false;

        targetPosition = endPosition;
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
