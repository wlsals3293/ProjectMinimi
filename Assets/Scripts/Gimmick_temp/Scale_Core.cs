using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Core : MonoBehaviour
{
    public Scale_Side Side1;
    public Scale_Side Side2;

    public Transform highPoint;
    public Transform lowPoint;
    public Vector3 middlePoint;

    public float middleDistance;
    public float moveDistance;
    public float weightProportion;

    public Scale_Side HeavySide;
    public Scale_Side LightSide;
    public Rigidbody HeavySide_rgd;
    public Rigidbody LightSide_rgd;

    public Vector3 HeavySide_Target;
    public Vector3 LightSide_Target;

    public float UpdownSpeed;
    
    // Start is called before the first frame update
    void Start()
    {   
        HeavySide = Side1;
        HeavySide_rgd = Side1.gameObject.GetComponent<Rigidbody>();
        LightSide = Side2;
        LightSide_rgd = Side2.gameObject.GetComponent<Rigidbody>();
        
        middleDistance = (highPoint.position.y - lowPoint.position.y) / 2;

        Side1.transform.position = new Vector3(Side1.transform.position.x, highPoint.position.y - middleDistance, Side1.transform.position.z);
        Side2.transform.position = new Vector3(Side2.transform.position.x, highPoint.position.y - middleDistance, Side2.transform.position.z);

        middlePoint = new Vector3(highPoint.position.x, highPoint.position.y - middleDistance, highPoint.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (HeavySide.TotalWeight == LightSide.TotalWeight)
            
        {
            weightProportion = 0;
        }

        else if(LightSide.TotalWeight * 2 <= HeavySide.TotalWeight)
        {
            weightProportion = 1;
        }

        else  weightProportion = (HeavySide.TotalWeight % LightSide.TotalWeight) / LightSide.TotalWeight;

        if (Side1.TotalWeight > Side2.TotalWeight)
        {
            HeavySide = Side1;
            HeavySide_rgd = Side1.gameObject.GetComponent<Rigidbody>();
            LightSide = Side2;
            LightSide_rgd = Side2.gameObject.GetComponent<Rigidbody>();
        }

        else
        {
            HeavySide = Side2;
            HeavySide_rgd = Side2.gameObject.GetComponent<Rigidbody>();
            LightSide = Side1;
            LightSide_rgd = Side1.gameObject.GetComponent<Rigidbody>();
        }

        moveDistance = middleDistance * weightProportion;

        HeavySide_Target = new Vector3(HeavySide.transform.position.x, middlePoint.y - moveDistance, HeavySide.transform.position.z);
        LightSide_Target = new Vector3(LightSide.transform.position.x, middlePoint.y + moveDistance, LightSide.transform.position.z);

        

        
    }

    private void FixedUpdate()
    {
        Vector3 HeavyDirection = HeavySide_Target - HeavySide.transform.position;
        Vector3 LightDirection = LightSide_Target - LightSide.transform.position;

        if (Vector3.Distance(HeavySide.transform.position, HeavySide_Target) > 0.1f)
        {
            HeavySide_rgd.MovePosition(HeavySide.transform.position + HeavyDirection.normalized * UpdownSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(LightSide.transform.position, LightSide_Target) > 0.1f)
        {
            LightSide_rgd.MovePosition(LightSide.transform.position + LightDirection.normalized * UpdownSpeed * Time.deltaTime);
        }
    }
}
