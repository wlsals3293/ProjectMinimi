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

    private float highPerMass = 0f;
    private float timer = 0f;

    private float startLeftY = 0f;
    private float startRightY = 0f;

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

    public void RemoveMinimi(Collider minimi, bool left)
    {
        if (minimi == null)
            return;

        int key = minimi.GetHashCode();
        Dictionary<int, Minimi> dic = null;
        if (left)
            dic = leftMinimiDic;
        else
            dic = rightMinimiDic;

        if (dic.ContainsKey(key))
        {
            dic.Remove(key);
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

        if (prevLeftMass != curLeftMass || prevRightMass != curRightMass)
        {
            prevLeftMass = curLeftMass;
            prevRightMass = curRightMass;

            startLeftY = leftMoveTarget.localPosition.y;
            startRightY = rightMoveTarget.localPosition.y;

            timer = 0f;
        }
        else
        {
            if (timer >= 1f)
            {
                return;
            }
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
        Dictionary<int, Minimi> lowDic, highDic;

        float lowY, highY = 0f;

        if (leftDown)
        {
            lowT = leftMoveTarget;
            highT = rightMoveTarget;

            lowY = startLeftY;
            highY = startRightY;

            lowDic = leftMinimiDic;
            highDic = rightMinimiDic;
        }
        else
        {
            lowT = rightMoveTarget;
            highT = leftMoveTarget;

            lowY = startRightY;
            highY = startLeftY;

            lowDic = rightMinimiDic;
            highDic = leftMinimiDic;
        }

        if (quick == true)
            timer = 1f;

        timer += Time.deltaTime * moveSpeed;

        float y = Mathf.Lerp(lowY, -(mass * highPerMass), timer);
        float ry = lowT.localPosition.y - y;

        lowT.localPosition = GetChangeLocalPosY(lowT, y);
        MoveMinimi(lowDic, -ry);

        y = Mathf.Lerp(highY, mass * highPerMass, timer);
        ry = highT.localPosition.y - y;

        highT.localPosition = GetChangeLocalPosY(highT, y);
        MoveMinimi(highDic, ry);
    }

    private Vector3 GetChangeLocalPosY(Transform trans, float y)
    {
        return new Vector3(trans.localPosition.x, y, trans.localPosition.z);
    }

    private void MoveMinimi(Dictionary<int, Minimi> dic, float y)
    {
        foreach (var item in dic.Values)
        {
            if (item.Type == MinimiType.Block)
            {
                if (item.gameObject.activeInHierarchy)
                {
                    item.transform.localPosition 
                        = GetChangeLocalPosY(item.transform, item.transform.localPosition.y + y);
                }               
            }
        }

    }

    private float CalcTotalMass(Dictionary<int, Minimi> dic, float defaultMass)
    {
        float totalMass = defaultMass;

        List<int> removeKeys = new List<int>();


        foreach (var item in dic)
        {
            if (item.Value.gameObject.activeInHierarchy)
            {
                if (item.Value.Type == MinimiType.Block)
                {
                    totalMass += item.Value.Mass;
                }
            }
            else
            {
                removeKeys.Add(item.Key);
            }
        }

        for (int i = 0; i < removeKeys.Count; i++)
        {
            dic.Remove(removeKeys[i]);
        }

        return totalMass;
    }

}
