using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private Vector3 _cameraOffset;

    private void LateUpdate()
    {
        if (Target == null) { return; }

        transform.position = Target.position + _cameraOffset;
    }
}
