using System.Collections.Generic;
using UnityEngine;

public class ProjectOverviewUI : MonoBehaviour
{
    public ProjectPanelUI projectPanel_BP;
    private List<ProjectPanelUI> projectPanels = new List<ProjectPanelUI>();

    public void UpdateProjectList(Project[] availableProjects, Player player)
    {
        foreach (var projectPanel in projectPanels)
        {
            Destroy(projectPanel.gameObject);
        }
        projectPanels.Clear();

        for (int i = 0; i < availableProjects.Length; i++)
        {
            ProjectPanelUI newPanel = Instantiate(projectPanel_BP, this.transform);
            projectPanels.Add(newPanel);
            newPanel.Setup(availableProjects[i], player);
        }
    }

    public void UpdateAffordables()
    {
        foreach (var panel in projectPanels)
        {
            panel.UpdateAffordable();
        }
    }

    public void ShowUI(bool show)
    {
        this.gameObject.SetActive(show);
    }
}
