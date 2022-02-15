using System.Collections.Generic;
using Managers;
using UnityEngine;

public enum BuildingPlacement
{
    Valid,
    Invalid,
    Fixed
};

public class Building : Unit
{

    private BuildingManager _buildingManager;
    private BuildingPlacement _placement;
    private List<Material> _materials;
    
    
    public Building(BuildingData data) : this(data, new List<ResourceValue>() { }) { }
    public Building(BuildingData data, List<ResourceValue> production) : base(data, production)
    {
        _buildingManager = _transform.GetComponent<BuildingManager>();
        _materials = new List<Material>();
        foreach (Material material in _transform.Find("Mesh").GetComponent<Renderer>().materials)
        {
            _materials.Add(new Material(material));
        }

        _placement = BuildingPlacement.Valid;
        SetMaterials();
    }

    public void SetMaterials() { SetMaterials(_placement); }
    public void SetMaterials(BuildingPlacement placement)
    {
        List<Material> materials;
        if (placement == BuildingPlacement.Valid)
        {
            Material refMaterial = Resources.Load("Materials/Valid") as Material;
            materials = new List<Material>();
            for (int i = 0; i < _materials.Count; i++)
                materials.Add(refMaterial);
        }
        else if (placement == BuildingPlacement.Invalid)
        {
            Material refMaterial = Resources.Load("Materials/Invalid") as Material;
            materials = new List<Material>();
            for (int i = 0; i < _materials.Count; i++)
                materials.Add(refMaterial);
        }
        else if (placement == BuildingPlacement.Fixed)
            materials = _materials;
        else
            return;
        _transform.Find("Mesh").GetComponent<Renderer>().materials = materials.ToArray();
    }

    public override void Place()
    {
        base.Place();
        // set placement state
        _placement = BuildingPlacement.Fixed;
        // change building materials
        SetMaterials();
    }

    public void CheckValidPlacement()
    {
        if (_placement == BuildingPlacement.Fixed) return;
        _placement = _buildingManager.CheckPlacement()
            ? BuildingPlacement.Valid
            : BuildingPlacement.Invalid;
    }

    public bool HasValidPlacement { get => _placement == BuildingPlacement.Valid; }
    public bool IsFixed { get => _placement == BuildingPlacement.Fixed; }
    public int DataIndex
    {
        get
        {
            for (int i = 0; i < Globals.BUILDING_DATA.Length; i++)
            {
                if (Globals.BUILDING_DATA[i].GetCode() == _data.GetCode())
                    return i;
            }
            return -1;
        }
    }
}