using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MinimiManager : MonoBehaviour
{
    private static readonly Vector3 INSTALL_RAY_OFFSET = new Vector3(0.0f, 1.1f, 2.0f);

    /// <summary>
    /// ��ġ ���� ���� ��밪
    /// </summary>
    private const float INSTALL_HEIGHT_TOLERANCE = 1.0f;

    /// <summary>
    /// �� ���� ���ü�ġ�Ǵ� �ִ� ����
    /// </summary>
    public const int MAX_STACK_COUNT = 3;



    public static MinimiManager _instance = null;

    [SerializeField] private GameObject boxMinimiRef = null;


    public bool IsEmpty { get => onHandMinimiList.Count == 0; }

    /// <summary>
    /// �̴Ϲ̰� �������� �Ÿ�. �� �Ÿ� �ȿ� �̴Ϲ̸� ��ġ�ϸ� ������
    /// </summary>
    [SerializeField] private float mergeDistance = 2.0f;

    /// <summary>
    /// �ʻ� �ִ� ��� �̴Ϲ��� ����Ʈ
    /// </summary>
    private Dictionary<MinimiType, List<Minimi>> allMinimiLists = new Dictionary<MinimiType, List<Minimi>>();
    
    /// <summary>
    /// �÷��̾ �����ϰ� �ִ� �̴Ϲ��� ����Ʈ
    /// </summary>
    private Dictionary<MinimiType, List<Minimi>> ownMinimiLists = new Dictionary<MinimiType, List<Minimi>>();

    /// <summary>
    /// �տ� ��� �ִ� �̴Ϲ��� ����Ʈ
    /// </summary>
    private List<Minimi> onHandMinimiList = new List<Minimi>();

    /// <summary>
    /// ���� ��ġ�Ϸ��� �غ����� �̴Ϲ��� ����
    /// </summary>
    private MinimiType onHandMinimiType = MinimiType.None;


    private bool blueprintActive = false;
    private GameObject[] blueprintObject = new GameObject[MAX_STACK_COUNT];



    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        for(int i=1; i<(int)MinimiType.Max; i++)
        {
            allMinimiLists.Add((MinimiType)i, new List<Minimi>());
            ownMinimiLists.Add((MinimiType)i, new List<Minimi>());
        }

        if (boxMinimiRef != null)
        {
            for (int i = 0; i < blueprintObject.Length; i++)
            {
                blueprintObject[i] = Instantiate(boxMinimiRef);
                blueprintObject[i].SetActive(false);
            }
        }
        else
        {
            Debug.LogError("�ڽ� �̴Ϲ� ������ ��� �ȵ�");
        }

    }

    private void Start()
    {
        // �ӽ÷� ���⼭ �̴Ϲ� ����
        // ���߿� �������� ���� �������� �ٷ��� �ҵ�
        for (int i = 0; i < 3; i++)
        {
            Minimi curMinimi = CreateMinimi(MinimiType.Block);
            if(curMinimi != null)
                curMinimi.Initialize();
        }

    }

    /// <summary>
    /// �̴Ϲ̸� �������� �� ����
    /// </summary>
    /// <param name="minimiType">������ �̴Ϲ� ����</param>
    /// <returns>������ �̴Ϲ�</returns>
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
    /// ���������� �ִ� ���� ȹ������ ���� �̴Ϲ̸� �÷��̾� ������ ȹ��
    /// </summary>
    /// <param name="minimi"></param>
    public void GainMinimi(Minimi minimi)
    {
        if (minimi == null || minimi.Type == MinimiType.None)
            return;

        ownMinimiLists[minimi.Type].Add(minimi);
    }

    /// <summary>
    /// ���濡�� �̴Ϲ̸� ����
    /// </summary>
    /// <param name="minimiType">���� �̴Ϲ��� ����</param>
    /// <returns>���� ����</returns>
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
                Debug.Log("Count:" + onHandMinimiList.Count + ", " + minimi.name + " ����");
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// ���� �տ� ��� �ִ� �̴Ϲ̸� ��� �ٽ� ���濡 �������
    /// </summary>
    public void PutInAllMinimis()
    {
        foreach (var minimi in onHandMinimiList)
        {
            minimi.GoIn();
        }
        onHandMinimiList.Clear();
        onHandMinimiType = MinimiType.None;
    }


    public bool InstallMinimi(Vector3 position, Quaternion rotation)
    {
        if (onHandMinimiType == MinimiType.None || onHandMinimiList.Count < 1)
            return false;


        Vector3 installPos;

        if (!FindGroundPos(position, out installPos))
        {
            return false;
        }

        Minimi parent = GetMergeableMinimi(installPos, mergeDistance);
        
        // ������ �̴Ϲ̰� ���� ��
        if(parent == null)
        {
            if(CheckInstallArea(installPos, rotation))
            {
                return false;
            }

            parent = onHandMinimiList[0];

            for (int i = 1; i < onHandMinimiList.Count; i++)
            {
                parent.AddChild(onHandMinimiList[i]);
            }

            parent.Install(installPos, rotation);
        }
        // ������ �̴Ϲ̰� ���� ��
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
            Debug.Log("ȸ���� �̴Ϲ� ����");
            return false;
        }
    }

    /// <summary>
    /// ������ ��ġ���� ��ġ�Ⱑ ������ ���� ����� �̴Ϲ̸� ��ȯ�մϴ�. ���� ��� null
    /// </summary>
    /// <param name="origin">ã�� ��ġ</param>
    /// <param name="radius">ã�� ����</param>
    /// <returns>ã�� �̴Ϲ�</returns>
    public Minimi GetMergeableMinimi(Vector3 origin, float radius)
    {
        Collider[] minimis = Physics.OverlapSphere(origin, radius, LayerMask.GetMask("Minimi"));
        float nearestDistanceSqr = 99999.0f;
        Minimi nearestMinimi = null;


        for (int i = 0; i < minimis.Length; i++)
        {
            Minimi curMinimi = minimis[i].GetComponentInParent<Minimi>();

            if (curMinimi == null)
            {
                Debug.LogWarning("Component Ȯ�� �ʿ�: " + minimis[i].gameObject.name);
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
    /// �̴Ϲ̰� ��ġ�� ���� ��� �ִ��� �˻�. �ƹ��͵� ������ false ��ȯ
    /// </summary>
    /// <param name="targetPosition">�˻��� ��ġ</param>
    /// <param name="targetRotation">�ڽ��� ȸ����</param>
    /// <returns>��ü ���� ����</returns>
    public bool CheckInstallArea(Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 halfExt = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 center = targetPosition + Vector3.up * halfExt.y;

        return Physics.CheckBox(center, halfExt, targetRotation,
            LayerMask.GetMask("Ground", "Object"), QueryTriggerInteraction.Ignore);
    }

    public bool FindGroundPos(Vector3 targetPosition, out Vector3 foundPosition)
    {
        Vector3 origin = targetPosition + Vector3.up * (INSTALL_HEIGHT_TOLERANCE * 0.5f);
        RaycastHit hit;

        bool result = Physics.SphereCast(origin, 0.1f, Vector3.down, out hit, INSTALL_HEIGHT_TOLERANCE,
            LayerMask.GetMask("Ground", "Object"), QueryTriggerInteraction.Ignore);

        if(result)
            foundPosition = origin + Vector3.down * hit.distance;
        else
            foundPosition = targetPosition;

        return result;
    }

    public void DrawBlueprintObject(Vector3 targetPosition, Quaternion targetRotation)
    {
        blueprintActive = true;

        Vector3 groundPos;

        if (FindGroundPos(targetPosition, out groundPos))
        {
            bool possibility = !CheckInstallArea(groundPos, targetRotation);

            for (int i = 0; i < onHandMinimiList.Count; i++)
            {
                if (!blueprintObject[i].activeSelf)
                {
                    blueprintObject[i].SetActive(true);
                }

                blueprintObject[i].transform.position = groundPos + Vector3.up * (2.0f * i);
                blueprintObject[i].transform.rotation = targetRotation;

                // TODO: ��ġ ���� ���ο� ���� ��Ƽ������ �ٲ� �÷��̾�� ǥ��
            }

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
}
