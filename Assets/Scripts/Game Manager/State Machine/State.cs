public class State
{
    protected StateMachine _stateMachine;
    protected GameStateManager _gameStateManager;

    
    public State(GameStateManager gameStateManager, StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _gameStateManager = gameStateManager;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void NetworkUpdate() { }
}
