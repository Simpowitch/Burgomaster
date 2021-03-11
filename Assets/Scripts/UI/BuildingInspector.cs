using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildingInspector : MonoBehaviour
{
    public GameObject mainParent;
    public TextMeshProUGUI namePlate;
    public Image mainImage;
    public ClampUIToCanvasSpace UIToCanvasSpace;
    public Button button;

    public SpriteTextPanel[] currentEffectPanels, currentUpkeepPanels, currentIncomePanels;
    public SpriteTextPanel[] upgradeCostPanels, updradeEffectPanels;

    public static BuildingInspector instance;

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


    public void SetupDefault(Transform follow, string name, Sprite image, Effect[] effects, Resource[] incomes, Resource[] upkeep)
    {
        UIToCanvasSpace.SetupFollowTransform(follow);

        namePlate.text = name;
        mainImage.sprite = image;

        for (int i = 0; i < currentEffectPanels.Length; i++)
        {
            bool show = i < effects.Length;

            currentEffectPanels[i].SetActive(show);
            if (show)
            {
                currentEffectPanels[i].Setup(effects[i].ToString(), effects[i].Sprite);
            }
        }

        for (int i = 0; i < currentIncomePanels.Length; i++)
        {
            bool show = i < incomes.Length;

            currentIncomePanels[i].SetActive(show);
            if (show)
            {
                currentIncomePanels[i].Setup(incomes[i].value.ToString(), incomes[i].Sprite);
            }
        }

        for (int i = 0; i < currentUpkeepPanels.Length; i++)
        {
            bool show = i < upkeep.Length;

            currentUpkeepPanels[i].SetActive(show);
            if (show)
            {
                currentUpkeepPanels[i].Setup(upkeep[i].value.ToString(), upkeep[i].Sprite);
            }
        }

        button.gameObject.SetActive(false);
    }

    public void SetupUpgradeable(Transform follow, string name, Sprite image, Effect[] effects, Resource[] incomes, Resource[] upkeep, UnityAction buttonAction, Resource[] actionCost, bool interactable)
    {
        SetupDefault(follow, name, image, effects, incomes, upkeep);

        for (int i = 0; i < upgradeCostPanels.Length; i++)
        {
            bool show = i < actionCost.Length;

            upgradeCostPanels[i].SetActive(show);
            if (show)
            {
                upgradeCostPanels[i].Setup(actionCost[i].ToString(), actionCost[i].Sprite);
            }
        }

        button.gameObject.SetActive(true);
        Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();
        onClick.AddListener(buttonAction);
        button.onClick = onClick;
        button.interactable = interactable;
    }

    public void Show(bool value) => mainParent.SetActive(value);
}
