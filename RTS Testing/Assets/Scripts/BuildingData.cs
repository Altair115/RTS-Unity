public class BuildingData
{
   private string _code;
   private int _hitpoints;
   
   public BuildingData(string code, int healthpoints)
   {
      _code = code;
      _hitpoints = healthpoints;
   }

   public string Code { get => _code; }
   public int HP { get => _hitpoints; }
}
