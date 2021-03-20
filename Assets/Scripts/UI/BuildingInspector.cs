using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildingInspector : MonoBehaviour
{
    public static BuildingInspector instance;

    public GameObject mainParent;
    public ClampUIToCanvasSpace UIToCanvasSpace;

    public TextMeshProUGUI currentNamePlate;
    public Image currentImage;
    public SpriteTextPanel[] currentEffectPanels, currentIncomePanels, currentUpkeepPanels, demolishRefundPanels;
    public GameObject currentEffectParentPanel, currentIncomeParentPanel, currentUpkeepParentPanel, currentDemolishParentPanel;

    public Button upgradeButton, demolishButton;
    public TextMeshProUGUI upgradeNamePlate;
    public Image upgradeImage;
    public SpriteTextPanel[] upgradeCostPanels, upgradeEffectPanels, upgradeIncomePanels, upgradeUpkeepPanels;
    public GameObject upgradeCostParentPanel, upgradeEffectParentPanel, upgradeIncomeParentPanel, upgradeUpkeepParentPanel;

    public GameObject levelUpRequirementPanel;
    public SpriteTextPanel levelUpRequirement;

    public NotificationInspector notificationInspector;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            Debug.Log($"Tried to instantiate a second {this}");
        }
        else
            instance = this;
    }

    public void SetupDefault(Building building)
    {
        UIToCanvasSpace.SetupFollowTransform(building.transform);

        currentNamePlate.text = building.MyName;
        currentImage.sprite = building.InspectorSprite;

        UpdatePanels(currentEffectPanels, building.CurrentEffects, currentEffectParentPanel);
        UpdatePanels(currentIncomePanels, building.Income, currentIncomeParentPanel);
        UpdatePanels(currentUpkeepPanels, building.Upkeep, currentUpkeepParentPanel);
        UpdatePanels(demolishRefundPanels, building.DemolishRefund, currentDemolishParentPanel);

        Button.ButtonClickedEvent demolishClickEvent = new Button.ButtonClickedEvent();
        demolishClickEvent.AddListener(building.Despawn);
        demolishClickEvent.AddListener(CloseAndDeselect);
        demolishButton.onClick = demolishClickEvent;

        upgradeButton.gameObject.SetActive(false);
    }

    public void SetupUpgradeable(Building building, UnityAction upgradeAction, bool interactable, NotificationInformation levelUpNoficiation, Player player)
    {
        SetupDefault(building);

        upgradeNamePlate.text = building.NextLevelName;
        upgradeImage.sprite = building.NextLevelInspectorSprite;

        UpdatePanels(upgradeCostPanels, building.LevelUpCost, upgradeCostParentPanel);
        UpdatePanels(upgradeEffectPanels, building.NextLevelEffects, upgradeEffectParentPanel);
        UpdatePanels(upgradeIncomePanels, building.NextLevelIncome, upgradeIncomeParentPanel);
        UpdatePanels(upgradeUpkeepPanels, building.NextLevelUpkeep, upgradeUpkeepParentPanel);

        BuildingBlueprint blueprint = building.NextLevelBlueprint;

        if (blueprint.HasRequirement)
        {
            levelUpRequirementPanel.SetActive(true);

            string status = blueprint.serviceBuildingRequirement.GetRequirementTextState(player);
            Sprite requirementSprite = TagSpriteDatabase.GetSprite(blueprint.serviceBuildingRequirement.type);
            levelUpRequirement.Setup(status, requirementSprite);
        }
        else
        {
            levelUpRequirementPanel.SetActive(false);
        }

        upgradeButton.gameObject.SetActive(true);
        Button.ButtonClickedEvent upgradeClickEvent = new Button.ButtonClickedEvent();
        upgradeClickEvent.AddListener(upgradeAction);
        UnityAction openNotificationEvent = () => SetupNotification(levelUpNoficiation);
        upgradeClickEvent.AddListener(openNotificationEvent);
        upgradeButton.onClick = upgradeClickEvent;
        upgradeButton.interactable = interactable;
    }

    void SetupNotification(NotificationInformation levelUpNoficiation) => notificationInspector.Setup(levelUpNoficiation);

    void UpdatePanels(SpriteTextPanel[] panels, Effect[] effects, GameObject parentPanel)
    {
        parentPanel.SetActive(effects != null && effects.Length > 0);

        for (int i = 0; i < panels.Length; i++)
        {
            bool show = i < effects.Length;

            panels[i].SetActive(show);
            if (show)
            {
                panels[i].Setup(effects[i].ToString(), effects[i].Sprite);
            }
        }
    }

    void UpdatePanels(SpriteTextPanel[] panels, Resource[] resources, GameObject parentPanel)
    {
        parentPanel.SetActive(resources != null && resources.Length > 0);

        for (int i = 0; i < panels.Length; i++)
        {
            bool show = i < resources.Length;

            panels[i].SetActive(show);
            if (show)
            {
                panels[i].Setup(resources[i].ToString(), resources[i].Sprite);
            }
        }
    }

    public void Show(bool value) => mainParent.SetActive(value);

    public void CloseAndDeselect()
    {
        Building.SelectedBuilding = null;
        Show(false);
    }
}
