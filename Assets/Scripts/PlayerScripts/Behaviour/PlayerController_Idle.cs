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

    private delegate void IdleUpdateDelegate();
    private IdleUpdateDelegate onIdleUpdate;



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

        if (playerAbility != null)
            playerAbility.AbilityUpdate();

        if (onIdleUpdate != null)
            onIdleUpdate();
    }

    private void Idle_FixedUpdate()
    {
        Move();
    }
    
    private void Idle_Exit(PlayerState next)
    {
        MinimiManager.Instance.UnDrawBlueprintObject();

        onIdleUpdate -= MinimiManager.Instance.DrawBlueprintObject;
    }
    #endregion


    private void Idle_GetInput()
    {
        // TODO 준비단계가 필요없으면 input e안으로
        RaycastHit hit = Raycast(RAY_DISTANCE);
        interactType = UpdateInteractActionType(hit);

        if (key_interact)
        {
            Debug.Log("Input E Key");
            Interact_Action(hit, interactType);
        }


        // 좌클릭
        /*if (abilityAction1.down)
        {
            if (MinimiManager.Instance.InstallMinimi())
            {
                onIdleUpdate -= MinimiManager.Instance.DrawBlueprintObject;
            }
        }*/
        playerAbility.MainAction1(mainAbilityAction1);

        // 우클릭
        /*if (mainAbilityAction2.down)
        {
            MinimiManager.Instance.PutInAllMinimis();
            onIdleUpdate -= MinimiManager.Instance.DrawBlueprintObject;
        }*/

        playerAbility.MainAction2(mainAbilityAction2);

        // 블럭 미니미
        /*if (key_alpha1)
        {
            if(MinimiManager.Instance.TakeOutMinimi(MinimiType.Block))
            {
                onIdleUpdate += MinimiManager.Instance.DrawBlueprintObject;
            }
        }*/

        // F Key
        /*if (key_f)
        {
            MinimiManager.Instance.UninstallMinimi();
        }*/

        moveDirection = moveDirection.relativeTo(CameraT);
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
            case InteractType.Wagon:
                Interact_Action_Drag();
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

        if (layer == Layers.Minimi)
        {
            //Debug.Log("Did Hit Minimi");
            return InteractType.Block;
        }
        else if (layer == Layers.Obj)
        {
            //Debug.Log("Did Hit Obejct");
            if (fsm.CurState == PlayerState.Idle)
            {
                hold_target = hit.transform;

                return InteractType.Hold;
            }
        }

        else if (layer == Layers.Wagon)
        {
            if(fsm.CurState == PlayerState.Idle)
            {
                wagon = hit.transform;

                return InteractType.Wagon;
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

    private void Interact_Action_Drag()
    {
        switch (fsm.CurState)
        {
            case PlayerState.Idle:
                fsm.ChangeState(PlayerState.Drag);
                break;
            case PlayerState.Drag:
                fsm.ChangeState(PlayerState.Idle);
                break;
        }
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
