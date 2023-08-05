using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderTransitionState : PlayerBaseState, IRootState
{
    private bool _transitionHasFinished;
    float timeCount;

    public PlayerLadderTransitionState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        _isRootState = true;

    }

    public override void EnterState()
    {
        _ctx.IsJumping = false;
        _ctx.IsFalling = false;
        timeCount = 0.0f;
        //Debug.Log("Hello from the Transition to ladDEr");
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
            _ctx.PlayerBody.position = Vector3.SmoothDamp(_ctx.PlayerBody.position, _ctx.PlayerAboveLadderClimbingPos.position, ref velocity, 0.05f);
            _ctx.PlayerBody.rotation = Quaternion.Lerp(_ctx.PlayerBody.rotation, _ctx.PlayerAboveLadderClimbingPos.rotation, timeCount);
            timeCount = timeCount +  Time.deltaTime;
            float tolerance = 0.01f; 
            if (Vector3.Distance(_ctx.PlayerBody.position, _ctx.PlayerAboveLadderClimbingPos.position) < tolerance)
            {
                _transitionHasFinished = true;

            }          
        }
        Debug.Log("transiftion has finished = " + _transitionHasFinished);
        if (_ctx.BelowLadder)
        {
            _ctx.PlayerBody.position = Vector3.SmoothDamp(_ctx.PlayerBody.position, _ctx.PlayerBelowLadderClimbingPos.position, ref velocity, 0.05f);
            _ctx.PlayerBody.rotation = Quaternion.Lerp(_ctx.PlayerBody.rotation, _ctx.PlayerBelowLadderClimbingPos.rotation, timeCount);
            timeCount = timeCount + Time.deltaTime;
            float tolerance = 0.05f; 
            if (Vector3.Distance(_ctx.PlayerBody.position, _ctx.PlayerBelowLadderClimbingPos.position) < tolerance)
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
        if (_ctx.OnLadder && _transitionHasFinished)
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
