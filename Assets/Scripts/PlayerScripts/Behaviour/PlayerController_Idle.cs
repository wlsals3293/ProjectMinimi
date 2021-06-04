using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    private const float RAY_DISTANCE = 5f;

    private static readonly Vector3 INSTALL_OFFSET = new Vector3(0.0f, 0.0f, 2.0f);

   
    private UseKeyActionType useKeyType = UseKeyActionType.None;


    private bool blueprintActive = false;


    #region <행동 추가시 디폴트 작업>
    private void Idle_SetState()
    {
        PlayerManager.Instance.AddBehaviour(
            PlayerState.Idle
            , new SimpleBehaviour(Idle_Enter, Idle_Update, Idle_Exit));
    }

    private void Idle_Enter(PlayerState prev)
    {

    }

    private void Idle_Update()
    {
        Idle_GetInput();
        DetectGround();
        if(!isOnGround)
            ApplyGravity();

        if (jumpInput && canJump)
        {
            Jump();
        }

        Move();
        Turn();

        rb.velocity = moveVelocity + (Vector3.up * verticalVelocity);


        if(blueprintActive)
        {
            MinimiManager._instance.DrawBlueprintObject(
                transform.position + transform.TransformDirection(INSTALL_OFFSET),
                transform.rotation);
        }
    }

    
    private void Idle_Exit(PlayerState next)
    {
    }
    #endregion


    private void Idle_GetInput()
    {
        // TODO 준비단계가 필요없으면 input e안으로
        RaycastHit hit = Raycast(RAY_DISTANCE);
        useKeyType = UpdateUseKeyActionType(hit);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogError("Input E Key");
            UseKeyAction(hit, useKeyType);
        }

        
        // 좌클릭
        if (Input.GetMouseButtonDown(0))
        {
            if (!MinimiManager._instance.IsEmpty)
            {
                Vector3 installPos = transform.position + transform.TransformDirection(INSTALL_OFFSET);

                if(MinimiManager._instance.InstallMinimi(installPos, transform.rotation))
                {
                    blueprintActive = false;
                    MinimiManager._instance.UnDrawBlueprintObject();
                }
            }
                
        }
        // 우클릭
        if (Input.GetMouseButtonDown(1))
        {
            MinimiManager._instance.PutInAllMinimis();
            blueprintActive = false;
            MinimiManager._instance.UnDrawBlueprintObject();
        }

        // 블럭 미니미
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(MinimiManager._instance.TakeOutMinimi(MinimiType.Block))
            {
                blueprintActive = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            MinimiManager._instance.UninstallMinimi();

            if (MinimiManager._instance.IsEmpty)
            {
                blueprintActive = false;
                MinimiManager._instance.UnDrawBlueprintObject();
            }
                
        }


        input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0.0f,
            Input.GetAxisRaw("Vertical")
            );
        inputDir = input.normalized;
        jumpInput = Input.GetButton("Jump");

        moveDirection = (input.z * Vector3.Scale(cameraT.forward, new Vector3(1, 0, 1)).normalized + input.x * cameraT.right).normalized;
    }

    public void UseKeyAction(RaycastHit hit, UseKeyActionType keyType)
    {
        switch (keyType)
        {
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
        if (hit.collider == null)
            return UseKeyActionType.None;

        int layer = hit.collider.gameObject.layer;

        if (layer == Layers.minimi)
        {
            Debug.Log("Did Hit Minimi");
            return UseKeyActionType.Block;
        }
        else if (layer == Layers.obj)
        {
            Debug.Log("Did Hit Obejct");
            if (hold_target == null)
            {
                hold_target = hit.transform;
            }
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
        Vector3 pos = trans.position + Vector3.up;

        if (Physics.Raycast(pos, trans.TransformDirection(Vector3.forward), out hit, RAY_DISTANCE))
        {
#if UNITY_EDITOR
            Debug.DrawLine(pos, pos + (trans.TransformDirection(Vector3.forward) * hit.distance), Color.red);
#endif
        }

        return hit;
    }

}
