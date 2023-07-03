using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    public PlayerInAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState()
    {
        //Debug.Log("cui");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleJumpGravity();
        HandleAnim();
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

    }
    void HandleJumpGravity()
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
