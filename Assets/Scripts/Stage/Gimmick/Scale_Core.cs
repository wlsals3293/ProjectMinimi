using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Core : MonoBehaviour
{
    public Scale_Side side1;
    public Scale_Side side2;

    public Transform highPoint;
    public Transform lowPoint;
    private Vector3 middlePoint;

    private float middleDistance;
    private float moveDistance;
    private float weightProportion;

    private Scale_Side heavySide;
    private Scale_Side lightSide;
    private Rigidbody heavySide_rgd;
    private Rigidbody lightSide_rgd;

    private Vector3 heavySide_Target;
    private Vector3 lightSide_Target;

    public float updownSpeed;
    
    // Start is called before the first frame update
    void Start()
    {   
        heavySide = side1;
        heavySide_rgd = side1.gameObject.GetComponent<Rigidbody>();
        lightSide = side2;
        lightSide_rgd = side2.gameObject.GetComponent<Rigidbody>();
        
        middleDistance = (highPoint.position.y - lowPoint.position.y) / 2;

        side1.transform.position = new Vector3(side1.transform.position.x, highPoint.position.y - middleDistance, side1.transform.position.z);
        side2.transform.position = new Vector3(side2.transform.position.x, highPoint.position.y - middleDistance, side2.transform.position.z);

        middlePoint = new Vector3(highPoint.position.x, highPoint.position.y - middleDistance, highPoint.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (heavySide.totalWeight == lightSide.totalWeight)

        {
            weightProportion = 0;
        }

        else if (lightSide.totalWeight * 2 <= heavySide.totalWeight)
        {
            weightProportion = 1;
        }

        else weightProportion = (heavySide.totalWeight % lightSide.totalWeight) / lightSide.totalWeight;

        if (side1.totalWeight > side2.totalWeight)
        {
            heavySide = side1;
            heavySide_rgd = side1.gameObject.GetComponent<Rigidbody>();
            lightSide = side2;
            lightSide_rgd = side2.gameObject.GetComponent<Rigidbody>();
        }

        else
        {
            heavySide = side2;
            heavySide_rgd = side2.gameObject.GetComponent<Rigidbody>();
            lightSide = side1;
            lightSide_rgd = side1.gameObject.GetComponent<Rigidbody>();
        }

        moveDistance = middleDistance * weightProportion;

        heavySide_Target = new Vector3(heavySide.transform.position.x, middlePoint.y - moveDistance, heavySide.transform.position.z);
        lightSide_Target = new Vector3(lightSide.transform.position.x, middlePoint.y + moveDistance, lightSide.transform.position.z);

    }

    private void FixedUpdate()
    {
        Vector3 HeavyDirection = heavySide_Target - heavySide.transform.position;
        Vector3 LightDirection = lightSide_Target - lightSide.transform.position;

        if (Vector3.Distance(heavySide.transform.position, heavySide_Target) > 0.1f)
        {
            heavySide_rgd.MovePosition(heavySide.transform.position + HeavyDirection.normalized * updownSpeed * Time.deltaTime);
            lightSide_rgd.MovePosition(lightSide.transform.position + LightDirection.normalized * updownSpeed * Time.deltaTime);
        }
    }
}
