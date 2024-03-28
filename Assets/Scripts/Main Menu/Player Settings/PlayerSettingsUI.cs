using TMPro;
using UnityEngine;

public class PlayerSettingsUI : MonoBehaviour
{
    [SerializeField] private Transform _avatarParentTransfrom;
    [SerializeField] private PlayerDataManager _dataManager;
    [SerializeField] private GameObject _avatarButtonPrefab;

    [SerializeField] private TMP_InputField _nameInput;

    private AvatarSO[] _allAvatars;

    public void OnConfirmPressed()
    {
        if (_nameInput.text.Length >= 3)
        {
            _dataManager.OnUsernameConfirmPressed(_nameInput.text);
        }
    }

    public void InitializPlayerSettings()
    {
        _allAvatars = _dataManager.Data.AllAvatars;
        _nameInput.text = _dataManager.UserNickname;

        foreach (AvatarSO avatar in _allAvatars)
        {
            SpawnOneItem(avatar);
        }
    }

    private void SpawnOneItem(AvatarSO avatar)
    {
        GameObject oneAvatar = Instantiate(_avatarButtonPrefab, _avatarParentTransfrom);
        AvatarItemUI avatarItemUI = oneAvatar.GetComponent<AvatarItemUI>();

        avatarItemUI.SetInformation(avatar);
        avatarItemUI.OnAvatarSelected += OnAvatarSelected;
    }

    private void OnAvatarSelected(AvatarSO avatar)
    {
        _dataManager.OnAvatarSelected(avatar.Id);
    }

    public void DeinitializePlayerSettings()
    {
        foreach (Transform child in _avatarParentTransfrom)
        {
            child.gameObject.GetComponent<AvatarItemUI>().OnAvatarSelected -= OnAvatarSelected;
            Destroy(child.gameObject);
        }
    }
}
