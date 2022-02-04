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
            GameObject button = Instantiate(buildingButtonPrefab, buildingMenu, true);
            string code = Globals.BUILDING_DATA[i].Code;
            button.name = code;
            button.transform.Find("Text").GetComponent<TMP_Text>().text = code;
            Button buildButton = button.GetComponent<Button>();
            _AddBuildingButtonListener(buildButton, i);
            
            _buildingButtons[code] = buildButton;
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
            _buildingButtons[data.Code].interactable = data.CanBuy();
        }
    }
}