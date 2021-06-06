using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBlocks : MonoBehaviour
{
    [Space]
    [Header("Editor중 자꾸 객체가 변경되면 꺼주세요")]
    [SerializeField] private bool nonUpdateEditor = false;

    [Space]
    [Header("On : 오브젝트 개별 처리, Off : 여기 값만처리, ")]
    [SerializeField] private bool isSingleBlock = false;

    [Space]
    [SerializeField] private SingleBlock prefab_block = null;
    [SerializeField] private Transform blockPivot = null;
    [SerializeField] private List<SingleBlock> blockList = new List<SingleBlock>();

    [Header("Settigns")]
    [SerializeField] private int blockCount = 3;
    [SerializeField] private Vector3 pivotPos = Vector3.zero;

    [Space]
    [SerializeField] private Vector3 offset = Vector3.forward;
    [SerializeField] private Vector3 fallDirection = -Vector3.up;

    [Space]
    [SerializeField] private float minHigh = 0f;
    [SerializeField] private float minHighTime = 1f;
    [Space]
    [SerializeField] private float maxHigh = 5f;
    [SerializeField] private float maxHighTime = 1f;

    [Space]
    [SerializeField] private float blockDirChangeDelay = 0.5f;
    [SerializeField] private float nextBlockDelay = 0.5f;

    public void OnValidate()
    {
#if UNITY_EDITOR
        if (nonUpdateEditor == false)
        {
            if (Application.isPlaying == false)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    if (Application.isPlaying == false)
                    {
                        //DeleteBlocks();
                        CreateBlocks();
                    }
                };

            }
        }
#endif
    }


    private void Awake()
    {
        //DeleteBlocks();
        
        CreateBlocks();

        if (isSingleBlock == false)
        {
            StartCoroutine(DelayStartBlock());
        }
    }


    private IEnumerator DelayStartBlock()
    {
        WaitForSeconds wfs = new WaitForSeconds(nextBlockDelay);

        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i].gameObject.activeSelf)
            {
                blockList[i].SetUse(true);
                yield return wfs;
            }
        }
    }

    public void DeleteBlocks()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (blockList[i] == null)
                continue;

            DestroyImmediate(blockList[i].gameObject, true);
        }

        blockList.Clear();
    }

    public void CreateBlocks()
    {
        Transform parent = blockPivot;
        if (parent == null)
        {
            parent = transform;
        }
        else
        {
            blockPivot.localPosition = pivotPos;
        }

        Vector3 pos = Vector3.zero;
        SingleBlock singleBlock = null;

        int length = Mathf.Max(blockList.Count, blockCount);
        int idx = 1;
        for (int i = 0; i < length; i++)
        {
            if (i >= blockCount)
            {
                blockList[i].gameObject.SetActive(false);
                continue;
            }
            
            if(blockList.Count < blockCount)
            {
                singleBlock = GameObject.Instantiate(prefab_block.gameObject, parent).GetComponent<SingleBlock>();
                blockList.Add(singleBlock);
            }

            blockList[i].gameObject.SetActive(true);
            singleBlock = blockList[i];

            singleBlock.Init(
                       fallDirection
                       , minHigh, minHighTime
                       , maxHigh, maxHighTime
                       , blockDirChangeDelay
                       , nextBlockDelay * idx
                       , isSingleBlock                       
                   );

            idx++;
            singleBlock.SetLocalPosition(pos);
            pos += offset;
        }

    }
}
