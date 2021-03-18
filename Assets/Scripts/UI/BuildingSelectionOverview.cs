using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectionOverview : MonoBehaviour
{
    public BuildingBlueprintPanel projectPanel_BP;
    public Transform viewportContent;
    public ThemeSelector themeSelector;

    private List<BuildingBlueprintPanel> allTab = new List<BuildingBlueprintPanel>();
    private List<BuildingBlueprintPanel> housingTab = new List<BuildingBlueprintPanel>();
    private List<BuildingBlueprintPanel> productionTab = new List<BuildingBlueprintPanel>();
    private List<BuildingBlueprintPanel> servicesTab = new List<BuildingBlueprintPanel>();
    private List<BuildingBlueprintPanel> uniqueTab = new List<BuildingBlueprintPanel>();
    public Toggle allToggle;

    public BuildingBlueprintInspector blueprintInspector;

    private Player player;

    public void UpdateProjectList(List<BuildingBlueprint> availableProjects, Player player)
    {
        this.player = player;
        foreach (var projectPanel in allTab)
        {
            Destroy(projectPanel.gameObject);
        }
        allTab.Clear();
        housingTab.Clear();
        productionTab.Clear();
        servicesTab.Clear();
        uniqueTab.Clear();


        for (int i = 0; i < availableProjects.Count; i++)
        {
            BuildingBlueprintPanel newPanel = Instantiate(projectPanel_BP, viewportContent);

            switch (availableProjects[i].category)
            {
                case BuildingBlueprint.Category.Housing:
                    housingTab.Add(newPanel);
                    break;
                case BuildingBlueprint.Category.Production:
                    productionTab.Add(newPanel);
                    break;
                case BuildingBlueprint.Category.Service:
                    servicesTab.Add(newPanel);
                    break;
                case BuildingBlueprint.Category.Unique:
                    uniqueTab.Add(newPanel);
                    break;
            }
            allTab.Add(newPanel);
            newPanel.Setup(availableProjects[i], player, this);
        }
        allToggle.Select();
    }

    public void UpdateAffordables()
    {
        foreach (var panel in allTab)
        {
            panel.UpdateAffordable();
        }
    }

    public void ShowUI(bool show)
    {
        this.gameObject.SetActive(show);

        if (!show)
            blueprintInspector.Show(false);
    }

    public void ShowAll(bool state) => ShowList(allTab, state);

    public void ShowHouseTab(bool state) => ShowList(housingTab, state);

    public void ShowProduction(bool state) => ShowList(productionTab, state);

    public void ShowServices(bool state) => ShowList(servicesTab, state);

    public void ShowUnique(bool state) => ShowList(uniqueTab, state);

    private void ShowList(List<BuildingBlueprintPanel> list, bool show)
    {
        foreach (var item in list)
        {
            item.gameObject.SetActive(show);
        }
    }

    public void SelectBlueprint(BuildingBlueprint selected)
    {
        blueprintInspector.Show(true);
        blueprintInspector.Setup(selected);
        player.SelectBlueprint(selected);
    }
}
