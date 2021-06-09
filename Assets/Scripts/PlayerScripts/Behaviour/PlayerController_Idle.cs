using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
using ECM.Common;

public partial class PlayerController : BaseCharacterController
{
    private const float RAY_DISTANCE = 5f;

   
    private UseKeyActionType useKeyType = UseKeyActionType.None;


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
        MinimiManager._instance.UnDrawBlueprintObject();

        IdleUpdateDelegate -= MinimiManager._instance.DrawBlueprintObject;
    }
    #endregion


    private void Idle_GetInput()
    {
        // TODO 준비단계가 필요없으면 input e안으로
        RaycastHit hit = Raycast(RAY_DISTANCE);
        useKeyType = UpdateUseKeyActionType(hit);

        if (key_interact)
        {
            Debug.LogError("Input E Key");
            UseKeyAction(hit, useKeyType);
        }


        // 좌클릭
        if (leftClick)
        {
            if (MinimiManager._instance.InstallMinimi())
            {
                IdleUpdateDelegate -= MinimiManager._instance.DrawBlueprintObject;
            }
        }
        // 우클릭
        if (rightClick)
        {
            MinimiManager._instance.PutInAllMinimis();
            IdleUpdateDelegate -= MinimiManager._instance.DrawBlueprintObject;
        }

        // 블럭 미니미
        if (key_alpha1)
        {
            if(MinimiManager._instance.TakeOutMinimi(MinimiType.Block))
            {
                IdleUpdateDelegate += MinimiManager._instance.DrawBlueprintObject;
            }
        }

        // F Key
        if (key_f)
        {
            MinimiManager._instance.UninstallMinimi();
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

    public void UseKeyAction(RaycastHit hit, UseKeyActionType keyType)
    {
        switch (keyType)
        { 
            case UseKeyActionType.None:
                break;
            case UseKeyActionType.Block:
                UseKeyAction_Block(hit);
                break;
            case UseKeyActionType.Hold:
                UseKeyAction_Hold();
                break;
        }
        
    }

    private UseKeyActionType UpdateUseKeyActionType(RaycastHit hit)
    {
        int layer = -1;

        if (hit.collider != null)
        {
            layer = hit.collider.gameObject.layer;
        }

        if (layer == Layers.minimi)
        {
            Debug.Log("Did Hit Minimi");
            return UseKeyActionType.Block;
        }
        else if (layer == Layers.obj)
        {
            Debug.Log("Did Hit Obejct");
            if (fsm.CurState == PlayerState.Idle)
            {
                hold_target = hit.transform;

                return UseKeyActionType.Hold;
            }
        }

        if (fsm.CurState == PlayerState.Holding && hold_target != null)
        {
            // OnOff Hold
            return UseKeyActionType.Hold;
        }

        return UseKeyActionType.None;
    }


    private void UseKeyAction_Hold()
    {
        switch (fsm.CurState)
        {
            case PlayerState.Idle:
                fsm.ChangeState(PlayerState.Holding);
                break;
            case PlayerState.Holding:
                fsm.ChangeState(PlayerState.Idle);
                break;
        }
    }
    private void UseKeyAction_Block(RaycastHit hit)
    {
        // TODO : 임시함수 매니져를 통해 함수호출로 변경 or state로 빼기
        hit.collider.SendMessage(MinimiController.SEND_SETPIVOT, trans);
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
