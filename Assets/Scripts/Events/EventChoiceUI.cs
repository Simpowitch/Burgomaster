using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventChoiceUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI description = null;
    [SerializeField] Button button = null;
    [SerializeField] SpriteTextPanel[] costPanels = null;
    [SerializeField] SpriteTextPanel skillcheckInfo = null;

    public void Show(bool value) => this.gameObject.SetActive(value);

    public void UpdateUI(Scourge.Option option, bool interactable)
    {
        description.text = option.optionText;
        button.interactable = interactable;

        //Challenge rating
        string challengetext = $"Difficulty: {option.checkType} - {option.challengeRating}";
        Sprite challengeSprite = AbilityScoreSpriteDatabase.GetSprite(option.checkType);

        skillcheckInfo.Setup(challengetext, challengeSprite);

        //Costs
        for (int i = 0; i < costPanels.Length; i++)
        {
            bool show = i < option.cost.Length;
            costPanels[i].SetActive(show);

            if (show)
            {
                string text = option.cost[i].value.ToString();
                Sprite sprite = ResourceSpriteDatabase.GetSprite(option.cost[i].resourceType);

                costPanels[i].Setup(text , sprite);
            }
        }
    }
}
