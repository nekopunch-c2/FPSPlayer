using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerBaseState, IRootState
{
    public PlayerInAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) 
    {
        _isRootState = true;
       
    }
    public override void EnterState()
    {
        //Debug.Log("cui");
        InitializeSubState();
    }

    public override void UpdateState()
    {
        HandleGravity();
        HandleAnim();
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsGrounded)
        {
            SwitchState(_factory.Grounded());
        }

    }

    public override void InitializeSubState()
    {
        if (!_ctx.IsMoving && !_ctx.IsRunning)
        {
            SetSubState(_factory.Idle());
        }
        else if (_ctx.IsMoving && !_ctx.IsRunning)
        {
            SetSubState(_factory.Walk());
        }
        else
        {
            SetSubState(_factory.Run());
        }
    }
    public void HandleGravity()
    {
        _ctx.VelocityY += _ctx.Gravity * Time.deltaTime;
        _ctx.CharacterController.Move(_ctx.Velocity * Time.deltaTime);

    }

    void HandleAnim()
    {
        _ctx.Animator.SetBool(_ctx.AnimIDInAir, true);
        _ctx.Animator.SetBool(_ctx.AnimIDGrounded, false);
        _ctx.Animator.SetBool(_ctx.AnimIDLanding, false);
    }
}
