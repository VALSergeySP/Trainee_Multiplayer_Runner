using Fusion;
using System.Collections;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float _laneChangeSpeed = 15f;
   
    private float _laneOffset;
    private int _numOfLanes = 2;

    private Rigidbody _rb;
    private SwipeManager _swipeManager;

    private float _pointStart;
    private float _pointFinish;

    private Coroutine _movementRoutine;
    private bool _isMoving = false;
    private float _lastVelocityX = 0f;

    public override void Spawned()
    {
        _pointStart = _pointFinish = transform.position.x;
        _laneOffset = LevelStaticInfo.LaneSize;
        _rb = GetComponent<Rigidbody>();
        _swipeManager = GetComponent<SwipeManager>();
    }


    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false) { return; }

        if(_swipeManager.IsLeftSwipe && _pointFinish > -_laneOffset * _numOfLanes)
        {
            MoveHorizontal(-_laneChangeSpeed);
            _swipeManager.ResetSwipes();
        } else if (_swipeManager.IsRightSwipe && _pointFinish < _laneOffset * _numOfLanes)
        {
            MoveHorizontal(_laneChangeSpeed);
            _swipeManager.ResetSwipes();
        }
    }


    public void MoveBackOnLine()
    {
        MoveHorizontal(-_lastVelocityX);
    }

    private void MoveHorizontal(float speed)
    {
        _pointStart = _pointFinish;
        _pointFinish += Mathf.Sign(speed) * _laneOffset;

        if (_isMoving)
        {
            StopCoroutine(_movementRoutine);
            _isMoving = false;
        }

        _movementRoutine = StartCoroutine(MoveRoutine(speed));
    }

    IEnumerator MoveRoutine(float speed)
    {
        _isMoving = true;
        _lastVelocityX = speed;

        while (Mathf.Abs(_pointStart - transform.position.x) < _laneOffset)
        {
            yield return new WaitForFixedUpdate();

            _rb.velocity = new Vector3(speed, 0, _rb.velocity.z);

            float x = Mathf.Clamp(transform.position.x, Mathf.Min(_pointStart, _pointFinish), Mathf.Max(_pointStart, _pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        _rb.velocity = new Vector3(0, 0, _rb.velocity.z);
        transform.position = new Vector3(_pointFinish, transform.position.y, transform.position.z);

        _isMoving = false;
    }
}
