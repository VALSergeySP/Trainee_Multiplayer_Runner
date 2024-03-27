using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpeedUI : NetworkBehaviour
{
    [SerializeField] private GameObject _speedCanvas;
    [SerializeField] private Image _arrowImage;
    [SerializeField] private TMP_Text _speedText;

    [SerializeField] private float _arrowStartRotation = 45f;
    [SerializeField] private float _arrowMaxRotation = -135f;
    [SerializeField] private float _arrowSpeed = 0.5f;

    [SerializeField] private float _speedCoef = 10f; // Чтоб скорость была больше 20 км/ч
    [SerializeField] private float _randomSpeedShifts = 1f;

    private float _playerMaxSpeed;
    private float _arrowDelta = 0f;

    public void Initialize()
    {
        if(_arrowDelta == 0f)
        {
            _speedText.text = "0";
            _arrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, _arrowStartRotation);

            InitializePlayer();
        }

        _speedCanvas.SetActive(true);
    }

    public void Deinitialize()
    {
        _speedCanvas.SetActive(false);
    }


    private void InitializePlayer()
    {
        if (Runner.TryGetPlayerObject(Runner.LocalPlayer, out var networkObject))
        {
            if (networkObject.TryGetComponent<PlayerAcceleration>(out var _player))
            {
                _playerMaxSpeed = _player.MaxSpeed;
            }
        }

        CanculateDelta();
    }

    private void CanculateDelta()
    {
        _arrowDelta = (_arrowStartRotation - _arrowMaxRotation) / _playerMaxSpeed;
    }

    public void SetSpeed(float speed)
    {
        float randomNum = Random.Range(-_randomSpeedShifts, _randomSpeedShifts);

        float newRotation = _arrowStartRotation - speed * _arrowDelta + randomNum;

        _speedText.text = Mathf.FloorToInt(speed * _speedCoef + randomNum).ToString();
        _arrowImage.rectTransform.rotation = Quaternion.Lerp(_arrowImage.rectTransform.rotation, Quaternion.Euler(0, 0, newRotation), _arrowSpeed);
    }
}
