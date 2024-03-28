using TMPro;
using UnityEngine;

public class CanvasPopUpUI : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private TMP_Text _textField;

    public void OnButtonClick()
    {
        _canvas.SetActive(false);
    }

    public void ActivatePopUp(string error)
    {
        _textField.text = error;
        _canvas.SetActive(true);
    }
}
