using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor.PackageManager;
using UnityEditor.VersionControl;

public class AuthManager : MonoBehaviour
{
    private const string EMAIL_END = "@gmail.com";

    //Firebase variables
    [Header("Firebase")]
    private DependencyStatus _dependencyStatus;
    private FirebaseAuth _auth;
    private FirebaseUser _user;

    //Login variables
    [Header("Login")]
    [SerializeField] private TMP_InputField _loginLoginField;
    [SerializeField] private TMP_InputField _passwordLoginField;
    [SerializeField] private Toggle _rememberMeToggle;

    //Register variables
    [Header("Register")]
    [SerializeField] private TMP_InputField _loginRegisterField;
    [SerializeField] private TMP_InputField _usernameRegisterField;
    [SerializeField] private TMP_InputField _passwordRegisterField;
    [SerializeField] private TMP_InputField _passwordRegisterVerifyField;

    [SerializeField] private CanvasPopUpUI _popUpCanvas;

    public UnityEvent Success;
    public UnityEvent SuccessRegistration;

    private void Awake()
    {
        StartCoroutine(CheckAndFixDependenciesAsync());   
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyTask.IsCompleted);
        
        _dependencyStatus = dependencyTask.Result;

        if (_dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();

            yield return new WaitForEndOfFrame();

            StartCoroutine(CheckForAutoLogin());
        }
        else
        {
            Debug.LogError("Could not resolve all Firebase dependencies: " + _dependencyStatus);
        }
    }


    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        _auth = FirebaseAuth.DefaultInstance;

        _auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private IEnumerator CheckForAutoLogin()
    {
        int isRememberMe = PlayerPrefs.GetInt("RememberMe", 0);

        if (isRememberMe == 1)
        {
            if (_user != null)
            {
                var reloadUser = _user.ReloadAsync();

                yield return new WaitUntil(() => reloadUser.IsCompleted);

                AutoLogin();
            }
        }
    }

    private void AutoLogin()
    {
        if(_user != null)
        {
            Success?.Invoke();
        }
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (_auth.CurrentUser != _user)
        {
            bool signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null;
            if (!signedIn && _user != null)
            {
                Debug.Log("Signed out " + _user.UserId);
            }
            _user = _auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + _user.UserId);
            }
        }
    }

    private void OnDestroy()
    {
        _auth.StateChanged -= AuthStateChanged;
        _auth = null;
    }


    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the login and password
        StartCoroutine(Login(_loginLoginField.text + EMAIL_END, _passwordLoginField.text));
    }

    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the login, password, and username
        StartCoroutine(Register(_loginRegisterField.text + EMAIL_END, _passwordRegisterField.text, _usernameRegisterField.text));
    }

    private IEnumerator Login(string _login, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = _auth.SignInWithEmailAndPasswordAsync(_login, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Login";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Login";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }

            _popUpCanvas.ActivatePopUp(message);
        }
        else
        {
            //User is now logged in
            //Now get the result
            _user = LoginTask.Result.User;

            _popUpCanvas.ActivatePopUp($"User signed in successfully: {_user.DisplayName}");

            if(_rememberMeToggle.isOn)
            {
                PlayerPrefs.SetInt("RememberMe", 1);
            } else
            {
                PlayerPrefs.SetInt("RememberMe", 2);
            }

            Success?.Invoke();
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            _popUpCanvas.ActivatePopUp("Missing Username");
        } else if (_email == "")
        {
            _popUpCanvas.ActivatePopUp("Missing Login");
        }
        else if (_passwordRegisterField.text != _passwordRegisterVerifyField.text)
        {
            _popUpCanvas.ActivatePopUp("Password Does Not Match!");
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = _auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Login";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Login Already In Use";
                        break;
                }

                _popUpCanvas.ActivatePopUp(message);
            }
            else
            {
                //User has now been created
                //Now get the result
                _user = RegisterTask.Result.User;

                if (_user != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = _user.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        _popUpCanvas.ActivatePopUp("Username Set Failed!");
                    }
                    else
                    {
                        SuccessRegistration?.Invoke();
                        _popUpCanvas.ActivatePopUp("Success!\nNow you can log in.");
                    }
                }
            }
        }
    }
}