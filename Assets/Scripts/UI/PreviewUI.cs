using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviewUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _playerNames;
    [SerializeField] private Image[] _playerImages;

    [SerializeField] private GameObject _previewCanvas;

    public void Initialize()
    {
        for(int i = 0; i < 2; i++)
        {
            _playerNames[i].text = $"Player test {i + 1}";
            //_playerImages[i].sprite = sprite;
        }

        _previewCanvas.SetActive(true);
    }

    public void Deinitialize()
    {
        _previewCanvas.SetActive(false);
    }
}
