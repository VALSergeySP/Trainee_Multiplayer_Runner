using UnityEngine;

public class PlayerCollisionsHandler : MonoBehaviour
{
    [SerializeField] private float _onPlayersCollideSlowCoef = 0.5f;
    private const string PLAYER_TAG = "Player";


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(PLAYER_TAG))
        {
            //collision.gameObject.SendMessage("SlowSpeed", _onPlayersCollideSlowCoef);
            gameObject.GetComponent<PlayerMovement>().MoveBackOnLine();
            Debug.LogWarning("Collision with Player!");
        }
    }
}
