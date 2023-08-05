using System.Collections.Generic;

enum PlayerStates
{
    idle,
    walk,
    run,
    crouch,
    jump,
    grounded,
    inAir,
    onLadder,
    ladderTranistion,
    fromLadderTransition
}

public class PlayerStateFactory
{
    PlayerStateMachine _context;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
        _states[PlayerStates.walk] = new PlayerWalkState(_context, this);
        _states[PlayerStates.run] = new PlayerRunState(_context, this);
        _states[PlayerStates.crouch] = new PlayerCrouchState(_context, this);
        _states[PlayerStates.jump] = new PlayerJumpState(_context, this);
        _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
        _states[PlayerStates.inAir] = new PlayerInAirState(_context, this);
        _states[PlayerStates.onLadder] = new PlayerOnLadderState(_context, this);
        _states[PlayerStates.ladderTranistion] = new PlayerLadderTransitionState(_context, this);
        _states[PlayerStates.fromLadderTransition] = new PlayerFromLadderTransitionState(_context, this);
    }

    public PlayerBaseState Idle()
    {
        return _states[PlayerStates.idle];
    }
    public PlayerBaseState Walk()
    {
        return _states[PlayerStates.walk];
    }
    public PlayerBaseState Run()
    {
        return _states[PlayerStates.run];
    }

    public PlayerBaseState Crouch()
    {
        return _states[PlayerStates.crouch];
    }
    public PlayerBaseState Jump()
    {
        return _states[PlayerStates.jump];
    }
    public PlayerBaseState Grounded()
    {
        return _states[PlayerStates.grounded];
    }
    public PlayerBaseState InAir()
    {
        return _states[PlayerStates.inAir];
    }
    public PlayerBaseState OnLadder()
    {
        return _states[PlayerStates.onLadder];
    }
    public PlayerBaseState LadderTransition()
    {
        return _states[PlayerStates.ladderTranistion];
    }

    public PlayerBaseState FromLadderTransition()
    {
        return _states[PlayerStates.fromLadderTransition];
    }
}
