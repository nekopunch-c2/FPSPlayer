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
        _ctx.IsJumping = false;
        _ctx.IsFalling = false;
        
        InitializeSubState();
    }

    public override void UpdateState()
    {
        _ctx.Movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
        
        HandleAnim();
        CheckSwitchStates();
        //_ctx.IsJumping = true;
    }

    public void HandleGravity()
    {
        if (_ctx.VelocityY < 0)
        {
            _ctx.VelocityY = -2f;
        }
        if (_ctx.OnStairsTagGetSet || _ctx.OnSlopeGetSet)
        {
            _ctx.VelocityY += _ctx.Gravity * Time.deltaTime;
            //_ctx.CharacterController.Move(_ctx.Velocity * _ctx.SlopeAndStairForce * Time.deltaTime);
            //Debug.Log("on stairs or slope via tag");
        }
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
        else if (!_ctx.IsGrounded && !_ctx.IsJumping)
        {
            //_ctx.IsJumping = true;
             SwitchState(_factory.InAir());
        }
        else if (_ctx.IsGrounded && _ctx.IsCrouching)
        {
            SwitchState(_factory.Crouch());
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

        if (_ctx.InputAllowed)
        {
            _ctx.Animator.SetFloat(_ctx.XVelHash, _ctx.CurrentInputVectorX);
            _ctx.Animator.SetFloat(_ctx.YVelHash, _ctx.CurrentInputVectorY);
        }

        _ctx.Animator.SetBool(_ctx.AnimIDInAir, false);
        _ctx.Animator.SetBool(_ctx.AnimIDLanding, true);

        _ctx.Animator.SetBool(_ctx.AnimIDGrounded, true);
        _ctx.Animator.SetBool(_ctx.AnimIDCrouching, false);
    }
    

}
