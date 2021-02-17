using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Resource[] startResources;

    int population;
    Resource[] resources;
    Resource[] incomes;
    Resource[] expenses;

    [SerializeField] List<Project> allProjects = new List<Project>();
    List<Project> availableProjects = new List<Project>();

    [SerializeField] ResourcePanelUI[] resourcePanels = null;
    [SerializeField] ProjectOverviewUI projectOverview = null;
    [SerializeField] AbilityScoreUI abilityScoreUI = null;
    [SerializeField] BuildingManager buildingManager = null;

    List<ServiceBuilding>
        authoritarianServices = new List<ServiceBuilding>(),
        cunningServices = new List<ServiceBuilding>(),
        diplomaticServices = new List<ServiceBuilding>(),
        liberalServices = new List<ServiceBuilding>(),
        natureServices = new List<ServiceBuilding>(),
        religiousServices = new List<ServiceBuilding>();

    public enum AbilityScore
    {
        Authoritarian,
        Cunning,
        Diplomatic,
        Liberal,
        Nature,
        Religious,
        UNUSED,
    }
    int[] abilityScores = new int[6];


    private void OnEnable()
    {
        TurnManager.OnTurnEnding += EndOfTurn;
    }

    private void OnDisable()
    {
        TurnManager.OnTurnEnding -= EndOfTurn;
    }

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
        UpdateAbilityScoreUI();

        UpdateAvailableProjects();
    }

    public void SelectProject(Project project) => buildingManager.SetCurrentProject(project);
    public void PayForProject(Project project) => RemoveResources(project.costToBegin);
    public bool CanDoProject(Project project) => availableProjects.Contains(project) && IsAffordable(project.costToBegin);

    private void EndOfTurn(object sender, TurnManager.OnTurnEventArgs e)
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

    public void IncreasePopulation(int increase) => population += increase;
    public void DecreasePopulation(int decrease) => population -= decrease;

    public void AddService(AbilityScore tag, ServiceBuilding serviceBuilding)
    {
        switch (tag)
        {
            case AbilityScore.UNUSED:
                Debug.LogError("Service building is missing tag");
                break;
            case AbilityScore.Authoritarian:
                authoritarianServices.Add(serviceBuilding);
                break;
            case AbilityScore.Cunning:
                cunningServices.Add(serviceBuilding);
                break;
            case AbilityScore.Diplomatic:
                diplomaticServices.Add(serviceBuilding);
                break;
            case AbilityScore.Liberal:
                liberalServices.Add(serviceBuilding);
                break;
            case AbilityScore.Nature:
                natureServices.Add(serviceBuilding);
                break;
            case AbilityScore.Religious:
                religiousServices.Add(serviceBuilding);
                break;
        }
        UpdateAvailableProjects();
    }

    public List<ServiceBuilding> GetServices(AbilityScore tag)
    {
        switch (tag)
        {
            case AbilityScore.UNUSED:
            default:
                return null;
            case AbilityScore.Authoritarian:
                return authoritarianServices;
            case AbilityScore.Cunning:
                return cunningServices;
            case AbilityScore.Diplomatic:
                return diplomaticServices;
            case AbilityScore.Liberal:
                return liberalServices;
            case AbilityScore.Nature:
                return natureServices;
            case AbilityScore.Religious:
                return religiousServices;
        }
    }

    public void ChangeAbilityScore(AbilityScore abilityScore, int change = 1)
    {
        abilityScores[(int)abilityScore] += change;
        UpdateAbilityScoreUI();
    }

    private void UpdateAbilityScoreUI() => abilityScoreUI.UpdateUI(abilityScores);

    private void UpdateAvailableProjects()
    {
        availableProjects.Clear();
        foreach (var project in allProjects)
        {
            if (project.serviceBuildingRequirement == null || project.serviceBuildingRequirement.RequirementFullfilled(this))
            {
                availableProjects.Add(project);
            }
        }
        projectOverview.UpdateProjectList(availableProjects, this);
    }

    public void RemoveUniqueProject(Project project)
    {
        if (allProjects.Contains(project))
        {
            allProjects.Remove(project);
            UpdateAvailableProjects();
        }
    }
}