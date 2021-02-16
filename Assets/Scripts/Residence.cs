using System.Collections.Generic;
using UnityEngine;

public class Residence : Building
{
    List<ServiceBuilding> nearbyServices = new List<ServiceBuilding>();
    int basePopulationChange;

    public delegate void ResidenceHandler(Residence residence);
    public static ResidenceHandler OnResidenceSpawned;

    public override void Setup(Player player, Project project, int themeIndex)
    {
        base.Setup(player, project, themeIndex);
        this.basePopulationChange = project.populationChange;
        OnCompletion += () => player.IncreasePopulation(basePopulationChange);
    }

    protected override void FinishConstruction()
    {
        base.FinishConstruction();
        OnResidenceSpawned?.Invoke(this);
    }

    public void AddServiceBuilding(ServiceBuilding serviceBuilding)
    {
        if (!nearbyServices.Contains(serviceBuilding))
        {
            nearbyServices.Add(serviceBuilding);

            int populationChange = Mathf.RoundToInt(basePopulationChange * ServiceBuilding.FACTORINCREASE);
            player.IncreasePopulation(populationChange);

            Resource[] incomeChange = new Resource[income.Length];
            for (int i = 0; i < incomeChange.Length; i++)
            {
                int valueChange = Mathf.RoundToInt(income[i].value * ServiceBuilding.FACTORINCREASE);
                incomeChange[i] = new Resource(income[i].resourceType, valueChange);
            }
            player.IncreaseTurnIncome(incomeChange);
        }
    }
}
