using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{

    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        HandleWalk();
        CheckSwitchStates();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsMoving && _ctx.IsRunning)
        {
            SwitchState(_factory.Run());
        }
        else if (!_ctx.IsMoving)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void InitializeSubState()
    {

    }

    void HandleWalk()
    {
        float currentMultiplier = _ctx.VectorMultiplier;
        // Target value for interpolation
        float targetValue = 1f;

        // Calculate the interpolated value
        float interpolatedValue = Mathf.Lerp(currentMultiplier, targetValue, _ctx.AnimationBlendSpeed * Time.deltaTime);

        // Assign the interpolated value back to VectorMultiplier
        _ctx.VectorMultiplier = interpolatedValue;
        //_ctx.AirMovementSmoothValueInAir = movement;
        Vector3 movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
        _ctx.CharacterController.Move(movement * _ctx.Speed * Time.deltaTime);
    }
}
