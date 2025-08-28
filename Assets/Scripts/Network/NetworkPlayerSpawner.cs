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
            if (Runner.LocalPlayer.PlayerId == 1)
            {
                NetworkObject obj = Runner.Spawn(PlayerPrefab, new Vector3(-LevelStaticInfo.LaneSize, _groundOffset, 0), Quaternion.identity);
                Runner.SetPlayerObject(Runner.LocalPlayer, obj);
            } else
            {
                NetworkObject obj = Runner.Spawn(PlayerPrefab, new Vector3(LevelStaticInfo.LaneSize, _groundOffset, 0), Quaternion.identity);
                Runner.SetPlayerObject(Runner.LocalPlayer, obj);
            }
        }
    }
}
