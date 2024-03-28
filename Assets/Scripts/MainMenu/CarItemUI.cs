using System;
using UnityEngine;

public class CarItemUI : MonoBehaviour
{
    CarSO _carInfo;

    public event Action<CarSO> OnCarSelected;

    public void SetInformation(CarSO carInfo)
    {
        _carInfo = carInfo;
    }

    public void OnClick()
    {
        OnCarSelected?.Invoke(_carInfo);
    }
}
