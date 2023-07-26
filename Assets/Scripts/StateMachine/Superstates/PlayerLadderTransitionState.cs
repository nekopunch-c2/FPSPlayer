using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderTransitionState : MonoBehaviour
{
    private Vector3 _movementSmooth;

    public PlayerLadderTransitionState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
        _movementSmooth = _ctx.MovementSmooth;
    }

    public override void EnterState()
    {
        _ctx.IsJumping = false;
        _ctx.IsFalling = false;
        Debug.Log("Hello from the Transition to ladDEr");
        HandleGravity();
        InitializeSubState();
    }
    public override void UpdateState()
    {
        if (CheckSwitchStates())
        {
            return;
        }
        _ctx.Movement = Vector3.zero;
        HandleAnim();
    }
    public override void ExitState()
    {
        _ctx.GetPlayerClimb = false;
        _ctx.Animator.SetBool(_ctx.OnLadderHash, false);
    }
    public override bool CheckSwitchStates()
    {
        if (_ctx.OnLadder)
        {
            SwitchState(_factory.Climb());
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

    }
}
