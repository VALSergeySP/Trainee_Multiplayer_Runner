using Fusion;
using UnityEngine;

public class SlowObstacle : Obstacle
{
    [SerializeField] private float _slowCoef = 0.5f;
    [SerializeField] private float _slowTime = 3f;

    protected override void OnTrigger(GameObject player)
    {
        player.GetComponent<PlayerAcceleration>().LimitMaxSpeed(_slowCoef, _slowTime);
    }
}
