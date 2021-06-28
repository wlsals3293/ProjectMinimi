using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
using ECM.Common;

public partial class PlayerController : BaseCharacterController
{
    private const float RAY_DISTANCE = 5f;

   
    private InteractType interactType = InteractType.None;


    private StateUpdateDelegate IdleUpdateDelegate;



    #region <행동 추가시 디폴트 작업>
    private void Idle_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Idle
            , new SimpleBehaviour(Idle_Enter, Idle_Update, Idle_FixedUpdate, Idle_Exit));
    }

    private void Idle_Enter(PlayerState prev)
    {

    }

    private void Idle_Update()
    {
        Idle_GetInput();
        UpdateRotation();
        Animate();

        if (IdleUpdateDelegate != null)
        {
            IdleUpdateDelegate();
        }
    }

    private void Idle_FixedUpdate()
    {
        Move();
    }
    
    private void Idle_Exit(PlayerState next)
    {
        MinimiManager.Instance.UnDrawBlueprintObject();

        IdleUpdateDelegate -= MinimiManager.Instance.DrawBlueprintObject;
    }
    #endregion


    private void Idle_GetInput()
    {
        // TODO 준비단계가 필요없으면 input e안으로
        RaycastHit hit = Raycast(RAY_DISTANCE);
        interactType = UpdateInteractActionType(hit);

        if (key_interact)
        {
            Debug.LogError("Input E Key");
            Interact_Action(hit, interactType);
        }


        // 좌클릭
        if (leftClick)
        {
            if (MinimiManager.Instance.InstallMinimi())
            {
                IdleUpdateDelegate -= MinimiManager.Instance.DrawBlueprintObject;
            }
        }
        // 우클릭
        if (rightClick)
        {
            MinimiManager.Instance.PutInAllMinimis();
            IdleUpdateDelegate -= MinimiManager.Instance.DrawBlueprintObject;
        }

        // 블럭 미니미
        if (key_alpha1)
        {
            if(MinimiManager.Instance.TakeOutMinimi(MinimiType.Block))
            {
                IdleUpdateDelegate += MinimiManager.Instance.DrawBlueprintObject;
            }
        }

        // F Key
        if (key_f)
        {
            MinimiManager.Instance.UninstallMinimi();
        }



        if (CameraManager.Instance.CurrentCameraCtrl != null && CameraManager.Instance.CurrentCameraCtrl.IsMainCamera)
        {
            //moveDirection = (input.z * Vector3.Scale(cameraT.forward, new Vector3(1, 0, 1)).normalized + input.x * cameraT.right).normalized;
            moveDirection = moveDirection.relativeTo(cameraT);
        }
        else
        {
            //moveDirection = (input.z * Vector3.Scale(cameraT.up, new Vector3(1, 0, 1)).normalized + input.x * cameraT.right).normalized;
        }
    }

    public void Interact_Action(RaycastHit hit, InteractType keyType)
    {
        switch (keyType)
        { 
            case InteractType.None:
                break;
            case InteractType.Block:
                Interact_Action_Block(hit);
                break;
            case InteractType.Hold:
                Interact_Action_Hold();
                break;
        }
        
    }

    private InteractType UpdateInteractActionType(RaycastHit hit)
    {
        int layer = -1;

        if (hit.collider != null)
        {
            layer = hit.collider.gameObject.layer;
        }

        if (layer == Layers.minimi)
        {
            //Debug.Log("Did Hit Minimi");
            return InteractType.Block;
        }
        else if (layer == Layers.obj)
        {
            //Debug.Log("Did Hit Obejct");
            if (fsm.CurState == PlayerState.Idle)
            {
                hold_target = hit.transform;

                return InteractType.Hold;
            }
        }

        if (fsm.CurState == PlayerState.Hold && hold_target != null)
        {
            // OnOff Hold
            return InteractType.Hold;
        }

        return InteractType.None;
    }


    private void Interact_Action_Hold()
    {
        switch (fsm.CurState)
        {
            case PlayerState.Idle:
                fsm.ChangeState(PlayerState.Hold);
                break;
            case PlayerState.Hold:
                fsm.ChangeState(PlayerState.Idle);
                break;
        }
    }
    private void Interact_Action_Block(RaycastHit hit)
    {
        // TODO : 임시함수 매니져를 통해 함수호출로 변경 or state로 빼기
        //hit.collider.SendMessage(MinimiController.SEND_SETPIVOT, trans);
        climbFaceNormal = hit.normal;
        ChangeState(PlayerState.Climb);
    }
    

    public void DrawLineRaycatAllways(float distance = 0f)
    {
        if(distance != 0)
        {
            Raycast(distance);
        }
        else
        {
            Raycast(RAY_DISTANCE);
        }
    }

    private RaycastHit Raycast(float distance)
    {
        RaycastHit hit;
        Vector3 pos = trans.position + (Vector3.up * 0.5f);

        if (Physics.Raycast(pos, trans.TransformDirection(Vector3.forward), out hit, RAY_DISTANCE))
        {
#if UNITY_EDITOR
            Debug.DrawLine(pos, pos + (trans.TransformDirection(Vector3.forward) * hit.distance), Color.red);
#endif
        }

        return hit;
    }

}
