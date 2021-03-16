using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PropBlueprintPanel : MonoBehaviour
{
    public TextMeshProUGUI blueprintNameplate;
    public Image blueprintImage;

    private PropBlueprint blueprint;
    private PropSelectionOverview overview;

    public void Setup(PropBlueprint blueprint, PropSelectionOverview overview)
    {
        this.blueprint = blueprint;
        this.overview = overview;

        blueprintNameplate.text = blueprint.propName;
        blueprintImage.sprite = blueprint.uiSprite;
    }

    public void SetActive(bool value) => this.gameObject.SetActive(value);

    public void Clicked()
    {
        overview.SelectBlueprint(blueprint);
    }
}

