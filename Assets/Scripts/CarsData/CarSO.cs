using UnityEngine;

[CreateAssetMenu(fileName = "Car Data", menuName = "Player/Car")]
public class CarSO : ScriptableObject
{
    [SerializeField] public int CarId;
    [SerializeField] public GameObject PlayerCar;
    [SerializeField] public GameObject MainMenuCar;
}
