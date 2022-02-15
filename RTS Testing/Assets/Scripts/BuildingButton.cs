using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private BuildingData _buildingData;
    
    public static event Action<BuildingData> HoverBuildingButton;
    public static event Action UnhoverBuildingButton;

    public void Initialize(BuildingData buildingData)
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