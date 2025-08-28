using Fusion;
using TMPro;
using UnityEngine;

public class FinishUI : NetworkBehaviour
{
    [SerializeField] private GameObject _finishCanvas;
    [SerializeField] private TMP_Text _placeText;
    [SerializeField] private TMP_Text _timeText;

    PlayerDataNetworked _playerData = null;

    public void Initialize()
    {
        if (_playerData == null)
        {
            InitializePlayer();
        }

        _placeText.text = _playerData.NickName.ToString();
        _timeText.text = _playerData.FinishTime.ToString();
        _finishCanvas.SetActive(true);
    }

    public void Deinitialize()
    {
        _finishCanvas.SetActive(false);
    }


    private void InitializePlayer()
    {
        if (Runner.TryGetPlayerObject(Runner.LocalPlayer, out var networkObject))
        {
            if (networkObject.TryGetComponent(out _playerData))
            {
                _placeText.text = _playerData.NickName.ToString();
                _timeText.text = _playerData.FinishTime.ToString();
            }
        }
    }
}
