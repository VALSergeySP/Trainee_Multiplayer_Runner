using Fusion;
using UnityEngine;

public class PlayerDataNetworked : NetworkBehaviour
{
    [HideInInspector]
    [Networked]
    public NetworkString<_16> NickName { get; private set; }

    [HideInInspector]
    [Networked]
    public float FinishTime { get; private set; }

    [HideInInspector]
    [Networked]
    public NetworkBool Finished { get; private set; }

    public override void Spawned()
    {

        // --- StateAuthority
        // Initialized game specific settings
        if (Object.HasStateAuthority)
        {
            FinishTime = 0; 
            Finished = false;
            NickName = Random.Range(0, 9999).ToString("0000");
        }
    }


    // Increase the score by X amount of points
    public void SetFinishTime(float newTime)
    {
        if (Object.HasStateAuthority)
        {
            FinishTime = newTime;
            Finished = true;
        }
    }

    // RPC used to send player information to the Host
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void RpcSetNickName(string nickName)
    {
        if (string.IsNullOrEmpty(nickName)) return;
        NickName = nickName;
    }
}
