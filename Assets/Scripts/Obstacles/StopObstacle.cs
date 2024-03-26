using UnityEngine;

public class StopObstacle : Obstacle
{
    [SerializeField] private bool _destroyOnCollision = false;

    protected override void OnCollision(GameObject player)
    {
        player.GetComponent<PlayerAcceleration>().ResetSpeed();

        if(_destroyOnCollision)
        {
            Runner.Despawn(Object);
        }
    }
}
