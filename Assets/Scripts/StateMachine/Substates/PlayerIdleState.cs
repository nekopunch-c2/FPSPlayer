using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates())
        {
            return;
        }
        HandleIdle();
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
        else if (_ctx.IsMoving)
        {
            SwitchState(_factory.Walk());
            return true;
        }
        return false;
    }

    public override void InitializeSubState()
    {

    }

    void HandleIdle()
    {
        float currentMultiplier = _ctx.VectorMultiplier;
        // Target value for interpolation
        float targetValue = 1f;
        // Calculate the interpolated value
        float interpolatedValue = Mathf.Lerp(currentMultiplier, targetValue, _ctx.AnimationBlendSpeed * Time.deltaTime);

        // Assign the interpolated value back to VectorMultiplier
        _ctx.VectorMultiplier = interpolatedValue;

        if (_ctx.StaminaGetSet != _ctx.MaxStamina)
        {
            _ctx.StaminaGetSet += _ctx.StaminaDecIncSpeedGetSet * Time.deltaTime;
        }
    }
}
