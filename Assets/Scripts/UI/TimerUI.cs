using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TimerUI : NetworkBehaviour
{
    [SerializeField] private GameObject _timerCanvas;
    [SerializeField] private TMP_Text _placeText;
    [SerializeField] private TMP_Text _timerText;

    private float _currentTime = 0f;
    private bool _isInitialized = false;

    private Transform _localPlayerPosition;
    private Transform _remotePlayerPosition;

    private void SetPlayersData()
    {
        List<PlayerRef> playerRefs = Runner.ActivePlayers.ToList();

        foreach (var player in playerRefs)
        {
            if (Runner.TryGetPlayerObject(player, out var networkPlayer) == false) 
                continue;

            if (Runner.LocalPlayer == player)
            {
                _localPlayerPosition = networkPlayer.transform;
            } else
            {
                _remotePlayerPosition = networkPlayer.transform;
            }
        }
    }

    public void Initialize()
    {
        if(_isInitialized) return;

        _currentTime = 0f;
        SetPlayersData();
        _timerCanvas.SetActive(true);
        _isInitialized = true;
    }

    private void Update()
    {
        if(!_isInitialized) return;

        _currentTime += Time.deltaTime;

        _timerText.text = FloatToTime(_currentTime);

        if(_localPlayerPosition.position.z > _remotePlayerPosition.position.z)
        {
            SetPlayerPosition(1);
        } else
        {
            SetPlayerPosition(2);
        }
    }

    private void SetPlayerPosition(int position)
    {
        if (position == 1)
        {
            _placeText.text = "1st";
        } else
        {
            _placeText.text = "2nd";
        }
    }

    private string FloatToTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        string result = $"{timeSpan.Minutes}:{timeSpan.Seconds}:{timeSpan.Milliseconds}";

        return result;
    }
}
