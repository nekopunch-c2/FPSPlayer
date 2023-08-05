using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    private float _storedSpeed;

    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void EnterState()
    {
        /*_storedSpeed = _ctx.Speed;
        _ctx.Speed = ;
        _ctx.RunningSpeed = x;*/
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates())
        {
            return;
        }
        HandleWalk();
    }

    public override void ExitState()
    {
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.IsMoving && _ctx.IsRunning && !_ctx.IsTooTired)
        {
            SwitchState(_factory.Run());
            return true;
        }
        else if (!_ctx.IsMoving)
        {
            SwitchState(_factory.Idle());
            return true;
        }
        return false;
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
        //Vector3 movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
        //_ctx.CharacterController.Move(_ctx.Movement * _ctx.Speed * Time.deltaTime);
        if (_ctx.StaminaGetSet != _ctx.MaxStamina)
        {
            _ctx.StaminaGetSet += _ctx.StaminaDecIncSpeedGetSet * Time.deltaTime;
        }

    }
}
