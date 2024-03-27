using Fusion;
using System.Collections;
using UnityEngine;

public class PlayerAcceleration : NetworkBehaviour
{
    public delegate void PlayerSpeedDelegate(float speed);
    public event PlayerSpeedDelegate PlayerSpeedChangedEvent; 

    [SerializeField] private float _maxSpeed = 5f;
    public float MaxSpeed { get => _maxSpeed + _speedFromNitro * _speedFromNitro; }
    [SerializeField] private float _timeToMaxSpeed = 15f;

    [SerializeField] private float _speedFromNitro = 5f;
    [SerializeField] private float _nitroAccelerationDecrease = 1f;
    public float NitroDurationTime { get => _speedFromNitro / _nitroAccelerationDecrease; }

    private float _currentSpeed;
    public float CurrentSpeed { get => _currentSpeed; }

    private float _acceleration = 0f;
    private float _nitroAcceleration = 0f;

    private float _maxSpeedCoef = 1f; // ��� ���������� ������������ ��������

    private Rigidbody _rb;


    private bool _nitroPressed = false;
    private bool _isNitroCollected = false;
    public bool IsNitroCollected { get => _isNitroCollected; }

    [Networked] private TickTimer _timer { get; set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _currentSpeed = 0f;
    }

    public void NitroCollected()
    {
        _isNitroCollected = true;
    }

    public void NitroPressed()
    {
        _nitroPressed = true;
    }

    public void StartAcceleration()
    {
        _acceleration = _maxSpeed / _timeToMaxSpeed;

        GetComponent<SwipeManager>().enabled = true;
    }

    public void StopAcceleration()
    {
        _acceleration = 0f;
    }

    public void ResetSpeed()
    {
        _currentSpeed = 0f;
    }

    public void SlowSpeed(float coef)
    {
        _currentSpeed *= coef;
    }

    public void LimitMaxSpeed(float coef, float time)
    {
        _currentSpeed *= coef;
        _maxSpeedCoef = coef;

        _timer = TickTimer.CreateFromSeconds(Runner, time);
    }

    public void ResetMaxSpeed()
    {
        _maxSpeedCoef = 1f;

        _timer = TickTimer.None;
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false) { return; }
        if(_acceleration == 0f) { return; }


        if (_nitroPressed && _isNitroCollected)
        {
            _nitroAcceleration = _speedFromNitro;
            _nitroPressed = false;
            _isNitroCollected = false;

            Debug.LogWarning("Nitro Used!");
        } else
        {
            _nitroPressed = false;
        }


        if (_currentSpeed < (_maxSpeed * _maxSpeedCoef + _speedFromNitro * _nitroAcceleration))
        {
            _currentSpeed += (_acceleration + _nitroAcceleration) * Runner.DeltaTime;
        }
        else
        {
            _currentSpeed = _maxSpeed * _maxSpeedCoef + _speedFromNitro * _nitroAcceleration;
        }


        if(_nitroAcceleration > 0f)
        {
            _nitroAcceleration -= _nitroAccelerationDecrease * Runner.DeltaTime;
        } else
        {
            _nitroAcceleration = 0f;
        }

        if(_timer.Expired(Runner))
        {
            ResetMaxSpeed();
        }

        PlayerSpeedChangedEvent?.Invoke(_currentSpeed);
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _currentSpeed);
    }
}
