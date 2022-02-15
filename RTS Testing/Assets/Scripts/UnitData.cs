using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Objects/Unit", order = 1)]
public class UnitData : ScriptableObject
{
   [SerializeField]private string code;
   [SerializeField]private string unitName;
   [SerializeField]private string description;
   [SerializeField]private int hitpoints;
   [SerializeField]private GameObject prefab;
   [SerializeField]private List<ResourceValue> cost;

   public string GetCode()
   {
       return code;
   }

   public string GetUnitName()
   {
       return unitName;
   }

   public string GetDescription()
   {
       return description;
   }

   public int GetHitpoints()
   {
       return hitpoints;
   }

   public GameObject GetPrefab()
   {
       return prefab;
   }

   public List<ResourceValue> GetCost()
   {
       return cost;
   }

   public bool CanBuy()
   {
       foreach (ResourceValue resource in cost)
           if (Globals.GAME_RESOURCES[resource.code].Amount < resource.amount)
               return false;
       return true;
   }
}
