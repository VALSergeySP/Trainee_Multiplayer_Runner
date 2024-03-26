using Fusion;
using UnityEngine;

public class GameStateManager : NetworkBehaviour
{
    [SerializeField] private float _startDelayTime = 5f;

    public StateMachine GameStateMachine { get; private set; }
    public PreviewState GamePreviewState { get; private set; }
    public StartState GameStartState { get; private set; }
    public RaceState GameRaceState { get; private set; }
    public FinishState GameFinishState { get; private set; }
    public EndState GameEndState { get; private set; }

    private void Awake()
    {
        GameStateMachine = new StateMachine();

        GamePreviewState = new PreviewState(this, GameStateMachine);
        GameStartState = new StartState(this, GameStateMachine);
        GameRaceState = new RaceState(this, GameStateMachine);
        GameFinishState = new FinishState(this, GameStateMachine);
        GameEndState = new EndState(this, GameStateMachine);
    }

    private void Start()
    {
        GameStateMachine.Initialize(GamePreviewState);
    }

    private void Update()
    {
        GameStateMachine.CurrentState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        GameStateMachine.CurrentState.PhysicsUpdate();
    }
}
