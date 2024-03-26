
public class StateMachine
{
    public State CurrentState { get; set; }

    public virtual void Initialize(State startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    public virtual void ChangeState(State newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
