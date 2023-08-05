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
        //_storedSpeed = _ctx.Speed;
        _ctx.RunningSpeed = _ctx.Speed * _ctx.SpeedMultiplier;
        //_ctx.Speed = 1f;
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates())
        {
            return;
        }
        HandleRun();
    }

    public override void ExitState()
    {
        //_ctx.Speed = _storedSpeed;
        _ctx.RunningSpeed = 1f;
    }

    public override bool CheckSwitchStates()
    {
        if ((_ctx.IsMoving && !_ctx.IsRunning) || _ctx.IsTooTired)
        {

            SwitchState(_factory.Walk());
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

    void HandleRun()
    {
        float currentMultiplier = _ctx.VectorMultiplier;
        float targetValue = 2f;
        float interpolatedValue = Mathf.Lerp(currentMultiplier, targetValue, _ctx.AnimationBlendSpeed * Time.deltaTime);
        _ctx.VectorMultiplier = interpolatedValue;

        _ctx.StaminaGetSet -= _ctx.StaminaDecIncSpeedGetSet * Time.deltaTime;

    }
}
