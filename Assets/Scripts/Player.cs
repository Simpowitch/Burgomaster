using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] List<BuildingBlueprint> buildingBlueprints = new List<BuildingBlueprint>();
    [SerializeField] List<PropBlueprint> propBlueprints = new List<PropBlueprint>();

    List<BuildingBlueprint> availableBuildingBlueprints = new List<BuildingBlueprint>();

    [SerializeField] TextMeshProUGUI leaderNameText = null;
    [SerializeField] Image leaderImage = null;
    [SerializeField] Image bannerImage = null, diceRollBanner = null;
    [SerializeField] AbilityScoreBlockUI abilityScoreUI = null;

    [SerializeField] TextMeshProUGUI populationText = null;
    [SerializeField] ResourcePanelUI[] resourcePanels = null;

    [SerializeField] BuildingSelectionOverview buildingSelector = null;
    [SerializeField] PropSelectionOverview propSelector = null;

    [SerializeField] SpawnManager buildingManager = null;

    public delegate void PlayerChange();
    public PlayerChange OnEconomyChanged;

    public int[] AbilityScores { get; private set; } = new int[6];
    int population;
    Resource[] resources;
    Resource[] incomes;
    Resource[] expenses;

    List<ServiceBuilding>
        authoritarianServices = new List<ServiceBuilding>(),
        cunningServices = new List<ServiceBuilding>(),
        diplomaticServices = new List<ServiceBuilding>(),
        liberalServices = new List<ServiceBuilding>(),
        natureServices = new List<ServiceBuilding>(),
        religiousServices = new List<ServiceBuilding>();


    private void OnEnable()
    {
        TurnManager.OnProduceIncome += SimulateEconomy;
    }

    private void OnDisable()
    {
        TurnManager.OnProduceIncome -= SimulateEconomy;
    }


    private void Start()
    {
        LoadSave(ActiveSessionHolder.ActiveSession);

        UpdateEconomyUI();
        UpdateAbilityScoreUI();
        populationText.text = population.ToString();

        OnEconomyChanged += UpdateEconomyUI;
        OnEconomyChanged += UpdateAvailableProjects;

        UpdateAvailableProjects();
    }

    void LoadSave(SaveData data)
    {
        if (data == null)
        {
            Debug.Log("Creating default data");
            data = new SaveData(new LeaderData("Debug", new AbilityScoreBlock { authority = 5, cunning = 4, diplomacy = 3, knowledge = 2, nature = 1, piety = 0 }, 0, 0, 0, 0), new CityData(1000, 1000, 1000, 1000));
        }

        //Setup resources
        resources = new Resource[(int)ResourceType.MAX];
        for (int i = 0; i < resources.Length; i++)
        {
            ResourceType type = (ResourceType)i;
            switch (type)
            {
                case ResourceType.Gold:
                    resources[i] = new Resource(type, data.CityData.gold, false);
                    break;
                case ResourceType.Wood:
                    resources[i] = new Resource(type, data.CityData.wood, false);
                    break;
                case ResourceType.Food:
                    resources[i] = new Resource(type, data.CityData.food, false);
                    break;
                case ResourceType.Iron:
                    resources[i] = new Resource(type, data.CityData.iron, false);
                    break;
                case ResourceType.MAX:
                    break;
            }
        }

        //TODO: Check incomes and expenses in saved data
        Debug.LogWarning("Loading income and expenses not implemented");

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

        //TODO: Load buildings etc
        Debug.LogWarning("Loading buildings not implemented");
        Debug.LogWarning("Loading advisors not implemented");

        //Display Load Leader
        leaderNameText.text = data.LeaderData.leaderName;
        leaderImage.sprite = LeaderDatabase.GetPortraitByRace(data.LeaderData.raceIndex, data.LeaderData.portraitIndex);
        bannerImage.sprite = LeaderDatabase.Banners[data.LeaderData.bannerIndex];
        diceRollBanner.sprite = LeaderDatabase.Banners[data.LeaderData.bannerIndex];

        //Display stats
        AbilityScores = data.LeaderData.abilityScores;
    }

    public void SelectBlueprint(Blueprint blueprint) => buildingManager.SetCurrentBlueprint(blueprint);

    public void PayForProject(Blueprint blueprint) => RemoveResources(blueprint.Cost);
    public bool CanDoProject(Blueprint blueprint)
    {
        if (blueprint is BuildingBlueprint)
        {
            return availableBuildingBlueprints.Contains(blueprint as BuildingBlueprint) && IsAffordable(blueprint.Cost);
        }
        else if (blueprint is PropBlueprint)
        {
            return propBlueprints.Contains(blueprint as PropBlueprint);
        }
        else
        {
            Debug.LogWarning("Check if blueprint can be spawned: Failed");
            return false;
        }
    }

    #region Economy
    private void SimulateEconomy(object sender, TurnManager.OnTurnEventArgs e)
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
        buildingSelector.UpdateAffordables();
    }

    public bool IsAffordable(Resource[] costs) => Resource.IsAffordable(costs, resources);

    public void AddResource(Resource resource)
    {
        resources[(int)resource.resourceType].AddValue(resource.value);
        OnEconomyChanged?.Invoke();
    }
    public void AddResources(Resource[] allResourcesToAdd)
    {
        if (allResourcesToAdd == null)
            return;
        foreach (var resouceToAdd in allResourcesToAdd)
        {
            AddResource(resouceToAdd);
        }
    }

    public void RemoveResource(Resource resource)
    {
        resources[(int)resource.resourceType].RemoveValue(resource.value);
        OnEconomyChanged?.Invoke();
    }
    public void RemoveResources(Resource[] allResourcesToRemove)
    {
        if (allResourcesToRemove == null)
            return;
        foreach (var resourceToRemove in allResourcesToRemove)
        {
            RemoveResource(resourceToRemove);
        }
    }

    public void ChangeTurnIncomes(Resource[] affectedIncomes, bool increase)
    {
        if (affectedIncomes == null)
            return;
        foreach (var income in affectedIncomes)
        {
            if (increase)
                incomes[(int)income.resourceType].AddValue(income.value);
            else
                incomes[(int)income.resourceType].RemoveValue(income.value);
        }
        OnEconomyChanged?.Invoke();
    }

    public void ChangeTurnExpenses(Resource[] affectedIncomes, bool increase)
    {
        if (affectedIncomes == null)
            return;
        foreach (var expense in affectedIncomes)
        {
            if (increase)
                this.expenses[(int)expense.resourceType].AddValue(expense.value);
            else
                this.expenses[(int)expense.resourceType].RemoveValue(expense.value);
        }
        OnEconomyChanged?.Invoke();
    }

    #endregion

    public void IncreasePopulation(int increase)
    {
        population += increase;
        populationText.text = population.ToString();
    }
    public void DecreasePopulation(int decrease)
    {
        population -= decrease;
        populationText.text = population.ToString();
    }

    public void AddService(AbilityScore tag, ServiceBuilding serviceBuilding)
    {
        switch (tag)
        {
            case AbilityScore.UNUSED:
                Debug.LogError("Service building is missing tag");
                break;
            case AbilityScore.Authority:
                authoritarianServices.Add(serviceBuilding);
                break;
            case AbilityScore.Cunning:
                cunningServices.Add(serviceBuilding);
                break;
            case AbilityScore.Diplomacy:
                diplomaticServices.Add(serviceBuilding);
                break;
            case AbilityScore.Knowledge:
                liberalServices.Add(serviceBuilding);
                break;
            case AbilityScore.Nature:
                natureServices.Add(serviceBuilding);
                break;
            case AbilityScore.Piety:
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
            case AbilityScore.Authority:
                return authoritarianServices;
            case AbilityScore.Cunning:
                return cunningServices;
            case AbilityScore.Diplomacy:
                return diplomaticServices;
            case AbilityScore.Knowledge:
                return liberalServices;
            case AbilityScore.Nature:
                return natureServices;
            case AbilityScore.Piety:
                return religiousServices;
        }
    }

    public void ChangeAbilityScore(AbilityScore abilityScore, int change = 1)
    {
        AbilityScores[(int)abilityScore] += change;
        UpdateAbilityScoreUI();
    }

    public int GetAbilityScore(AbilityScore abilityScore) => AbilityScores[(int)abilityScore];

    private void UpdateAbilityScoreUI() => abilityScoreUI.UpdateUI(AbilityScores);

    private void UpdateAvailableProjects()
    {
        availableBuildingBlueprints.Clear();
        foreach (var project in buildingBlueprints)
        {
            if (project.serviceBuildingRequirement == null || project.serviceBuildingRequirement.RequirementFullfilled(this))
            {
                availableBuildingBlueprints.Add(project);
            }
        }
        buildingSelector.UpdateProjectList(availableBuildingBlueprints, this);

        propSelector.UpdateProjectList(propBlueprints, this);
    }

    public void AddUniqueBlueprint(BuildingBlueprint blueprint)
    {
        if (!buildingBlueprints.Contains(blueprint))
        {
            buildingBlueprints.Add(blueprint);
            UpdateAvailableProjects();
        }
    }

    public void RemoveUniqueBlueprint(BuildingBlueprint blueprint)
    {
        if (buildingBlueprints.Contains(blueprint))
        {
            buildingBlueprints.Remove(blueprint);
            UpdateAvailableProjects();
        }
    }
}