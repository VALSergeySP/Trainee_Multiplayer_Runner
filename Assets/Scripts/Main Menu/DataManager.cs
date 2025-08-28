using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private CarSO[] _allCars;
    public CarSO[] AllCars { get => _allCars; }
    [SerializeField] private AvatarSO[] _allAvatars;
    public AvatarSO[] AllAvatars { get => _allAvatars; }

    public GameObject GetCarModelById(int carId)
    {
        return _allCars[carId].MainMenuCar;
    }

    public GameObject GetCarPlayerById(int carId)
    {
        return _allCars[carId].PlayerCar;
    }

    public Sprite GetAvatarById(int avatarId)
    {
        return _allAvatars[avatarId].AvatarSprite;
    }
}
