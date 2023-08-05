using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFromLadderTransitionState : PlayerBaseState, IRootState
{
    private bool _transitionHasFinished;
    float timeCount;

    public PlayerFromLadderTransitionState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
    }

    public override void EnterState()
    {
        _ctx.IsJumping = false;
        _ctx.IsFalling = false;
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
        Vector3 velocity = Vector3.zero;
        if (_ctx.AboveLadder)
        {
            //smoothdamp in a certain way
            _ctx.PlayerBody.position = Vector3.SmoothDamp(_ctx.PlayerBody.position, _ctx.PlayerAboveLadderGetDownPos.position, ref velocity, 0.05f);
            float tolerance = 0.01f; 
            if (Vector3.Distance(_ctx.PlayerBody.position, _ctx.PlayerAboveLadderGetDownPos.position) < tolerance)
            {
                _transitionHasFinished = true;

            }
        }
        Debug.Log("transiftion has finished = " + _transitionHasFinished);
        if (_ctx.BelowLadder)
        {
            //smoothdamp in another way
            _ctx.PlayerBody.position = Vector3.SmoothDamp(_ctx.PlayerBody.position, _ctx.PlayerBelowLadderGetDownPos.position, ref velocity, 0.05f);
            float tolerance = 0.01f; // A small tolerance value for position comparison.
            if (Vector3.Distance(_ctx.PlayerBody.position, _ctx.PlayerBelowLadderGetDownPos.position) < tolerance)
            {
                _transitionHasFinished = true;
            }
        }
        //HandleAnim();
    }


    public override void ExitState()
    {
        _ctx.GetPlayerClimb = false;
        _ctx.Animator.SetBool(_ctx.OnLadderHash, false);
        _transitionHasFinished = false;
    }
    public override bool CheckSwitchStates()
    {
        if (_ctx.IsGrounded && _transitionHasFinished )
        {
            SwitchState(_factory.Grounded());
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
}
