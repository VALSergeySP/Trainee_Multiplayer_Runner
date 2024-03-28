using UnityEngine;

[CreateAssetMenu(fileName = "Player Avatar", menuName = "Player/Player Avatar")]
public class AvatarSO : ScriptableObject
{
    [SerializeField] private int _avatarId;
    public int Id { get => _avatarId; }

    [SerializeField] private Sprite _avatarSprite;
    public Sprite AvatarSprite { get => _avatarSprite; }
}
