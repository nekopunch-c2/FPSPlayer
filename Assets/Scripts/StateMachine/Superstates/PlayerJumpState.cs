using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    //private PlayerBaseState playerBaseState= new PlayerBaseState();
    private Vector3 _movementSmooth;
    private float _jumpingY;
    private Vector3 _previousVelocityXZ;
    private Vector3 _airMovementSmoothValueInAir;
    private Vector3 _movementInAirSmooth;
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
        _airMovementSmoothValueInAir = _ctx.AirMovementSmoothValueInAir;
        _movementSmooth = _ctx.MovementSmooth;
    }

    public override void EnterState()
    {
        _ctx.IsJumping = true;
        _previousVelocityXZ = new Vector3(_ctx.MovementX, 0f, _ctx.MovementZ);
        HandleJumpLogic();
        InitializeSubState();
        
    }

    public override void UpdateState()
    {

        if (CheckSwitchStates())
        {
            return;
        }
        _ctx.GroundedTimer += Time.deltaTime;
        HandleGravity();
        HandleJumpUpdate();
        //Debug.Log("VelocityY: " + _ctx.VelocityY);
        //Debug.Log("Gravity: " + _ctx.Gravity);
        
        if (_ctx.VelocityY < 0.3f && _ctx.VelocityY > -0.3f)
        {
            HighestPoint();
        }
        HandleAnim();
        //Debug.Log("JUMPING");
    }

    public override void ExitState()
    {
        
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.GroundedTimer >= 0.1f && _ctx.IsGrounded)
        {
              SwitchState(_factory.Grounded());
            return true;
        }
        else if (_ctx.OnLadder && _ctx.GetPlayerClimb)
        {
            SwitchState(_factory.OnLadder());
            return true;
        }
        return false;
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
    void HandleJumpLogic()
    {
        _ctx.VelocityY = Mathf.Sqrt(_ctx.JumpHeight * -2f * _ctx.Gravity);
        //_ctx.IsJumping = true;

        _jumpingY = _ctx.PlayerBody.transform.position.y;
        _ctx.Animator.CrossFadeInFixedTime("JumpStart", 0.1f);
        //_ctx.Movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);

        //_ctx.AirMovementSmoothValueInAir = Vector3.SmoothDamp(_ctx.AirMovementSmoothValueInAir, _ctx.Movement, ref _movementSmooth, _ctx.AirSmoothment);
        //_ctx.CharacterController.Move(_ctx.AirMovementSmoothValueInAir * _ctx.Speed * Time.deltaTime);

        //_ctx.CharacterController.Move(_ctx.Velocity * Time.deltaTime);


    }

    void HandleAnim()
    {
        //_ctx.Animator.CrossFadeInFixedTime("JumpStart", 0.1f);
    }

    void HandleJumpUpdate()
    {
        //_ctx.CharacterController.Move(_ctx.Velocity * Time.deltaTime);
        //_ctx.Movement = _ctx.AirMovementSmoothValueInAir;
        Vector3 movementInAir = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
        _ctx.Movement = Vector3.SmoothDamp(_ctx.Movement, movementInAir, ref _movementSmooth, _ctx.AirSmoothment); ;
        //_ctx.AirMovementSmoothValueInAir = Vector3.SmoothDamp(_ctx.AirMovementSmoothValueInAir, _ctx.Movement, ref _movementSmooth, _ctx.AirSmoothment);
    }
    void HighestPoint()
    {
        _ctx.GroundFarEnough();
    }
    public void HandleGravity()
    {
        _ctx.VelocityY += _ctx.Gravity * Time.deltaTime;
    }

}
