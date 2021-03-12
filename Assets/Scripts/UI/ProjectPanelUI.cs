using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectPanelUI : MonoBehaviour
{
    public TextMeshProUGUI projectNamePlate, description;
    public TextMeshProUGUI turnPlate;
    public Image projectImage;
    public ResourcePanelUI[] costs;
    public Button button;

    private Project project;
    private Player player;

    public void Setup(Project project, Player player)
    {
        this.project = project;
        this.player = player;

        projectNamePlate.text = project.projectName;
        if (description)
            description.text = project.description;
        turnPlate.text = project.turnsToComplete.ToString();
        projectImage.sprite = project.sprite;

        for (int i = 0; i < costs.Length; i++)
        {
            bool show = i < project.cost.Length;
            costs[i].Show(show);

            if (show)
            {
                costs[i].Setup(project.cost[i].resourceType);
                costs[i].UpdatePanel(project.cost[i].value);
            }
        }
    }

    public void UpdateAffordable()
    {
        button.interactable = player.IsAffordable(project.cost);
    }

    public void Clicked()
    {
        player.SelectProject(project);
    }
}
