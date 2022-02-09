﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private BuildingPlacer _buildingPlacer;
    
    [SerializeField]private Transform buildingMenu;
    [SerializeField]private GameObject buildingButtonPrefab;
    [SerializeField]private Transform resourcesUIParent;
    [SerializeField]private GameObject gameResourceDisplayPrefab;

    private Dictionary<string, TMP_Text> _resourceTexts;
    private Dictionary<string, Button> _buildingButtons;
    
    private void OnEnable()
    {
        
        BuildingPlacer.UpdateResourceTexts += UpdateResourceTexts;
        BuildingPlacer.CheckBuildingButtons += CheckBuildingButtons;
    }

    private void OnDisable()
    {
        BuildingPlacer.UpdateResourceTexts -= UpdateResourceTexts;
        BuildingPlacer.CheckBuildingButtons -= CheckBuildingButtons;
    }
    
    private void Awake()
    {
        _buildingPlacer = GetComponent<BuildingPlacer>();
        
        // create texts for each in-game resource (gold, wood, stone...)
        _resourceTexts = new Dictionary<string, TMP_Text>();
        foreach (KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES)
        {
            GameObject display = Instantiate(gameResourceDisplayPrefab, resourcesUIParent, true);
            display.name = pair.Key;
            _resourceTexts[pair.Key] = display.transform.Find("Text").GetComponent<TMP_Text>();
            _SetResourceText(pair.Key, pair.Value.Amount);
        }

        
        // create buttons for each building type
        _buildingButtons = new Dictionary<string, Button>();
        for (int i = 0; i < Globals.BUILDING_DATA.Length; i++)
        {
            BuildingData data = Globals.BUILDING_DATA[i];
            GameObject button = Instantiate(buildingButtonPrefab, buildingMenu, true);
            button.name = data.GetUnitName();
            button.transform.Find("Text").GetComponent<TMP_Text>().text = data.GetUnitName();
            Button buildButton = button.GetComponent<Button>();
            _AddBuildingButtonListener(buildButton, i);
            
            _buildingButtons[data.GetCode()] = buildButton;
            if (!Globals.BUILDING_DATA[i].CanBuy())
            {
                buildButton.interactable = false;
            }
        }
    }

    private void _AddBuildingButtonListener(Button b, int i)
    {
        b.onClick.AddListener(() => _buildingPlacer.SelectPlacedBuilding(i));
    }
    
    private void _SetResourceText(string resource, int value)
    {
        _resourceTexts[resource].text = value.ToString();
    }
    
    private void _OnUpdateResourceTexts()
    {
        foreach (KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES)
            _SetResourceText(pair.Key, pair.Value.Amount);
    }

    private void _OnCheckBuildingButtons()
    {
        foreach (BuildingData data in Globals.BUILDING_DATA)
            _buildingButtons[data.GetCode()].interactable = data.CanBuy();
    }

    public void UpdateResourceTexts()
    {
        foreach (KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES)
        {
            _SetResourceText(pair.Key, pair.Value.Amount);
        }
    }
    
    public void CheckBuildingButtons()
    {
        foreach (BuildingData data in Globals.BUILDING_DATA)
        {
            _buildingButtons[data.GetCode()].interactable = data.CanBuy();
        }
    }
}