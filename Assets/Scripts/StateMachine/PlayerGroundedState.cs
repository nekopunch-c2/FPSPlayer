using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) 
    {
        _isRootState = true;
        
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void UpdateState()
    {
        
        HandleAnim();
        OnSlope();
        CheckSwitchStates();
    }

    public void HandleGravity()
    {

    }

    public override void ExitState()
    {
        //Debug.Log("Cosito exited grounded");
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.PlayerJump())
        {
            SwitchState(_factory.Jump());
        }
        if (!_ctx.IsGrounded && !_ctx.IsJumping)
        {
            SwitchState(_factory.InAir());
        }
    }

    public override void InitializeSubState()
    {
        if (!_ctx.IsMoving && (!_ctx.IsRunning))
        {
            SetSubState(_factory.Idle());
        }
        else if (_ctx.IsMoving && (!_ctx.IsRunning))
        {
            SetSubState(_factory.Walk());
        }
        else
        {
            SetSubState(_factory.Run());
        }
    }

    void HandleAnim()
    {
        _ctx.Animator.SetBool(_ctx.AnimIDInAir, false);
        _ctx.Animator.SetBool(_ctx.AnimIDLanding, true);

        _ctx.Animator.SetBool(_ctx.AnimIDGrounded, true);
    }
    private bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(_ctx.playerBody.position * (_ctx.CharacterController.height / 2), Vector3.down, out hit, _ctx.CharacterController.height / 2 * _ctx.SlopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

}
