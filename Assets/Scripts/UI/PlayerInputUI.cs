using Fusion;
using UnityEngine;

public class PlayerInputUI : NetworkBehaviour
{
    [SerializeField] private GameObject _inputCanvas;

    private PlayerAcceleration _player;

    public void Initialize()
    {
        if(_player == null)
        {
            InitializePlayer();
        }

        _inputCanvas.SetActive(true);
    }

    public void Deinitialize()
    {
        _inputCanvas.SetActive(false);
    }
    private void InitializePlayer()
    {
        if (Runner.TryGetPlayerObject(Runner.LocalPlayer, out var networkObject))
        {
            if (networkObject.TryGetComponent(out _player)) {  }
        }
    }

    public void OnNitroPressed()
    {
        if(_player  != null)
        {
            _player.NitroPressed();
        }
    }
}
