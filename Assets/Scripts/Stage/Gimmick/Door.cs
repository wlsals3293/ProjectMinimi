using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door : Activatee
{
    public float doorSpd = 4.0f;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 targetPosition;


    private Coroutine coroutine;


    private void Start()
    {
        startPoint = transform.position;
        endPoint = startPoint - new Vector3(0, transform.lossyScale.y, 0);

        if (ActivateOnStart)
            Activate();
    }


    protected override void Activate()
    {
        base.Activate();

        targetPosition = endPoint;
        if (coroutine == null)
        {
            coroutine = StartCoroutine(OnMove());
        }
    }

    protected override void Deactivate()
    {
        base.Deactivate();

        targetPosition = startPoint;
        if (coroutine == null)
        {
            coroutine = StartCoroutine(OnMove());
        }
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
                Vector3.MoveTowards(transform.position, targetPosition, doorSpd * Time.deltaTime);

            yield return null;
        }
    }
}
