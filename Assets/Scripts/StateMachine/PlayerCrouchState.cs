using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private float velocityCrouch;
    public PlayerCrouchState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory)
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
        HandleMovement();
        HandleCrouch();
        HandleAnim();
        CheckSwitchStates();
        //_ctx.IsJumping = true;
    }

    

    public override void ExitState()
    {
        //Debug.Log("Cosito exited grounded");
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.PlayerJump() && _ctx.CanGoUp)
        {

            SwitchState(_factory.Jump());
        }
        else if (!_ctx.IsGrounded && !_ctx.IsJumping)
        {
            //_ctx.IsJumping = true;
            SwitchState(_factory.InAir());
        }
        else if (!_ctx.IsJumping && !_ctx.IsCrouching && _ctx.CanGoUp)
        {
            SwitchState(_factory.Grounded());
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

    void HandleMovement()
    {
        _ctx.Movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
    }

    void HandleCrouch()
    {
        float crouching = Mathf.SmoothDamp(_ctx.CharacterController.height, _ctx.CrouchHeight, ref velocityCrouch, _ctx.CrouchSmoothTime);
        _ctx.CharacterController.height = crouching;

        Vector3 crouchingCenter = Vector3.SmoothDamp(_ctx.CharacterController.center, crouchCenterAdjusted, ref MovementSmooth, _ctx.CrouchSmoothTime);
        _ctx.CharacterController.center = crouchingCenter;
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

        _ctx.Animator.SetBool(_ctx.AnimIDGrounded, false);
        _ctx.Animator.SetBool(_ctx.AnimIDCrouching, true);
    }


}
