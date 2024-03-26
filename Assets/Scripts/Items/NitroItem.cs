using UnityEngine;

public class NitroItem : Item
{
    protected override void OnTrigger(GameObject player)
    {
        if(player.TryGetComponent<PlayerAcceleration>(out var acceleration))
        {
            acceleration.NitroCollected();
            Runner.Despawn(Object);
        }
    }
}
