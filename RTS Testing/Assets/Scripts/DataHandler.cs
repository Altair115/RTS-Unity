using UnityEngine;

public static class DataHandler
{
    public static void LoadGameData()
    {
        Globals.BUILDING_DATA = Resources.LoadAll<UnitData>("ScriptableObjects/Units/Buildings") as BuildingData[];
    }
}