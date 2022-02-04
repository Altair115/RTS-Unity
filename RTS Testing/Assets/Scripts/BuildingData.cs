using System.Collections.Generic;

public class BuildingData
{
   private string _code;
   private int _hitpoints;
   private Dictionary<string, int> _cost;
   
   public BuildingData(string code, int healthpoints, Dictionary<string, int> cost)
    {
        _code = code;
        _hitpoints = healthpoints;
        _cost = cost;
    }
   
   public bool CanBuy()
   {
       foreach (KeyValuePair<string, int> pair in _cost)
       {
           if (Globals.GAME_RESOURCES[pair.Key].Amount < pair.Value)
           {
               return false;
           }
       }
       return true;
   }

   public string Code { get => _code; }
   public int HP { get => _hitpoints; }
   public Dictionary<string, int> Cost { get => _cost; }
}
