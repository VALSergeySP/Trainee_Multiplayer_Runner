using UnityEngine;

public class FinishScript : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PLAYER_TAG))
        {
            other.GetComponent<PlayerDataNetworked>().SetFinishTime(0);
        }
    }
}
