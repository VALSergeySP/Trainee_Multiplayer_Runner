using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    private const float MIN_SWIPE_DIST = 50f;


    private bool _leftSwipe = false;
    public bool IsLeftSwipe { get => _leftSwipe; }

    private bool _rightSwipe = false;
    public bool IsRightSwipe { get => _rightSwipe; }




    private Vector2 _startTouchPosition;
    private Vector2 _swipeMovementDelta;

    private bool _touchMoved;


    private Vector2 TouchPosition() { return (Vector2)Input.mousePosition; }
    private bool TouchBegan() { return Input.GetMouseButtonDown(0); }
    private bool TouchEnded() { return Input.GetMouseButtonUp(0); }
    private bool GetTouch() { return Input.GetMouseButton(0); }


    private void Update()
    {
        if (TouchBegan())
        {
            _startTouchPosition = TouchPosition();
            _touchMoved = true;
        }
        else if (TouchEnded() && _touchMoved == true)
        {
            _leftSwipe = false;
            _rightSwipe = false;
            _touchMoved = false;
        }

        _swipeMovementDelta = Vector2.zero;
        if (_touchMoved && GetTouch())
        {
            _swipeMovementDelta = TouchPosition() - _startTouchPosition;
        }

        if (_swipeMovementDelta.sqrMagnitude > MIN_SWIPE_DIST * MIN_SWIPE_DIST)
        {
            if (Mathf.Abs(_swipeMovementDelta.x) > Mathf.Abs(_swipeMovementDelta.y))
            {
                _leftSwipe = _swipeMovementDelta.x < 0;
                _rightSwipe = _swipeMovementDelta.x > 0;
            }

            _swipeMovementDelta = Vector2.zero;
            _touchMoved = false;
        }
    }

    public void ResetSwipes()
    {
        _leftSwipe = false;
        _rightSwipe = false;
    }
}
