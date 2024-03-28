using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectionUI : MonoBehaviour
{
    [SerializeField] private float _carMovementStep = 4f;
    [SerializeField] private float _allCarsMoveOffset = 0.9f;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private GameObject _carPointPrefab;
    [SerializeField] private CarSO[] _carsData;
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private float _carRotationAngle = -140f;

    private Vector3 _startPosition;

    public void InitializeCarSelectionMenu()
    {
        _startPosition = _startPoint.position;

        for (int i = 0; i < _carsData.Length; i++)
        {
            SpawnOneItem(_carsData[i]);
        }
    }

    private void SpawnOneItem(CarSO carData)
    {
        GameObject oneCar = Instantiate(_carPointPrefab, Vector3.zero, Quaternion.identity);
        CarItemUI carItemUI = oneCar.GetComponent<CarItemUI>();

        carItemUI.SetInformation(carData);
        carItemUI.OnCarSelected += OnCarSelected;

        Transform carModel = Instantiate(carData.MainMenuCar, oneCar.transform).transform;
        oneCar.transform.parent = _startPoint;
        oneCar.transform.localPosition = _carMovementStep * carData.CarId * Vector3.right;
        carModel.rotation = Quaternion.Euler(0, _carRotationAngle, 0);
    }

    public void OnScrollValue(float scrollValue)
    {
        _startPoint.position = _startPosition - _carMovementStep * scrollValue * _carsData.Length * _allCarsMoveOffset * Vector3.right;
    }

    private void OnCarSelected(CarSO carInfo)
    {
        Debug.Log(carInfo.CarId.ToString() + " selected");
    }

    public void DeinitializeCarSelectionMenu()
    {
        foreach(Transform child in _startPoint)
        {
            child.gameObject.GetComponent<CarItemUI>().OnCarSelected -= OnCarSelected;
            Destroy(child.gameObject);
        }
    }

}
