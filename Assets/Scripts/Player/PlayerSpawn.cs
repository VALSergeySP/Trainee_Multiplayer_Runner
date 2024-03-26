using Fusion;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    private Camera _camera;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            _camera = Camera.main;
            _camera.GetComponent<ThirdPersonCamera>().Target = transform;
        }
    }
}
