
public abstract class PlayerBaseState
{
    protected bool _isRootState = false;
    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;
    protected PlayerBaseState _currentSubState;
    protected PlayerBaseState _currentSuperState;

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract bool CheckSwitchStates();

    public abstract void InitializeSubState();

    public void UpdateStates() 
    {
        UpdateState();
        if(_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(PlayerBaseState newState) 
    {
        //current state exits state
        ExitState();

        //newState enters state
        newState.EnterState();
        if (_isRootState)
        {
            //switch current state of context
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState) 
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState) 
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

}
