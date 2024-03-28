using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItemUI : MonoBehaviour
{
    [SerializeField] private Image _background;

    [SerializeField] private TMP_Text _playerPlace;
    [SerializeField] private Image _avatarImage;
    [SerializeField] private TMP_Text _playerNick;
    [SerializeField] private TMP_Text _playerScore;

    public void SetLeaderboardItem(Sprite avatar, string playerNick, string playerScore, int playerCount, bool isLocalPlayer)
    {
        if(isLocalPlayer)
        {
            _background.enabled = true;
        }

        _playerPlace.text = playerCount.ToString();
        _avatarImage.sprite = avatar;
        _playerNick.text = playerNick;
        _playerScore.text = playerScore;
    }
}
