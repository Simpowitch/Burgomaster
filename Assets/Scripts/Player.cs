using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Resource[] startResources;

    Resource[] resources;
    Resource[] incomes;
    Resource[] expenses;

    public Project[] availableProjects;

    public ResourcePanelUI[] resourcePanels;
    public ProjectOverviewUI projectOverview;
    public BuildingManager buildingManager;

    private void Start()
    {
        resources = new Resource[(int)ResourceType.MAX];
        for (int i = 0; i < resources.Length; i++)
        {
            Resource startResource = Resource.GetResourceByType(startResources, (ResourceType)i);
            int startValue = startResource != null ? startResource.value : 0;
            resources[i] = new Resource((ResourceType)i, startValue);
        }

        incomes = new Resource[(int)ResourceType.MAX];
        for (int i = 0; i < incomes.Length; i++)
        {
            incomes[i] = new Resource((ResourceType)i);
        }

        expenses = new Resource[(int)ResourceType.MAX];
        for (int i = 0; i < expenses.Length; i++)
        {
            expenses[i] = new Resource((ResourceType)i);
        }

        UpdateEconomyUI();

        projectOverview.UpdateProjectList(availableProjects, this);
        TurnManager.OnNewTurnBegun += NewTurn;
    }

    public void SelectProject(Project project) => buildingManager.SetCurrentProject(project);
    public void PayForProject(Project project) => RemoveResources(project.costToBegin);
    public bool CanPayForProject(Project project) => IsAffordable(project.costToBegin);

    private void NewTurn(object sender, TurnManager.OnTurnEventArgs e)
    {
        SimulateEconomy();
    }

    #region Economy
    private void SimulateEconomy()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            int resourceDifference = incomes[i].value - expenses[i].value;

            if (resourceDifference > 0)
                resources[i].AddValue(resourceDifference);
            else if (resourceDifference < 0)
                resources[i].RemoveValue(resourceDifference * -1);
        }

        UpdateEconomyUI();
    }

    private void UpdateEconomyUI()
    {
        for (int i = 0; i < resourcePanels.Length; i++)
        {
            resourcePanels[i].UpdatePanel(resources[i].value, incomes[i].value, expenses[i].value);
        }
        projectOverview.UpdateAffordables();
    }

    public bool IsAffordable(Resource[] costs) => Resource.IsAffordable(costs, resources);

    public void AddResources(Resource[] allResourcesToAdd)
    {
        foreach (var resouceToAdd in allResourcesToAdd)
        {
            resources[(int)resouceToAdd.resourceType].AddValue(resouceToAdd.value);
        }
        UpdateEconomyUI();
    }

    public void RemoveResources(Resource[] allResourcesToRemove)
    {
        foreach (var resourceToRemove in allResourcesToRemove)
        {
            resources[(int)resourceToRemove.resourceType].RemoveValue(resourceToRemove.value);
        }
        UpdateEconomyUI();
    }

    public void IncreaseTurnIncome(Resource[] allNewIncomes)
    {
        foreach (var newIncome in allNewIncomes)
        {
            incomes[(int)newIncome.resourceType].AddValue(newIncome.value);
        }
        UpdateEconomyUI();
    }

    public void IncreaseTurnExpenses(Resource[] allNewExpenses)
    {
        foreach (var newExpense in allNewExpenses)
        {
            expenses[(int)newExpense.resourceType].AddValue(newExpense.value);
        }
        UpdateEconomyUI();
    }
    #endregion
}