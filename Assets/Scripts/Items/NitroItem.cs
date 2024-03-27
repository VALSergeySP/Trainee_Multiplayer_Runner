using UnityEngine;

public class NitroItem : Item
{
    protected override void OnTrigger(GameObject player)
    {
        if(player.TryGetComponent<PlayerAcceleration>(out var acceleration))
        {
            acceleration.NitroCollected();
            gameObject.SetActive(false);
            Runner.Despawn(Object);
        }
    }
}
