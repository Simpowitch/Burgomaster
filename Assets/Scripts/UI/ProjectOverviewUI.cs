using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectOverviewUI : MonoBehaviour
{
    public ProjectPanelUI projectPanel_BP;
    public Transform projectParent;

    private List<ProjectPanelUI> allTab = new List<ProjectPanelUI>();
    private List<ProjectPanelUI> housingTab = new List<ProjectPanelUI>();
    private List<ProjectPanelUI> productionTab = new List<ProjectPanelUI>();
    private List<ProjectPanelUI> servicesTab = new List<ProjectPanelUI>();
    private List<ProjectPanelUI> uniqueTab = new List<ProjectPanelUI>();
    public Toggle allToggle;

    public ProjectInspector projectInspector;

    public void UpdateProjectList(List<Project> availableProjects, Player player)
    {
        foreach (var projectPanel in allTab)
        {
            Destroy(projectPanel.gameObject);
        }
        allTab.Clear();
        housingTab.Clear();
        productionTab.Clear();
        servicesTab.Clear();
        uniqueTab.Clear();

        allToggle.Select();

        for (int i = 0; i < availableProjects.Count; i++)
        {
            ProjectPanelUI newPanel = Instantiate(projectPanel_BP, projectParent);

            switch (availableProjects[i].category)
            {
                case Project.Category.Housing:
                    housingTab.Add(newPanel);
                    break;
                case Project.Category.Production:
                    productionTab.Add(newPanel);
                    break;
                case Project.Category.Service:
                    servicesTab.Add(newPanel);
                    break;
                case Project.Category.Unique:
                    uniqueTab.Add(newPanel);
                    break;
            }
            allTab.Add(newPanel);
            newPanel.Setup(availableProjects[i], player, this);
        }
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
            projectInspector.Show(false);
    }

    public void ShowAll(bool state) => ShowList(allTab, state);

    public void ShowHouseTab(bool state) => ShowList(housingTab, state);

    public void ShowProduction(bool state) => ShowList(productionTab, state);

    public void ShowServices(bool state) => ShowList(servicesTab, state);

    public void ShowUnique(bool state) => ShowList(uniqueTab, state);

    private void ShowList(List<ProjectPanelUI> list, bool show)
    {
        foreach (var item in list)
        {
            item.gameObject.SetActive(show);
        }
    }

    public void SelectedProject(Project selected)
    {
        projectInspector.Show(true);
        projectInspector.Setup(selected);
    }
}
