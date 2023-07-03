using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    //private PlayerBaseState playerBaseState= new PlayerBaseState();
    private Vector3 movementSmooth;
    private float jumpingY;

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        InitializeSubState();
        movementSmooth = _ctx.MovementSmooth;
    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleJumpUpdate();
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
        if (_ctx.PlayerBody.transform.position.y >= (jumpingY + _ctx.JumpHeightPull))
        {
            SwitchState(_factory.InAir());
        }
    }

    public override void InitializeSubState()
    {
        if (!_ctx.IsMoving && (_ctx.GetPlayerRunning() == 0f || !_ctx.IsRunning))
        {
            SetSubState(_factory.Idle());
        }
        else if (_ctx.IsMoving && (_ctx.GetPlayerRunning() == 0f || !_ctx.IsRunning))
        {
            SetSubState(_factory.Walk());
        }
        else
        {
            SetSubState(_factory.Run());
        }
    }
    void HandleJump()
    {
        _ctx.VelocityY = Mathf.Sqrt(_ctx.JumpHeight * -2f * _ctx.Gravity);
        Debug.Log(_ctx.VelocityY);
        //_ctx.IsJumping = true;

        jumpingY = _ctx.PlayerBody.transform.position.y;
        Debug.Log(jumpingY);

        Vector3 movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);

        _ctx.AirMovementSmoothValueInAir = Vector3.SmoothDamp(_ctx.AirMovementSmoothValueInAir, movement, ref movementSmooth, _ctx.AirSmoothment);
        //_ctx.CharacterController.Move(_ctx.AirMovementSmoothValueInAir * _ctx.Speed * Time.deltaTime);

        //_ctx.CharacterController.Move(_ctx.Velocity * Time.deltaTime);

        
    }

    void HandleAnim()
    {
        _ctx.Animator.CrossFadeInFixedTime("JumpStart", 0.1f);
    }

    void HandleJumpUpdate()
    {
        //_ctx.VelocityY += _ctx.Gravity * Time.deltaTime;


        _ctx.CharacterController.Move(_ctx.Velocity * Time.deltaTime);
    }

}
