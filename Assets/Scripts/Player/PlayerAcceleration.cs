using Fusion;
using System.Collections;
using UnityEngine;

public class PlayerAcceleration : NetworkBehaviour
{
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _timeToMaxSpeed = 15f;

    [SerializeField] private float _speedFromNitro = 5f;
    [SerializeField] private float _nitroAccelerationDecrease = 1f;

    private float _currentSpeed;
    private float _acceleration = 0f;
    private float _nitroAcceleration = 0f;

    private float _maxSpeedCoef = 1f; // Для уменьшения максимальной скорости

    private Rigidbody _rb;


    private bool _nitroPressed = false;

    [Networked] private TickTimer _timer { get; set; }

    public override void Spawned()
    {
        StartAcceleration();
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _currentSpeed = 0f;
    }

    public void StartAcceleration()
    {
        _acceleration = _maxSpeed / _timeToMaxSpeed;
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
        if(_acceleration == 0f) { return; }


        if(_nitroPressed)
        {
            _nitroAcceleration = _speedFromNitro;
            _nitroPressed = false;
        }


        if (_currentSpeed < _maxSpeed * _maxSpeedCoef)
        {
            _currentSpeed += (_acceleration + _nitroAcceleration) * Runner.DeltaTime;
        }
        else
        {
            _currentSpeed = _maxSpeed * _maxSpeedCoef;
        }


        if(_nitroAcceleration > 0f)
        {
            _nitroAcceleration -= _nitroAccelerationDecrease * Runner.DeltaTime;
        }

        if(_timer.Expired(Runner))
        {
            ResetMaxSpeed();
        }


        _rb.velocity = new Vector3(_rb.velocity.x, 0, _currentSpeed);
    }
}
