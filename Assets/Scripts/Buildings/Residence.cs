using System.Collections.Generic;
using UnityEngine;

public class Residence : Building
{
    List<ServiceBuilding> nearbyServices = new List<ServiceBuilding>();
    int basePopulationChange;
    float effectivity = 1f;
    Effect[] currentEffects;

    public override void Setup(Player player, Project project, int themeIndex)
    {
        base.Setup(player, project, themeIndex);

        foreach (var effect in projectInfo.completionEffects)
        {
            switch (effect.type)
            {
                case Effect.Type.Population:
                    basePopulationChange = effect.effectValue;
                    break;
                case Effect.Type.ProductivityPercentage:
                case Effect.Type.Authority:
                case Effect.Type.Cunning:
                case Effect.Type.Diplomacy:
                case Effect.Type.Knowledge:
                case Effect.Type.Nature:
                case Effect.Type.Piety:
                    Debug.LogWarning("Unhandled effecttype for Residence");
                    break;
            }
        }

        currentEffects = project.completionEffects;

        OnCompletion += () => player.IncreasePopulation(basePopulationChange);
    }

    protected override void FinishConstruction()
    {
        base.FinishConstruction();
    }

    public void AddServiceBuilding(ServiceBuilding serviceBuilding)
    {
        if (!nearbyServices.Contains(serviceBuilding))
        {
            nearbyServices.Add(serviceBuilding);

            int populationChange = Mathf.RoundToInt(basePopulationChange * serviceBuilding.productivityInfluence);
            player.IncreasePopulation(populationChange);

            Resource[] incomeChange = new Resource[projectInfo.income.Length];
            for (int i = 0; i < incomeChange.Length; i++)
            {
                int valueChange = Mathf.RoundToInt(projectInfo.income[i].value * serviceBuilding.productivityInfluence);
                incomeChange[i] = new Resource(projectInfo.income[i].resourceType, valueChange);
            }
            player.ChangeTurnIncomes(incomeChange, true);

            effectivity += serviceBuilding.productivityInfluence;

            currentEffects = projectInfo.completionEffects.Copy(effectivity);
        }
    }

    protected override void Select()
    {
        BuildingInspector.instance.Show(true);
        BuildingInspector.instance.SetupDefault(this.transform, projectInfo.name, projectInfo.sprite, currentEffects, income, upkeep);
    }

    protected override void DeSelect()
    {
        BuildingInspector.instance.Show(false);
    }
}
