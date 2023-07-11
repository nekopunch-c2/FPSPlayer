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
        HandleSlopesAndStairs();
        InitializeSubState();
    }

    public override void UpdateState()
    {
        Debug.Log("IN AIR STATE");
        HandleGravity();
        HandleAnim();
        CheckSwitchStates();
        //_ctx.Movement = _ctx.AirMovementSmoothValueInAir;
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsGrounded/* && !_ctx.IsJumping*/)
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

    void HandleSlopesAndStairs()
    {
        RaycastHit hit;

        if (Physics.SphereCast(_ctx.PlayerBody.position * (_ctx.CharacterController.height / 2), 1f, Vector3.down, out hit, _ctx.LengthFromGround))
        {

            //Debug.Log(hit.distance);
            if (hit.distance >= 0.01f)
            {
                //_ctx.IsFalling = true;

            }
            else
            {
                Debug.Log(hit.distance);
                //_ctx.IsFalling = false;
            }
        }
    }

    public void HandleGravity()
    {
        _ctx.VelocityY += _ctx.Gravity * Time.deltaTime;
        //_ctx.CharacterController.Move(_ctx.Velocity * Time.deltaTime);

    }

    void HandleAnim()
    {
        _ctx.Animator.SetBool(_ctx.AnimIDInAir, true);
        _ctx.Animator.SetBool(_ctx.AnimIDGrounded, false);
        _ctx.Animator.SetBool(_ctx.AnimIDLanding, false);
    }
}
