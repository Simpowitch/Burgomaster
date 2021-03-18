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
    public SpriteTextPanel[] currentEffectPanels, currentIncomePanels, currentUpkeepPanels;

    public Button upgradeButton;
    public TextMeshProUGUI upgradeNamePlate;
    public Image upgradeImage;
    public SpriteTextPanel[] upgradeCostPanels, upgradeEffectPanels, upgradeIncomePanels, upgradeUpkeepPanels;

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

        UpdatePanels(currentEffectPanels, building.CurrentEffects);
        UpdatePanels(currentIncomePanels, building.Income);
        UpdatePanels(currentUpkeepPanels, building.Upkeep);

        upgradeButton.gameObject.SetActive(false);
    }

    public void SetupUpgradeable(Building building, UnityAction buttonAction, bool interactable, NotificationInformation levelUpNoficiation)
    {
        SetupDefault(building);

        upgradeNamePlate.text = building.NextLevelName;
        upgradeImage.sprite = building.NextLevelInspectorSprite;

        UpdatePanels(upgradeCostPanels, building.LevelUpCost);
        UpdatePanels(upgradeEffectPanels, building.NextLevelEffects);
        UpdatePanels(upgradeIncomePanels, building.NextLevelIncome);
        UpdatePanels(upgradeUpkeepPanels, building.NextLevelUpkeep);

        upgradeButton.gameObject.SetActive(true);
        Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();
        onClick.AddListener(buttonAction);
        UnityAction openNotificationEvent = () => SetupNotification(levelUpNoficiation);
        onClick.AddListener(openNotificationEvent);
        upgradeButton.onClick = onClick;
        upgradeButton.interactable = interactable;
    }

    void SetupNotification(NotificationInformation levelUpNoficiation) => notificationInspector.Setup(levelUpNoficiation);

    void UpdatePanels(SpriteTextPanel[] panels, Effect[] effects)
    {
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

    void UpdatePanels(SpriteTextPanel[] panels, Resource[] resources)
    {
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

    public void CloseAndDeselect() => Building.SelectedBuilding = null;
}
