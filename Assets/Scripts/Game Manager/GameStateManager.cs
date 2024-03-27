using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [Networked] private NetworkBehaviourId Winner { get; set; }


    [SerializeField] private float _previewTime = 5f;
    [SerializeField] private float _startDelay = 3.0f;
    [SerializeField] private float _endDelay = 10.0f;
    [SerializeField] private CanvasesManagerUI _managerUI;
    public CanvasesManagerUI CanvasManagerUI { get => _managerUI; }


    public bool GameIsRunning => Phase != GamePhase.Waiting;

    private NetworkObject _localPlayerObject;
    private PlayerAcceleration _localPlayerAcceleration;
    private PlayerDataNetworked _localPlayerData;

    private List<NetworkBehaviourId> _playerDataNetworkedIds = new();



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
                UpdateRunningOrFinishedDisplay();
                if (HasStateAuthority)
                {
                    CheckIfGameHasEnded();
                }
                break;
            case GamePhase.Finish:
                UpdateFinishDisplay();
                if (HasStateAuthority)
                {
                    CheckIfGameHasEnded();
                }
                break;
            case GamePhase.End:
                UpdateEndingDisplay();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateWaitingDisplay()
    {
        if (!Object.HasStateAuthority)
            return;

        if (Runner.ActivePlayers.Count() == 2)
        {
            SetPlayersData();
            Phase = GamePhase.Preview;
            Timer = TickTimer.CreateFromSeconds(Runner, _previewTime);
        }
    }

    private void SetPlayersData()
    {
        List<PlayerRef> playerRefs = Runner.ActivePlayers.ToList();

        if (playerRefs.Count != 2) return;

        foreach (var player in playerRefs)
        {
            if (Runner.TryGetPlayerObject(player, out var networkPlayer))
            {
                PlayerDataNetworked playerData = networkPlayer.GetComponent<PlayerDataNetworked>();
                _playerDataNetworkedIds.Add(playerData);
            }
        }
    }

    private void UpdatePreviewDisplay()
    {
        if (_playerDataNetworkedIds.Count <= 0)
        {
            SetPlayersData();
        }
        else
        {

            _managerUI.PreviewInstance.Initialize();


            if (!Object.HasStateAuthority)
                return;

            if (!Timer.Expired(Runner))
                return;

            Phase = GamePhase.Starting;
            Timer = TickTimer.CreateFromSeconds(Runner, _startDelay);
        }
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

    private void LocalCheckIfFinished()
    {
        if(_localPlayerData.Finished == true)
        {
            UpdateFinishDisplay();
        } else
        {
            UpdateRunningDisplay();
        }
    }


    private void UpdateRunningOrFinishedDisplay()
    {
        if(_localPlayerAcceleration == null)
        {
            if (Runner.TryGetPlayerObject(Runner.LocalPlayer, out _localPlayerObject))
            {
                if (_localPlayerObject.TryGetComponent(out _localPlayerAcceleration))
                {
                    _localPlayerAcceleration.StartAcceleration();
                }
                if(_localPlayerObject.TryGetComponent(out _localPlayerData)) { }
            }
        } else
        {
            LocalCheckIfFinished();
        }
    }

    private void UpdateRunningDisplay()
    {
        _managerUI.StartInstance.Deinitialize();
        _managerUI.InputInstance.Initialize();
        _managerUI.PlayerInstance.Initialize();
        _managerUI.TimerInstance.Initialize();
        _managerUI.PlayerInstance.SetSpeed(_localPlayerAcceleration.CurrentSpeed);
    }

    private void UpdateFinishDisplay()
    {
        _managerUI.InputInstance.Deinitialize();
        _managerUI.PlayerInstance.Deinitialize();
        _managerUI.FinishInstance.Initialize();
    }

    public void TrackNewPlayer(NetworkBehaviourId playerDataNetworkedId)
    {
        _playerDataNetworkedIds.Add(playerDataNetworkedId);
    }

    private void UpdateEndingDisplay()
    {
        // --- All clients
        // Display the results and
        // the remaining time until the current game session is shutdown
        
        if (Runner.TryFindBehaviour(Winner, out PlayerDataNetworked playerData) == false) return;

        _managerUI.FinishInstance.Deinitialize();
        _managerUI.PreviewInstance.Initialize();

        // Shutdowns the current game session.
        if (Timer.Expired(Runner))
            Runner.Shutdown();
    }

    private int CountFinishedPlayers()
    {
        int playersFinished = 0;

        for (int i = 0; i < _playerDataNetworkedIds.Count; i++)
        {
            if (Runner.TryFindBehaviour(_playerDataNetworkedIds[i], out PlayerDataNetworked playerDataNetworkedComponent) == false)
            {
                _playerDataNetworkedIds.RemoveAt(i);
                i--;
                continue;
            }

            if (playerDataNetworkedComponent.Finished) playersFinished++;
        }

        return playersFinished;
    }

    private void FindWinner()
    {
        float bestFinishTime = 1000f;

        foreach (var playerDataNetworkedId in _playerDataNetworkedIds)
        {
            if (Runner.TryFindBehaviour(playerDataNetworkedId, out PlayerDataNetworked playerDataNetworkedComponent) == false)
                continue;

            if (playerDataNetworkedComponent.Finished && playerDataNetworkedComponent.FinishTime < bestFinishTime)
            {
                Winner = playerDataNetworkedId;
            }
        }
    }

    public void CheckIfGameHasEnded()
    {
        int playersFinished = CountFinishedPlayers();

        if (playersFinished < Runner.ActivePlayers.Count() && (Runner.ActivePlayers.Count() != 1)) return;

        FindWinner();

        if (Winner == default)
        {
            Winner = _playerDataNetworkedIds[0];
        }

        GameHasEnded();
    }

    private void GameHasEnded()
    {
        Timer = TickTimer.CreateFromSeconds(Runner, _endDelay);
        Phase = GamePhase.End;
    }
}
