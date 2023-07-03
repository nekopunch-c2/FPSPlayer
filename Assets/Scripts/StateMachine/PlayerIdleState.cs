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
        CheckSwitchStates();
        Debug.Log("the state is Idle");
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
        else if (_ctx.IsMoving)
        {
            SwitchState(_factory.Walk());
            Debug.Log("the state should be walk");
        }
    }

    public override void InitializeSubState()
    {

    }
}
