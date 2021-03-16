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

    public void Setup(BuildingBlueprint blueprint)
    {
        currentNamePlate.text = blueprint.buildingName;
        currentImage.sprite = blueprint.uiSprite;

        UpdatePanels(effectPanels, blueprint.completionEffects);
        UpdatePanels(incomePanels, blueprint.income);
        UpdatePanels(upkeepPanels, blueprint.upkeep);
        UpdatePanels(costPanels, blueprint.cost);
    }

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
}
