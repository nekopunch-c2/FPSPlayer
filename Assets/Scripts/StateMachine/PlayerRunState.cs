using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    private float _storedSpeed;
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState()
    {
        _storedSpeed = _ctx.Speed;
        _ctx.RunningSpeed = _ctx.Speed * _ctx.SpeedMultiplier;
        _ctx.Speed = 1f;
    }

    public override void UpdateState()
    {
        HandleRun();
        CheckSwitchStates();
        
    }

    public override void ExitState()
    {
        _ctx.Speed = _storedSpeed;
        _ctx.RunningSpeed = 1f;
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
        //Vector3 movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
        Debug.Log("Should Run ");
        //multiply it by the speed multiplier
        //_ctx.Movement = _ctx.BaseMovement * _ctx.SpeedMultiplier;


    }
}
