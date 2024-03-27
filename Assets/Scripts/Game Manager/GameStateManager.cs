using Fusion;
using System;
using TMPro;
using UnityEngine;

public class GameStateManager : NetworkBehaviour
{
    enum GamePhase
    {
        Waiting,
        Preview,
        Starting,
        Race,
        Finish,
        End
    }

    [Networked] private TickTimer Timer { get; set; }
    [Networked] private GamePhase Phase { get; set; }


    [SerializeField] private float _previewTime = 5f;
    [SerializeField] private float _startDelay = 3.0f;
    [SerializeField] private CanvasesManagerUI _managerUI;
    public CanvasesManagerUI CanvasManagerUI { get => _managerUI; }


    public bool GameIsRunning => Phase != GamePhase.Waiting;

    private PlayerAcceleration _localPlayerAcceleration;


    public override void Spawned()
    {
        _managerUI.PreviewInstance.Initialize();


        if (Object.HasStateAuthority)
        {
            Phase = GamePhase.Waiting;
        }
    }

    public override void Render()
    {
        // Update the game display with the information relevant to the current game phase
        switch (Phase)
        {
            case GamePhase.Waiting:
                UpdateWaitingDisplay();
                break;
            case GamePhase.Preview:
                UpdatePreviewDisplay();
                break;
            case GamePhase.Starting:
                UpdateStartingDisplay();
                break;
            case GamePhase.Race:
                UpdateRunningDisplay(); 
                break;
            case GamePhase.Finish:
                //UpdateFinishDisplay();
                if (HasStateAuthority)
                {
                    //CheckIfGameHasEnded();
                }
                break;
            case GamePhase.End:
                //UpdateEndingDisplay();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateWaitingDisplay()
    {
        if (!Object.HasStateAuthority)
            return;

        if (Runner.SessionInfo.PlayerCount == 2)
        {
            Phase = GamePhase.Preview;
            Timer = TickTimer.CreateFromSeconds(Runner, _previewTime);
        }
    }

    private void UpdatePreviewDisplay()
    {
        _managerUI.PreviewInstance.Initialize();


        if (!Object.HasStateAuthority)
            return;

        if (!Timer.Expired(Runner))
            return;

        Phase = GamePhase.Starting;
        Timer = TickTimer.CreateFromSeconds(Runner, _startDelay);
    }

    private void UpdateStartingDisplay()
    {
        _managerUI.PreviewInstance.Deinitialize();
        _managerUI.StartInstance.Initialize();
        _managerUI.StartInstance.SetTimer(Mathf.RoundToInt(Timer.RemainingTime(Runner) ?? 0));
        

        if (!Object.HasStateAuthority)
            return;

        if (!Timer.Expired(Runner))
            return;


        // Switches to the Running GameState and sets the time to the length of a game session
        Phase = GamePhase.Race;
    }

    private void UpdateRunningDisplay()
    {
        if(_localPlayerAcceleration == null)
        {
            if (Runner.TryGetPlayerObject(Runner.LocalPlayer, out var networkObject))
            {
                if (networkObject.TryGetComponent(out _localPlayerAcceleration))
                {
                    _localPlayerAcceleration.StartAcceleration();
                }
            }
        } else
        {
            _managerUI.StartInstance.Deinitialize();
            _managerUI.InputInstance.Initialize();
            _managerUI.PlayerInstance.Initialize();
            _managerUI.PlayerInstance.SetSpeed(_localPlayerAcceleration.CurrentSpeed);
        }
    }

    /*private void UpdateEndingDisplay()
    {
        // --- All clients
        // Display the results and
        // the remaining time until the current game session is shutdown
        
        if (Runner.TryFindBehaviour(Winner, out PlayerDataNetworked playerData) == false) return;

        _startEndDisplay.gameObject.SetActive(true);
        _ingameTimerDisplay.gameObject.SetActive(false);
        _startEndDisplay.text = $"{playerData.NickName} won with {playerData.Score} points. Disconnecting in {Mathf.RoundToInt(Timer.RemainingTime(Runner) ?? 0)}";
        _startEndDisplay.color = SpaceshipController.GetColor(playerData.Object.InputAuthority.PlayerId);

        // Shutdowns the current game session.
        if (Timer.Expired(Runner))
            Runner.Shutdown();
    }

    public void CheckIfGameHasEnded()
    {
        // --- Master client

        if (Timer.ExpiredOrNotRunning(Runner))
        {
            GameHasEnded();
            return;
        }

        // Dont check for the first few seconds of the match or after a player joined for a winner to allow for players to join and spawn their spaceships
        if (_dontCheckforWinTimer.Expired(Runner) == false)
        {
            return;
        }


        int playersAlive = 0;

        for (int i = 0; i < _playerDataNetworkedIds.Count; i++)
        {
            if (Runner.TryFindBehaviour(_playerDataNetworkedIds[i],
                    out PlayerDataNetworked playerDataNetworkedComponent) == false)
            {
                _playerDataNetworkedIds.RemoveAt(i);
                i--;
                continue;
            }

            if (playerDataNetworkedComponent.Lives > 0) playersAlive++;
        }


        // If more than 1 player is left alive, the game continues.
        // If only 1 player is left, the game ends immediately.
        if (playersAlive > 1 || (Runner.ActivePlayers.Count() == 1 && playersAlive == 1)) return;

        foreach (var playerDataNetworkedId in _playerDataNetworkedIds)
        {
            if (Runner.TryFindBehaviour(playerDataNetworkedId,
                    out PlayerDataNetworked playerDataNetworkedComponent) ==
                false) continue;

            if (playerDataNetworkedComponent.Lives > 0 == false) continue;

            Winner = playerDataNetworkedId;
        }

        if (Winner == default) // when playing alone in host mode this marks the own player as winner
        {
            Winner = _playerDataNetworkedIds[0];
        }

        GameHasEnded();
    }

    private void GameHasEnded()
    {
        Timer = TickTimer.CreateFromSeconds(Runner, _endDelay);
        Phase = GamePhase.Ending;
    }*/
}
