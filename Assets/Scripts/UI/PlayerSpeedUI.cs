using Fusion;

public class PlayerSpeedUI : NetworkBehaviour
{
    private PlayerAcceleration player;


    public void InitializePlayer()
    {
        if (Runner.TryGetPlayerObject(Runner.LocalPlayer, out var networkObject))
        {
            if (networkObject.TryGetComponent(out player))
            {
                player.NitroPressed();
            }
        }
    }

    public void OnSpeedChanged()
    {
        if (player != null)
        {
            player.NitroPressed();
        }
        else
        {
            InitializePlayer();
        }
    }
}
