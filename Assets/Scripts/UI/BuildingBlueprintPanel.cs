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

    public SpriteTextPanel requirement;
    public GameObject requirementPanel;

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

        if (blueprint.HasRequirement)
        {
            requirementPanel.SetActive(true);

            string status = blueprint.serviceBuildingRequirement.GetRequirementTextState(player);
            Sprite requirementSprite = TagSpriteDatabase.GetSprite(blueprint.serviceBuildingRequirement.type);
            requirement.Setup(status, requirementSprite);
        }
        else
        {
            requirementPanel.SetActive(false);
        }
        UpdateInteractable();
    }

    public void UpdateInteractable()
    {
        bool interactable = true;

        if (!player.IsAffordable(blueprint.cost))
            interactable = false;

        if (blueprint.HasRequirement)
            if (!blueprint.serviceBuildingRequirement.RequirementFullfilled(player))
                interactable = false;
        button.interactable = interactable;
    }

    public void Clicked()
    {
        overview.SelectBlueprint(blueprint);
    }
}
