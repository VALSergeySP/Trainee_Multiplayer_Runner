using UnityEngine;

public class PushBackObstacle : Obstacle
{
    [SerializeField] private float _pushBackDistance = 5f;

    protected override void OnCollision(GameObject player)
    {
        player.GetComponent<PlayerAcceleration>().ResetSpeed();

        Vector3 playerPos = player.transform.position;

        player.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z - _pushBackDistance);
    }
}
