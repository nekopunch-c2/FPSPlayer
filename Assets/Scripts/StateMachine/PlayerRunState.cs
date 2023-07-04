using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{

    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleRun();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsMoving && !_ctx.IsRunning)
        {
            SwitchState(_factory.Walk());
        }
        else if (!_ctx.IsMoving)
        {
            SwitchState(_factory.Idle());
        }

    }

    public override void InitializeSubState()
    {

    }

    void HandleRun()
    {
        // Current _vectorMultiplier value
        float currentMultiplier = _ctx.VectorMultiplier;
        // Target value for interpolation
        float targetValue = 2f;

        // Calculate the interpolated value
        float interpolatedValue = Mathf.Lerp(currentMultiplier, targetValue, _ctx.AnimationBlendSpeed * Time.deltaTime);

        // Assign the interpolated value back to VectorMultiplier
        _ctx.VectorMultiplier = interpolatedValue;
        //calculate the movement direction
        Vector3 movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);

        //multiply it by the speed multiplier
        movement = movement * _ctx.SpeedMultiplier;

        //use char controller's Move() method to move the player using Movement, Speed and deltaTime
        _ctx.CharacterController.Move(movement * _ctx.Speed * Time.deltaTime);
    }
}
