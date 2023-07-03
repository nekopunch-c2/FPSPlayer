using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) 
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleAnim();
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
        if (!_ctx.IsGrounded)
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

}
