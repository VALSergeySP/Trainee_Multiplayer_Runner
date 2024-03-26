using Fusion;
using UnityEngine;

public class Obstacle : NetworkBehaviour
{
    protected const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            OnTrigger(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(PLAYER_TAG))
        {
            OnCollision(collision.gameObject);
        }
    }

    protected virtual void OnCollision(GameObject player) { }
    protected virtual void OnTrigger(GameObject player) { }
}
