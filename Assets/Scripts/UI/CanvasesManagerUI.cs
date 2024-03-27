using UnityEngine;

public class CanvasesManagerUI : MonoBehaviour
{
    private PreviewUI _previewUI;
    public PreviewUI PreviewInstance { get => _previewUI; }

    private GameStartUI _startUI;
    public GameStartUI StartInstance { get => _startUI; }

    private PlayerSpeedUI _playerUI;
    public PlayerSpeedUI PlayerInstance { get => _playerUI; }

    private TimerUI _timerUI;
    public TimerUI TimerInstance { get => _timerUI; }

    private PlayerInputUI _playerInputUI;
    public PlayerInputUI InputInstance { get => _playerInputUI; }

    private FinishUI _finishUI;
    public FinishUI FinishInstance { get => _finishUI; }

    private void Start()
    {
        _previewUI = GetComponent<PreviewUI>();
        _startUI = GetComponent<GameStartUI>();
        _playerUI = GetComponent<PlayerSpeedUI>();
        _timerUI = GetComponent<TimerUI>();
        _playerInputUI = GetComponent<PlayerInputUI>();
        _finishUI = GetComponent<FinishUI>();
    }
}
