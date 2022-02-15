using System;
using System.Collections.Generic;
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
    [SerializeField]private GameObject gameResourceCostPrefab;
    [SerializeField]private GameObject infoPanel;
    
    private TMP_Text _infoPanelTitleText;
    private TMP_Text _infoPanelDescriptionText;
    private Transform _infoPanelResourcesCostParent;

    private Dictionary<string, TMP_Text> _resourceTexts;
    private Dictionary<string, Button> _buildingButtons;
    
    private void OnEnable()
    {
        BuildingPlacer.UpdateResourceTexts += UpdateResourceTexts;
        BuildingPlacer.CheckBuildingButtons += CheckBuildingButtons;

        BuildingButton.HoverBuildingButton += _OnHoverBuildingButton;
        BuildingButton.UnhoverBuildingButton += _OnUnhoverBuildingButton;
    }

    private void OnDisable()
    {
        BuildingPlacer.UpdateResourceTexts -= UpdateResourceTexts;
        BuildingPlacer.CheckBuildingButtons -= CheckBuildingButtons;
        
        BuildingButton.HoverBuildingButton -= _OnHoverBuildingButton;
        BuildingButton.UnhoverBuildingButton -= _OnUnhoverBuildingButton;
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
            UnitData data = Globals.BUILDING_DATA[i];
            GameObject button = Instantiate(buildingButtonPrefab, buildingMenu, true);
            button.name = data.GetUnitName();
            button.transform.Find("Text").GetComponent<TMP_Text>().text = data.GetUnitName();
            Button buildButton = button.GetComponent<Button>();
            button.GetComponent<BuildingButton>().Initialize(Globals.BUILDING_DATA[i]);
            _AddBuildingButtonListener(buildButton, i);
            
            _buildingButtons[data.GetCode()] = buildButton;
            if (!Globals.BUILDING_DATA[i].CanBuy())
            {
                buildButton.interactable = false;
            }
            
            
        }
        
        Transform infoPanelTransform = infoPanel.transform;
        _infoPanelTitleText = infoPanelTransform.Find("Panel_Content/Text_Title").GetComponent<TMP_Text>();
        _infoPanelDescriptionText = infoPanelTransform.Find("Panel_Content/Text_Description").GetComponent<TMP_Text>();
        _infoPanelResourcesCostParent = infoPanelTransform.Find("Panel_Content/Panel_ResourcesCost");
        ShowInfoPanel(false);
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
        foreach (UnitData data in Globals.BUILDING_DATA)
            _buildingButtons[data.GetCode()].interactable = data.CanBuy();
    }
    
    private void _OnHoverBuildingButton(UnitData data)
    {
        SetInfoPanel(data);
        ShowInfoPanel(true);
    }

    private void _OnUnhoverBuildingButton()
    {
        ShowInfoPanel(false);
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
        foreach (UnitData data in Globals.BUILDING_DATA)
        {
            _buildingButtons[data.GetCode()].interactable = data.CanBuy();
        }
    }
    
    public void SetInfoPanel(UnitData data)
    {
        // update texts
        if (data.GetCode() != "") _infoPanelTitleText.text = data.GetCode();
        if (data.GetDescription() != "")  _infoPanelDescriptionText.text = data.GetDescription();
        // clear resource costs and reinstantiate new ones
        foreach (Transform child in _infoPanelResourcesCostParent) Destroy(child.gameObject);
        if (data.GetCost().Count > 0)
        {
            GameObject g; Transform t;
            foreach (ResourceValue resource in data.GetCost())
            {
                g = Instantiate(gameResourceCostPrefab) as GameObject;
                t = g.transform;
                t.Find("Text").GetComponent<TMP_Text>().text = resource.amount.ToString();
                //t.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>($"Textures/GameResources/{resource.code}");
                t.SetParent(_infoPanelResourcesCostParent);
                // check to see if resource requirement is not
                // currently met - in that case, turn the text into the "invalid"
                // color
                if (Globals.GAME_RESOURCES[resource.code].Amount < resource.amount)
                    t.Find("Text").GetComponent<TMP_Text>().color = Color.red;
            }
        }
    }

    public void ShowInfoPanel(bool show)
    {
        infoPanel.SetActive(show);
    }
}