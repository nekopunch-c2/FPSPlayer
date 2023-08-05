using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnLadderState : PlayerBaseState, IRootState
{
    private Vector3 _movementSmooth;

    public PlayerOnLadderState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
        _movementSmooth = _ctx.MovementSmooth;
    }

    public override void EnterState()
    {
        _ctx.IsJumping = false;
        _ctx.IsFalling = false;
        Debug.Log("Hello from the Ladder");
        HandleGravity();
        InitializeSubState();
    }
    public override void UpdateState()
    {

        if (CheckSwitchStates())
        {
            return;
        }
        _ctx.Movement = (_ctx.MoveInputY * _ctx.PlayerBody.up);
        HandleAnim();
    }
    public override void ExitState()
    {
        _ctx.GetPlayerClimb = false;
        _ctx.Animator.SetBool(_ctx.OnLadderHash, false);
    }
    public override bool CheckSwitchStates()
    {
        if (_ctx.PlayerJump && !_ctx.IsFalling)
        {

            SwitchState(_factory.Jump());
            return true;
        }
        else if (_ctx.IsGrounded && _ctx.GetPlayerClimb)
        {
            SwitchState(_factory.Grounded());
            return true;
        }
        else if (_ctx.OnLadder && _ctx.AboveLadder && _ctx.GetPlayerClimb)
        {
            SwitchState(_factory.FromLadderTransition());
        }
        else if (!_ctx.IsJumping && !_ctx.OnLadder)
        {
            //_ctx.IsJumping = true;
            _ctx.VelocityY = 0f;
            SwitchState(_factory.InAir());
            return true;
        }
        else if (_ctx.IsGrounded && _ctx.IsCrouching)
        {
            SwitchState(_factory.Crouch());
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
        _ctx.VelocityY = 0f;
    }
    void HandleAnim()
    {
        _ctx.Animator.SetBool(_ctx.OnLadderHash, true);
        if (_ctx.InputAllowed)
        {
            _ctx.Animator.SetFloat(_ctx.YVelHash, _ctx.CurrentInputVectorY);
        }
        
    }
}
