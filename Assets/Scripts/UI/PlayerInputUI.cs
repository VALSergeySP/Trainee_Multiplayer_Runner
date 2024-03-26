using Fusion;

public class PlayerInputUI : NetworkBehaviour
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

    public void OnNitroPressed()
    {
        if(player  != null)
        {
            player.NitroPressed();
        } else
        {
            InitializePlayer();
        }
    }
}
