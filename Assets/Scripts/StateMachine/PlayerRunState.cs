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
}
