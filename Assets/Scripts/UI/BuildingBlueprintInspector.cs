using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingBlueprintInspector : MonoBehaviour
{
    public GameObject mainParent;

    public TextMeshProUGUI currentNamePlate;
    public Image currentImage;
    public SpriteTextPanel[] costPanels, effectPanels, incomePanels, upkeepPanels;
    public GameObject costParentPanel, effectParentPanel, incomeParentPanel, upkeepParentPanel;


    public GameObject levelUpRequirementPanel;
    public SpriteTextPanel levelUpRequirement;

    public void Setup(BuildingBlueprint blueprint, Player player)
    {
        currentNamePlate.text = blueprint.buildingName;
        currentImage.sprite = blueprint.uiSprite;

        UpdatePanels(effectPanels, blueprint.completionEffects, effectParentPanel);
        UpdatePanels(incomePanels, blueprint.income, incomeParentPanel);
        UpdatePanels(upkeepPanels, blueprint.upkeep, upkeepParentPanel);
        UpdatePanels(costPanels, blueprint.cost, costParentPanel);

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
    }

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
}
