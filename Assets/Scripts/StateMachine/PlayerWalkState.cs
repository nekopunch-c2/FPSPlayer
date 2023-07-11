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
        //Debug.Log("IS WALKING");
        HandleWalk();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        /*_ctx.Speed = x;
        _ctx.RunningSpeed = x;*/
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsMoving && _ctx.IsRunning)
        {
            SwitchState(_factory.Run());
        }
        else if (!_ctx.IsMoving)
        {
            Debug.Log("Entered else for idle");
            SwitchState(_factory.Idle());
            Debug.Log("Passed idle");
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
        //Vector3 movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
        //_ctx.CharacterController.Move(_ctx.Movement * _ctx.Speed * Time.deltaTime);
    }
}
