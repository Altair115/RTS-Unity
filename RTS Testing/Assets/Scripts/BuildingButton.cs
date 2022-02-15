using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UnitData _buildingData;
    
    public static event Action<UnitData> HoverBuildingButton;
    public static event Action UnhoverBuildingButton;

    public void Initialize(UnitData buildingData)
    {
        _buildingData = buildingData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverBuildingButton?.Invoke(_buildingData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnhoverBuildingButton?.Invoke();
    }
}