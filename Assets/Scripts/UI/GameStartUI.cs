using TMPro;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    [SerializeField] private string _startText = "Go!";
    [SerializeField] private TMP_Text _timerText;

    [SerializeField] private GameObject _startCanvas;

    public void Initialize()
    {
        SetTimer(3);

        _startCanvas.SetActive(true);
    }

    public void SetTimer(int time)
    {
        if(time == 0)
        {
            _timerText.text = _startText;

        }
        else
        {
            _timerText.text = time.ToString();
        }
    }

    public void Deinitialize()
    {
        _startCanvas.SetActive(false);
    }
}
