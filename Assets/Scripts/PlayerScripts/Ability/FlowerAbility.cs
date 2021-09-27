using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAbility : PlayerAbility
{
    [Tooltip("삼키기 가능한 거리")]
    [SerializeField]
    private float swallowDistance = 5.0f;

    [Tooltip("발사되는 힘")]
    [SerializeField]
    private float firePower = 25.0f;

    [Tooltip("현재 가지고 있는 오브젝트")]
    [SerializeField, ReadOnly]
    private ISwallowableObject curHavingObject;

    [Tooltip("현재 크로스헤어에 있는 오브젝트")]
    [SerializeField, ReadOnly]
    private ISwallowableObject focusedObject;


    private bool havingObject = false;

    private bool aiming = false;



    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    public override void AbilityUpdate()
    {
        if (havingObject)
            return;

        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = CameraManager.Instance.MainCam.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f,
            LayerMasks.Object, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag(Tags.SwallowableObject))
            {
                float distanceSqr = 
                    (pc.transform.position - hit.collider.transform.position).sqrMagnitude;

                if (distanceSqr <= swallowDistance * swallowDistance)
                {
                    focusedObject = hit.collider.GetComponent<ISwallowableObject>();

                    UIManager.Instance.HUD.SetBehaviourDesc(true);
                    return;
                }
            }
        }
        focusedObject = null;
        UIManager.Instance.HUD.SetBehaviourDesc(false);
    }

    public override void MainAction1(KeyInfo key)
    {
        if (key.down)
            MainAction1_Down();

        else if (key.current)
            MainAction1_Holding();
    }

    private void MainAction1_Down()
    {
        if (aiming && havingObject)
        {
            Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
            Ray ray = CameraManager.Instance.MainCam.ScreenPointToRay(screenCenter);
            Vector3 fireDirection;

            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f,
                LayerMasks.GO, QueryTriggerInteraction.Ignore))
            {
                fireDirection = (hit.point - pc.firePoint.position).normalized;
            }
            else
            {
                fireDirection = (ray.GetPoint(50.0f) - pc.firePoint.position).normalized;
            }

            curHavingObject.Spit(pc.firePoint.position, fireDirection * firePower);
            curHavingObject = null;
            havingObject = false;
        }
        else if (focusedObject != null)
        {
            havingObject = true;
            curHavingObject = focusedObject;
            focusedObject = null;
            curHavingObject.Swallow();

            UIManager.Instance.HUD.SetBehaviourDesc(false);
        }
    }

    private void MainAction1_Holding()
    {

    }

    public override void MainAction2(KeyInfo key)
    {
        if (key.down)
            MainAction2_Down();

        else if (key.up)
            MainAction2_Up();
    }

    private void MainAction2_Down()
    {
        aiming = true;
        pc.ChangeState(PlayerState.Aim);
    }

    private void MainAction2_Up()
    {
        if (aiming)
        {
            aiming = false;
            pc.ChangeState(PlayerState.Idle);
        }
    }

    private void MainAction2_Holding()
    {

    }

    public override void NumAction1(KeyInfo key)
    {
        throw new System.NotImplementedException();
    }

    public override void NumAction2(KeyInfo key)
    {
        throw new System.NotImplementedException();
    }

    public override void NumAction3(KeyInfo key)
    {
        throw new System.NotImplementedException();
    }
}
