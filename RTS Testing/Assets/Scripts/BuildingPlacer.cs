using System;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingPlacer : MonoBehaviour
{
    private Building _placedBuilding = null;
    private Ray _ray;
    private RaycastHit _raycastHit;
    private Vector3 _lastPlacementPosition;

    public static event Action UpdateResourceTexts;
    public static event Action CheckBuildingButtons;
    

    private void Update()
    {
        if (_placedBuilding != null)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                _CancelPlacedBuilding();
                return;
            }

            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(
                    _ray,
                    out _raycastHit,
                    1000f,
                    Globals.TerrainLayerMask
                ))
            {
                _placedBuilding.SetPosition(_raycastHit.point);
                if (_lastPlacementPosition != _raycastHit.point)
                {
                    _placedBuilding.CheckValidPlacement();
                }

                _lastPlacementPosition = _raycastHit.point;
            }

            if (_placedBuilding.HasValidPlacement && Mouse.current.leftButton.wasReleasedThisFrame && !EventSystem.current.IsPointerOverGameObject())
            {
                _PlaceBuilding();
            }
        }
    }

    private void _PreparePlacedBuilding(int buildingDataIndex)
    {
        Building building = new Building(Globals.BUILDING_DATA[buildingDataIndex]);
        // link the data into the manager
        building.Transform.GetComponent<BuildingManager>().Initialize(building);
        _placedBuilding = building;
        _lastPlacementPosition = Vector3.zero;
    }
    
    void _PlaceBuilding()
    {
        _placedBuilding.Place();
        // keep on building the same building type
        if (_placedBuilding.CanBuy())
            _PreparePlacedBuilding(_placedBuilding.DataIndex);
        else
            _placedBuilding = null;
        
        UpdateResourceTexts?.Invoke();
        CheckBuildingButtons?.Invoke();
    }

    private void _CancelPlacedBuilding()
    {
        // destroy the "phantom" building
        Destroy(_placedBuilding.Transform.gameObject);
        _placedBuilding = null;
    }
    
    public void SelectPlacedBuilding(int buildingDataIndex)
    {
        _PreparePlacedBuilding(buildingDataIndex);
    }
}