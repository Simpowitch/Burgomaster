using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventChoiceUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI description = null;
    [SerializeField] Button button = null;
    [SerializeField] ResourcePanelUI[] costPanels = null;
    [SerializeField] TextMeshProUGUI skillcheckInfo = null;

    public void Show(bool value) => this.gameObject.SetActive(value);

    public void UpdateUI(Scourge.Option option, bool interactable)
    {
        description.text = option.optionText;
        button.interactable = interactable;

        skillcheckInfo.text = $"Difficulty: {option.checkType} - {option.challengeRating}";

        for (int i = 0; i < costPanels.Length; i++)
        {
            bool show = i < option.cost.Length;
            costPanels[i].Show(show);

            if (show)
            {
                costPanels[i].Setup(option.cost[i].resourceType);
                costPanels[i].UpdatePanel(option.cost[i].value);
            }
        }
    }
}
