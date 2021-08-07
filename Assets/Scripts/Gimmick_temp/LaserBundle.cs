using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBundle : Switch_C_OBJ
{

    public LineRenderer[] lasers;
    public Transform[] startPoints;
    Coroutine active;
    LayerMask layerMask;
    bool alreadyActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground", "Object", "Player");
        lasers = GetComponentsInChildren<LineRenderer>();
        startPoints = GetComponentsInChildren<Transform>();
        foreach (Switchs_Ctrl switchs in _switchs)
        {
            if (switchs._connetObj != null) { Debug.LogError("한개의 스위치는 한개의 오브젝트만 연결이 가능합니다."); }
            switchs.Connecting(GetComponent<LaserBundle>());
        }
        active = StartCoroutine(ActiveLaser());
    }

    private void Update()
    {
        if(AllSwitchOn && !alreadyActive)
        {
            active = StartCoroutine(ActiveLaser());
            alreadyActive = true;
        }
    }

    IEnumerator ActiveLaser()
    {
        while (AllSwitchOn)
        {
            int i = 1;
            foreach (LineRenderer laser in lasers)
            {
                laser.enabled = true;
                RaycastHit hit;
                Physics.Raycast(
                    startPoints[i].position,
                    startPoints[i].forward,
                    out hit,
                    40f,
                    layerMask,
                    QueryTriggerInteraction.Ignore
                    );
                
                laser.SetPosition(0, startPoints[i].position);
                laser.SetPosition(1, hit.point);

                if (hit.collider != null && hit.collider.CompareTag(Tags.Player))
                {
                    hit.collider.GetComponent<PlayerCharacter>().TakeDamage(1, -hit.collider.transform.forward);
                }

                i++;

                yield return new WaitForSeconds(0.03f);
            }
        }
        if (!AllSwitchOn)
        {
            foreach (LineRenderer laser in lasers)
            {
                laser.enabled = false;
            }
            alreadyActive = false;
            StopCoroutine(active);
        }
    }
}
