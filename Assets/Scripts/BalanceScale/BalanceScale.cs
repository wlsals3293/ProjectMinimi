using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceScale : MonoBehaviour
{

    [Space]
    [Header("Default")]
    [SerializeField] private float maxMass = 5f;
    [Space]
    [SerializeField] private float moveHigh = 5f;
    [SerializeField] private float moveSpeed = 0.5f;

    [Space]
    [Header("Left")]
    [SerializeField] private Transform leftMoveTarget = null;
    [SerializeField] private float leftDefaultMass = 0f;

    [Space]
    [Header("Right")]
    [SerializeField] private Transform rightMoveTarget = null;
    [SerializeField] private float rightDefaultMass = 0f;


    private Dictionary<int, Minimi> leftMinimiDic = new Dictionary<int, Minimi>();
    private Dictionary<int, Minimi> rightMinimiDic = new Dictionary<int, Minimi>();

    private float prevLeftMass = 0f;
    private float prevRightMass = 0f;

    private float halfHigh = 0f;
    private float highPerMass = 0f;
    private float timer = 0f;

    private void Awake()
    {
        Init();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        Init();
#endif
    }

    private void Init()
    {
        halfHigh = moveHigh * 0.5f;
        highPerMass = moveHigh / maxMass;

        UpdateMass(true);
    }

    public void AddMinimi(Collider minimi, bool left)
    {
        if (minimi == null)
            return;

        int key = minimi.GetHashCode();
        Dictionary<int, Minimi> dic = null;
        if (left)
            dic = leftMinimiDic;
        else
            dic = rightMinimiDic;

        if (dic.ContainsKey(key) == false)
        {
            Minimi item = minimi.GetComponent<Minimi>();
            if(item != null)
                dic.Add(key, item);
        }
    }

    private void Update()
    {
        UpdateMass();
    }

    private void UpdateMass(bool quick = false)
    {
        float curLeftMass = CalcTotalMass(leftMinimiDic, leftDefaultMass);
        float curRightMass = CalcTotalMass(rightMinimiDic, rightDefaultMass);

        //if (prevLeftMass != curLeftMass || prevRightMass != curRightMass)
        //{
        //    prevLeftMass = curLeftMass;
        //    prevRightMass = curRightMass;
        //}

        if (prevLeftMass == curLeftMass && prevRightMass == curRightMass)
        {
            timer = 0f;
            return;
        }

        bool leftDown = false;
        if (curLeftMass > curRightMass)         // Left Down
        {
            leftDown = true;
        }
        else if (curLeftMass < curRightMass)    // Left Up
        {
            leftDown = false;
        }

        float mass = Mathf.Abs(curLeftMass - curRightMass);
        mass = Mathf.Min(mass, maxMass);
        Transform lowT, highT;

        if (leftDown)
        {
            lowT = leftMoveTarget;
            highT = rightMoveTarget;
        }
        else
        {
            lowT = rightMoveTarget;
            highT = leftMoveTarget;
        }

        float lowY = 0f;
        float highY = 0f;

        if (quick == true)
            timer = 1f;

        lowY = Mathf.Lerp(lowT.localPosition.y, -(mass * highPerMass), timer);
        lowT.localPosition = GetChangeLocalPosY(lowT, lowY);

        highY = Mathf.Lerp(highT.localPosition.y, mass * highPerMass, timer);
        highT.localPosition = GetChangeLocalPosY(highT, highY);

        // 그냥하면 mass == 0 일때 infinity 에러
        timer += moveSpeed * Time.deltaTime / (mass + 1) * 0.01f;

        if (timer >= 1f)
        {
            prevLeftMass = curLeftMass;
            prevRightMass = curRightMass;

            lowT.localPosition = GetChangeLocalPosY(lowT, (-highPerMass * mass));
            highT.localPosition = GetChangeLocalPosY(highT, (highPerMass * mass));

            timer = 0f;
        }
    }

    private Vector3 GetChangeLocalPosY(Transform trans, float y)
    {
        return new Vector3(trans.localPosition.x, y, trans.localPosition.z);
    }

    private float CalcTotalMass(Dictionary<int, Minimi> dic, float defaultMass)
    {
        float totalMass = defaultMass;

        foreach (var item in dic.Values)
        {
            if(item.Type == MinimiType.Block)
            {
                totalMass += item.Mass;
            }
        }

        return totalMass;
    }

}
