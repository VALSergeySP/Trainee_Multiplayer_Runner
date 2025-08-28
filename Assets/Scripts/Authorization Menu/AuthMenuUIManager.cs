using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthMenuUIManager : MonoBehaviour
{
    private const string MAINMENU_SCENE_NAME = "MainMenu";

    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _authCanvas;
    [SerializeField] private GameObject _registrCanvas;

    private void Awake()
    {
        ResetAllCanvases();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        if (PlayerPrefs.GetInt("RememberMe", 0) == 0)
        {
            _mainCanvas.SetActive(true);
        }
        else
        {
            _authCanvas.SetActive(true);
        }
    }

    public void OnAuthorizationGame()
    {
        SceneManager.LoadScene(MAINMENU_SCENE_NAME);
    }

    public void OnAuthorization()
    {
        ResetAllCanvases();
        _authCanvas.SetActive(true);
    }

    public void OnRegistration()
    {
        ResetAllCanvases();
        _registrCanvas.SetActive(true);
    }

    private void ResetAllCanvases()
    {
        _mainCanvas.SetActive(false);
        _registrCanvas.SetActive(false);
        _authCanvas.SetActive(false);
    }
}
