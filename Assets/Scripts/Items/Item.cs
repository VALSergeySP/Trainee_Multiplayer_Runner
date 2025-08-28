using Fusion;
using UnityEngine;

public class Item : NetworkBehaviour
{
    protected const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            OnTrigger(other.gameObject);
        }
    }

    protected virtual void OnTrigger(GameObject player) { }
}
