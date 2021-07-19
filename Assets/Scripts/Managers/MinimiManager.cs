using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinimiManager : BaseManager<MinimiManager>
{
    /// <summary>
    /// 설치 높이 오차 허용값
    /// </summary>
    private const float INSTALL_HEIGHT_TOLERANCE = 2.0f;

    /// <summary>
    /// 한 번에 동시설치되는 최대 개수
    /// </summary>
    public const int MAX_STACK_COUNT = 3;

    /// <summary>
    /// 캐릭터로 부터 미니미가 설치되는 위치
    /// </summary>
    private static readonly Vector3 INSTALL_OFFSET = new Vector3(0.0f, 0.0f, 2.0f);

    /// <summary>
    /// 미니미 설치가 가능할 때 색상
    /// </summary>
    private static readonly Color COLOR_VALID = new Color(0.4f, 0.67f, 0.97f, 0.5f);

    /// <summary>
    /// 미니미 설치가 불가능할 때 색상
    /// </summary>
    private static readonly Color COLOR_INVALID = new Color(1.0f, 0.0f, 0.0f, 0.5f);



    public bool IsEmpty { get => onHandMinimiList.Count == 0; }

    /// <summary>
    /// 미니미가 합쳐지는 거리. 이 거리 안에 미니미를 설치하면 합쳐짐
    /// </summary>
    [SerializeField] private float mergeDistance = 2.0f;

    /// <summary>
    /// 맵상에 있는 모든 미니미의 리스트
    /// </summary>
    private Dictionary<MinimiType, List<Minimi>> allMinimiLists = new Dictionary<MinimiType, List<Minimi>>();
    
    /// <summary>
    /// 플레이어가 소유하고 있는 미니미의 리스트
    /// </summary>
    private Dictionary<MinimiType, List<Minimi>> ownMinimiLists = new Dictionary<MinimiType, List<Minimi>>();

    /// <summary>
    /// 손에 들고 있는 미니미의 리스트
    /// </summary>
    private List<Minimi> onHandMinimiList = new List<Minimi>();

    /// <summary>
    /// 현재 설치하려고 준비중인 미니미의 종류
    /// </summary>
    private MinimiType onHandMinimiType = MinimiType.None;


    [SerializeField] private GameObject boxMinimiRef = null;

    private bool blueprintActive = false;
    private bool wasValid = false;
    private GameObject[] blueprintObject = new GameObject[MAX_STACK_COUNT];
    private Renderer[] blueprintObjectRenderers = new Renderer[MAX_STACK_COUNT];

    [HideInInspector] public Transform playerTrans = null;



    protected override void Awake()
    {
        base.Awake();

        for(int i=1; i<(int)MinimiType.Max; i++)
        {
            allMinimiLists.Add((MinimiType)i, new List<Minimi>());
            ownMinimiLists.Add((MinimiType)i, new List<Minimi>());
        }
    }

    public void Initialize()
    {
        playerTrans = PlayerManager.Instance.PlayerCtrl.transform;

        // 리스트 초기화       
        for(int i=1; i<(int)MinimiType.Max; i++)
        {
            ownMinimiLists[(MinimiType)i].Clear();

            /*foreach(var minimi in allMinimiLists[(MinimiType)i])
            {
                Destroy(minimi.gameObject);
            }*/
            allMinimiLists[(MinimiType)i].Clear();
        }


        // 임시로 여기서 미니미 생성
        // 나중에 스테이지 생성 과정에서 다루어야 할듯
        for (int i = 0; i < 3; i++)
        {
            Minimi curMinimi = CreateMinimi(MinimiType.Block);
            if (curMinimi != null)
                curMinimi.Initialize();
        }

        CreateBlueprintMinimi();
    }

    private void CreateBlueprintMinimi()
    {
        if (blueprintObject[0] != null)
            return;

        if (boxMinimiRef != null)
        {
            for (int i = 0; i < blueprintObject.Length; i++)
            {
                blueprintObject[i] = Instantiate(boxMinimiRef);
                blueprintObjectRenderers[i] = blueprintObject[i].GetComponentInChildren<Renderer>();
                blueprintObject[i].SetActive(false);
            }
        }
        else
        {
            Debug.LogError("박스 미니미 프리팹 등록 안됨");
        }
    }

    /// <summary>
    /// 미니미를 스테이지 상에 생성
    /// </summary>
    /// <param name="minimiType">생성할 미니미 종류</param>
    /// <returns>생성된 미니미</returns>
    public Minimi CreateMinimi(MinimiType minimiType)
    {
        Minimi newMinimi = null;

        switch(minimiType)
        {
            case MinimiType.Block:
                newMinimi = ResourceManager.Instance.CreatePrefab<BlockMinimi>(PrefabNames.BlockMinimi);
                break;
            case MinimiType.Fire:
                //newMinimi = ResourceManager.Instance.CreatePrefab<FireMinimi>(PrefabNames.Minimi_Dump);
                break;
            case MinimiType.Wind:
                //newMinimi = ResourceManager.Instance.CreatePrefab<WindMinimi>(PrefabNames.Minimi_Dump);
                break;
            default:
                break;
        }

        if(newMinimi == null)
            return null;

        allMinimiLists[minimiType].Add(newMinimi);
        return newMinimi;
    }

    /// <summary>
    /// 스테이지에 있는 아직 획득하지 못한 미니미를 플레이어 소유로 획득
    /// </summary>
    /// <param name="minimi"></param>
    public void GainMinimi(Minimi minimi)
    {
        if (minimi == null || minimi.Type == MinimiType.None)
            return;

        ownMinimiLists[minimi.Type].Add(minimi);

        minimi.GoIn();
    }

    /// <summary>
    /// 가방에서 미니미를 꺼냄
    /// </summary>
    /// <param name="minimiType">꺼낸 미니미의 종류</param>
    /// <returns>성공 여부</returns>
    public bool TakeOutMinimi(MinimiType minimiType)
    {
        if (minimiType != onHandMinimiType)
        {
            onHandMinimiType = minimiType;

            foreach(var minimi in onHandMinimiList)
            {
                minimi.GoIn();
            }

            onHandMinimiList.Clear();
        }

        if (onHandMinimiList.Count >= MAX_STACK_COUNT)
            return false;


        foreach (var minimi in ownMinimiLists[onHandMinimiType])
        {
            if (minimi.State == MinimiState.InBag)
            {
                onHandMinimiList.Add(minimi);
                minimi.GoOut();
                Debug.Log("Count:" + onHandMinimiList.Count + ", " + minimi.name + " 꺼냄");
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 현재 손에 들고 있는 미니미를 모두 다시 가방에 집어넣음
    /// </summary>
    public void PutInAllMinimis()
    {
        foreach (var minimi in onHandMinimiList)
        {
            minimi.GoIn();
        }
        onHandMinimiList.Clear();
        onHandMinimiType = MinimiType.None;

        UnDrawBlueprintObject();
    }


    public bool InstallMinimi()
    {
        if (onHandMinimiType == MinimiType.None || onHandMinimiList.Count < 1 || playerTrans == null)
            return false;


        Vector3 targetPos = playerTrans.position + playerTrans.TransformDirection(INSTALL_OFFSET);

        if (!FindGroundPos(ref targetPos))
        {
            return false;
        }

        Minimi parent = GetMergeableMinimi(targetPos, mergeDistance);
        
        // 합쳐질 미니미가 없을 때
        if(parent == null)
        {
            if(CheckInstallArea(targetPos, playerTrans.rotation))
            {
                return false;
            }

            parent = onHandMinimiList[0];

            for (int i = 1; i < onHandMinimiList.Count; i++)
            {
                parent.AddChild(onHandMinimiList[i]);
            }

            parent.Install(targetPos, playerTrans.rotation);
        }
        // 합쳐질 미니미가 있을 때
        else
        {
            for (int i = 0; i < onHandMinimiList.Count; i++)
            {
                parent.AddChild(onHandMinimiList[i]);
            }
        }

        parent.UpdateStatus();

        onHandMinimiList.Clear();
        onHandMinimiType = MinimiType.None;

        UnDrawBlueprintObject();

        return true;
    }

    public bool UninstallMinimi()
    {
        Ray camRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));

        if (Physics.Raycast(camRay, out RaycastHit hit, 999.0f, LayerMask.GetMask("Minimi"), QueryTriggerInteraction.Ignore))
        {
            Minimi curMinimi = hit.collider.GetComponent<Minimi>();

            if (curMinimi == null)
                return false;

            if (curMinimi.Parent != null)
            {
                curMinimi.Parent.Uninstall();
            }
            else
            {
                curMinimi.Uninstall();
            }

            return true;
        }
        else
        {
            Debug.Log("회수할 미니미 없음");
            return false;
        }
    }

    public void UnInstallAllMinimis()
    {
        for (int i = 1; i < (int)MinimiType.Max; i++)
        {
            foreach (var minimi in ownMinimiLists[(MinimiType)i])
            {
                if (minimi.State == MinimiState.Installed)
                {
                    minimi.Uninstall();
                }
            }
        }

    }

    public void DrawBlueprintObject()
    {
        if (playerTrans == null)
            return;

        blueprintActive = true;

        Vector3 targetPos = playerTrans.position + playerTrans.TransformDirection(INSTALL_OFFSET);

        if (FindGroundPos(ref targetPos))
        {
            bool validation = !CheckInstallArea(targetPos, playerTrans.rotation);
            bool colorChange = validation != wasValid;

            for (int i = 0; i < onHandMinimiList.Count; i++)
            {
                if (!blueprintObject[i].activeSelf)
                {
                    blueprintObject[i].SetActive(true);
                    colorChange = true;
                }

                blueprintObject[i].transform.SetPositionAndRotation(
                    targetPos + Vector3.up * (2.0f * i), playerTrans.rotation);


                // 설치 가능 여부에 따라 머티리얼을 바꿔 플레이어에게 표시
                if (colorChange && blueprintObjectRenderers[i] != null)
                {
                    if (validation)
                    {
                        blueprintObjectRenderers[i].material.color = COLOR_VALID;
                    }
                    else
                    {
                        blueprintObjectRenderers[i].material.color = COLOR_INVALID;
                    }
                }

            }

            wasValid = validation;
        }
        else
        {
            UnDrawBlueprintObject();
        }
    }

    public void UnDrawBlueprintObject()
    {
        if (!blueprintActive)
            return;

        blueprintActive = false;

        for (int i = 0; i < blueprintObject.Length; i++)
        {
            blueprintObject[i].SetActive(false);
        }
    }

    /// <summary>
    /// 지정한 위치에서 합치기가 가능한 가장 가까운 미니미를 반환합니다. 없을 경우 null
    /// </summary>
    /// <param name="origin">찾는 위치</param>
    /// <param name="radius">찾을 범위</param>
    /// <returns>찾은 미니미</returns>
    private Minimi GetMergeableMinimi(Vector3 origin, float radius)
    {
        Collider[] minimis = Physics.OverlapSphere(origin, radius, LayerMask.GetMask("Minimi"));
        float nearestDistanceSqr = 99999.0f;
        Minimi nearestMinimi = null;


        for (int i = 0; i < minimis.Length; i++)
        {
            Minimi curMinimi = minimis[i].GetComponentInParent<Minimi>();

            if (curMinimi == null)
            {
                Debug.LogWarning("Component 확인 필요: " + minimis[i].gameObject.name);
                continue;
            }

            if (curMinimi.State != MinimiState.Installed ||
                curMinimi.ChildCount + onHandMinimiList.Count >= MAX_STACK_COUNT)
            {
                continue;
            }

            if (curMinimi.Type == onHandMinimiType)
            {
                float distanceSqr = (curMinimi.transform.position - origin).sqrMagnitude;
                if (distanceSqr <= nearestDistanceSqr)
                {
                    nearestDistanceSqr = distanceSqr;
                    nearestMinimi = curMinimi;
                }
            }
        }

        return nearestMinimi;
    }

    /// <summary>
    /// 미니미가 설치될 곳이 비어 있는지 검사. 아무것도 없으면 false 반환
    /// </summary>
    /// <param name="targetPosition">검사할 위치</param>
    /// <param name="targetRotation">박스의 회전값</param>
    /// <returns>물체 존재 여부</returns>
    private bool CheckInstallArea(Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 halfExt = new Vector3(0.95f, 0.95f, 0.95f);
        Vector3 center = targetPosition + Vector3.up * halfExt.y;

        return Physics.CheckBox(center, halfExt, targetRotation,
            LayerMask.GetMask("Ground", "Object"), QueryTriggerInteraction.Ignore);
    }

    private bool FindGroundPos(ref Vector3 targetPosition)
    {
        Vector3 origin = targetPosition + Vector3.up * (INSTALL_HEIGHT_TOLERANCE * 0.5f);
        RaycastHit hit;

        bool result = Physics.SphereCast(origin, 0.1f, Vector3.down, out hit, INSTALL_HEIGHT_TOLERANCE,
            LayerMask.GetMask("Ground", "Object"), QueryTriggerInteraction.Ignore);

        if(result)
            targetPosition = origin + Vector3.down * hit.distance;

        return result;
    }

    
}
