using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataManager : MonoBehaviour
{
    private const string PREFS_USER_ID = "USERID";

    private const string BD_NAME = "users";
    private const string BD_PLAYER_USERNAME = "username";
    private const string BD_PLAYER_AVATAR_ID = "avatarId";
    private const string BD_PLAYER_CAR_ID = "carId";
    private const string BD_PLAYER_MAXSCORE = "maxscore";

    public UnityEvent PlayerDataLoaded;


    [SerializeField] private DataManager _dataManager;
    public DataManager Data { get => _dataManager; }

    private DatabaseReference _DBreference;
    public DatabaseReference Database { get => _DBreference; }
    private FirebaseUser _user;
    public FirebaseUser CurrentUser { get => _user; }

    private string _userNickname;
    public string UserNickname { get => _userNickname; }

    private int _userAvatar;
    public int UserAvatar { get => _userAvatar; }

    private int _userCar;
    public int UserCar { get => _userCar; }

    private string _userScore;
    public string UserScore { get => _userScore; }


    private void Awake()
    {
        InitializeFirebase();
    }

    private void Start()
    {
        StartCoroutine(LoadCurrentUserData());
    }

    public void OnAvatarSelected(int id)
    {
        StartCoroutine(UpdateAvatarIdDatabase(id));
    }

    public void OnCarSelected(int id)
    {
        StartCoroutine(UpdateCarIdDatabase(id));
    }

    public void OnUsernameConfirmPressed(string name)
    {
        StartCoroutine(UpdateUsernameDatabase(name));
    }

    public void OnRaceFinished(float score)
    {
        StartCoroutine(UpdateScoreDatabase(score));
    }

    private void InitializeFirebase()
    {
        _user = FirebaseAuth.DefaultInstance.CurrentUser;
        PlayerPrefs.SetString(PREFS_USER_ID, _user.UserId);

        if (_user == null)
        {
            Debug.Log("User Error!");
        }

        _DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        Debug.Log("Setting up Firebase Database");
    }

    private IEnumerator UpdateAvatarIdDatabase(int id)
    {
        var DBTask = _DBreference.Child(BD_NAME).Child(_user.UserId).Child(BD_PLAYER_AVATAR_ID).SetValueAsync(id);

        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log($"Avatar was setted to: {id}");
        }
    }

    private IEnumerator UpdateCarIdDatabase(int id)
    {
        var DBTask = _DBreference.Child(BD_NAME).Child(_user.UserId).Child(BD_PLAYER_CAR_ID).SetValueAsync(id);

        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log($"Car was setted to: {id}");
        }
    }

    private IEnumerator UpdateScoreDatabase(float score) 
    {
        var DBGetTask = _DBreference.Child(BD_NAME).Child(_user.UserId).Child(BD_PLAYER_MAXSCORE).GetValueAsync();

        yield return new WaitUntil(() => DBGetTask.IsCompleted);

        if (DBGetTask.Exception != null)
        {
            Debug.LogWarning($"Failed to load data from task with {DBGetTask.Exception}");
        }
        else if (DBGetTask.Result.Value != null)
        {
            float snapshotScore = float.Parse(DBGetTask.Result.Value.ToString());

            if (score < snapshotScore)
            {
                score = snapshotScore;
            }
        }

        var DBTask = _DBreference.Child(BD_NAME).Child(_user.UserId).Child(BD_PLAYER_MAXSCORE).SetValueAsync(score);

        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log($"Score was setted to: {score}");
        }
    }


    private IEnumerator LoadCurrentUserData()
    {
        var DBTask = _DBreference.Child(BD_NAME).Child(_user.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to load data from task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            _userNickname = _user.DisplayName;
            _userCar = 0;
            _userAvatar = 0;
            _userScore = "0.0";

            SignUpDataInitialization(_user.DisplayName);
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            _userNickname = snapshot.Child(BD_PLAYER_USERNAME).Value.ToString(); ;
            _userCar = int.Parse(snapshot.Child(BD_PLAYER_CAR_ID).Value.ToString());
            _userAvatar = int.Parse(snapshot.Child(BD_PLAYER_AVATAR_ID).Value.ToString());
            _userScore = snapshot.Child(BD_PLAYER_MAXSCORE).Value.ToString();
        }

        PlayerDataLoaded?.Invoke();
    }

    IEnumerator UpdateUsernameDatabase(string username)
    {
        var DBTask = _DBreference.Child(BD_NAME).Child(_user.UserId).Child(BD_PLAYER_USERNAME).SetValueAsync(username);

        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {DBTask.Exception}");
        }
    }

    public void SignUpDataInitialization(string name)
    {
        StartCoroutine(UpdateUsernameDatabase(name));
        StartCoroutine(UpdateScoreDatabase(0f));
        StartCoroutine(UpdateAvatarIdDatabase(0));
        StartCoroutine(UpdateCarIdDatabase(0));
    }
}
