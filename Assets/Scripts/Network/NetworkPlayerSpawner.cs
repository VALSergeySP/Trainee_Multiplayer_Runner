using Fusion;
using UnityEngine;

public class NetworkPlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private float _groundOffset = 0.1f;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, new Vector3(0, _groundOffset, 0), Quaternion.identity);
        }
    }
}
