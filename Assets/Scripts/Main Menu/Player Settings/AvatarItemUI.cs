using System;
using UnityEngine;
using UnityEngine.UI;

public class AvatarItemUI : MonoBehaviour
{
    [SerializeField] private Image _avatarImage;
    AvatarSO _avatarInfo;

    public event Action<AvatarSO> OnAvatarSelected;

    public void SetInformation(AvatarSO newInfo)
    {
        _avatarInfo = newInfo;
        _avatarImage.sprite = _avatarInfo.AvatarSprite;
    }

    public void OnClick()
    {
        OnAvatarSelected?.Invoke(_avatarInfo);
    }
}
