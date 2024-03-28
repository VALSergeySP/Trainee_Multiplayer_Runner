using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    private const string GAME_SCENE_NAME = "SampleScene";

    [SerializeField] private GameObject _playerCar;

    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private Transform _mainMenuCameraPosition;
    [SerializeField] private GameObject _carSelectionCanvas;
    [SerializeField] private Transform _carSelectionCameraPosition;
    [SerializeField] private GameObject _playerSettingsCanvas;
    [SerializeField] private GameObject _leaderboardCanvas;

    private Camera _mainCamera;
    [SerializeField] private float _moveSpeed = 5f;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _mainCamera.transform.SetPositionAndRotation(_mainMenuCameraPosition.position, _mainMenuCameraPosition.rotation);

        ResetAllCanvases();
        _mainMenuCanvas.SetActive(true);
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }

    public void OnCarSelect()
    {
        _playerCar.SetActive(false);
        ResetAllCanvases();
        StartCoroutine(CameraMoveRoutine(_carSelectionCameraPosition));
        _carSelectionCanvas.SetActive(true);
    }

    public void OnMainMenu()
    {
        _playerCar.SetActive(true);
        ResetAllCanvases();
        if(_mainCamera.transform != _mainMenuCameraPosition)
            StartCoroutine(CameraMoveRoutine(_mainMenuCameraPosition));
        _mainMenuCanvas.SetActive(true);
    }

    public void OnLeaderboardMenu()
    {
        ResetAllCanvases();
        _leaderboardCanvas.SetActive(true);
    }

    public void OnPlayerSettings()
    {
        ResetAllCanvases();
        _playerSettingsCanvas.SetActive(true);
    }

    private void ResetAllCanvases()
    {
        StopAllCoroutines();
        _mainMenuCanvas.SetActive(false);
        _carSelectionCanvas.SetActive(false);
        _playerSettingsCanvas.SetActive(false);
        _leaderboardCanvas.SetActive(false);
    }


    private IEnumerator CameraMoveRoutine(Transform target)
    {
        while (true)
        {
            _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, target.position, _moveSpeed * Time.deltaTime);
            _mainCamera.transform.rotation = Quaternion.Slerp(_mainCamera.transform.rotation, target.rotation, _moveSpeed * Time.deltaTime);

            if(_mainCamera.transform.position == target.position)
            {
                break;
            }

            yield return null;
        }

        _mainCamera.transform.SetPositionAndRotation(target.position, target.rotation);
    } 
}
