using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingBlueprintPanel : MonoBehaviour
{
    public TextMeshProUGUI blueprintNameplate, description;
    public TextMeshProUGUI turnPlate;
    public Image blueprintImage;
    public SpriteTextPanel[] costs;
    public Button button;

    private BuildingBlueprint blueprint;
    private Player player;
    private BuildingSelectionOverview overview;

    public void Setup(BuildingBlueprint blueprint, Player player, BuildingSelectionOverview overview)
    {
        this.blueprint = blueprint;
        this.player = player;
        this.overview = overview;

        blueprintNameplate.text = blueprint.buildingName;
        if (description)
            description.text = blueprint.description;
        turnPlate.text = blueprint.turnsToComplete.ToString();
        blueprintImage.sprite = blueprint.uiSprite;

        for (int i = 0; i < costs.Length; i++)
        {
            bool show = i < blueprint.cost.Length;
            costs[i].SetActive(show);

            if (show)
            {
                costs[i].Setup(blueprint.cost[i].value.ToString(), ResourceSpriteDatabase.GetSprite(blueprint.cost[i].resourceType));
            }
        }
    }

    public void UpdateAffordable()
    {
        button.interactable = player.IsAffordable(blueprint.cost);
    }

    public void Clicked()
    {
        overview.SelectBlueprint(blueprint);
    }
}
