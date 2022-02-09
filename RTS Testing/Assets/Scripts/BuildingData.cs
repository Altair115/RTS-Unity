using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Objects/Building", order = 1)]
public class BuildingData : ScriptableObject
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

   public string GetDiscription()
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
