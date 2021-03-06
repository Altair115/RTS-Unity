using System.Collections.Generic;
using Managers;

public class Globals
{
    public static int TerrainLayerMask = 1 << 6;
    public static List<UnitManager> SELECTED_UNITS = new List<UnitManager>();
    public static BuildingData[] BUILDING_DATA; //Hatefull thing
    
    public static Dictionary<string, GameResource> GAME_RESOURCES = new Dictionary<string, GameResource>
    {
        { "gold", new GameResource("Gold", 300) },
        { "wood", new GameResource("Wood", 300) },
        { "stone", new GameResource("Stone", 300) }
    };
}