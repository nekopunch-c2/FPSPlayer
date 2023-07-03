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
        CheckSwitchStates();
        HandleWalk();
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchStates()
    {
        if (!_ctx.GetPlayerMovementApplied.Equals(Vector2.zero) && _ctx.IsRunning)
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
        Debug.Log("isWalking");
        Vector3 movement = (_ctx.MoveInputY * _ctx.PlayerBody.forward) + (_ctx.MoveInputX * _ctx.PlayerBody.right);
        //_ctx.AirMovementSmoothValueInAir = movement;

        //_ctx.Animator.SetFloat(_ctx.XVelHash, _ctx.CurrentInputVectorX);
        //_ctx.Animator.SetFloat(_ctx.YVelHash, _ctx.CurrentInputVectorY);

        _ctx.CharacterController.Move(movement * _ctx.Speed * Time.deltaTime);
    }
}
