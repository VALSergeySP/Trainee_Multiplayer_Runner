using Firebase.Database;
using System.Collections;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Firebase Data")]
    [SerializeField] private PlayerDataManager _playerData;
    private DatabaseReference _DBreference;

    [Header("LeaderboardFields")]
    [SerializeField] private GameObject _onePlayerLinePrefab;
    [SerializeField] private GameObject _leaderboardPanel;

    public void InitializeLeaderBoard()
    {
        _DBreference = _playerData.Database;

        ClearLeaderBoard();
        StartCoroutine(LoadLeaderboardData());
    }

    public void ClearLeaderBoard()
    {
        foreach(Transform child in _leaderboardPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InstantiateOneLine(DataSnapshot childSnapshot, int num, bool isLocal)
    {
        int avatarId = int.Parse(childSnapshot.Child("avatarId").Value.ToString());
        string username = childSnapshot.Child("username").Value.ToString();
        string score = childSnapshot.Child("maxscore").Value.ToString();

        GameObject item = Instantiate(_onePlayerLinePrefab, _leaderboardPanel.transform);

        if (item.TryGetComponent<LeaderboardItemUI>(out var itemUI) == false)
        {
            Debug.LogWarning("Error setting up data!");
        }
        else
        {
            itemUI.SetLeaderboardItem(_playerData.Data.GetAvatarById(avatarId), username, score, num, isLocal);
        }
    }

    private void SetupLeaderboard(DataSnapshot snapshot)
    {
        int count = 0;
        bool currentUserInTop = false;
        bool isLocalPlayer;

        foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
        {
            count++;

            isLocalPlayer = false;

            if (childSnapshot.Key.Equals(PlayerPrefs.GetString("USERID")))
            {
                currentUserInTop = true;
                isLocalPlayer = true;
            }

            InstantiateOneLine(childSnapshot, count, isLocalPlayer);

            if (count >= 5)
            {
                break;
            }
        }

        if (currentUserInTop == false)
        {
            Debug.Log("User is not in top 5!");
        }
    }

    private IEnumerator LoadLeaderboardData()
    {
        var DBTask = _DBreference.Child("users").OrderByChild("score").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to load data from task with {DBTask.Exception}");
        } else
        {
            DataSnapshot snapshot = DBTask.Result;

            SetupLeaderboard(snapshot);
        }
    }
}
