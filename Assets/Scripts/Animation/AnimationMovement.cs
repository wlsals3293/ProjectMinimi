using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovement
{
    public bool active = false;

    private float movementTime;
    private float elapsedTime;
    private Vector3 moveVector;
    private Vector3 moveVectorPre;


    private Rigidbody rigidbody;
    private AnimCurve3 curves;


    public AnimationMovement(Rigidbody inRigidBody)
    {
        rigidbody = inRigidBody;
    }

    public void StartMovement(Vector3 startPosition, Vector3 targetPosition, float animationTime, AnimCurve3 inCurve)
    {
        movementTime = animationTime;
        elapsedTime = 0f;
        curves = inCurve;

        moveVector = targetPosition - startPosition;
        moveVectorPre = Vector3.zero;

        active = true;
    }

    public bool UpdatePosition()
    {
        if (elapsedTime >= movementTime)
        {
            active = false;
            return true;
        }

        elapsedTime += Time.deltaTime;

        float t = elapsedTime / movementTime;

        float forwardT = curves.z.Evaluate(t);
        float upT = curves.y.Evaluate(t);


        Vector3 next = moveVector * forwardT;
        next.y = moveVector.y * upT;

        Vector3 newPos = rigidbody.position;
        newPos += next - moveVectorPre;
        

        rigidbody.MovePosition(newPos);


        moveVectorPre = next;

        return false;
    }
}
