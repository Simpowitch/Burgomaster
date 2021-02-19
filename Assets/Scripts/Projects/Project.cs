using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Project", menuName = "ScriptableObject/Project")]
public class Project : ScriptableObject
{
    public string projectName;
    public enum Category { Housing, Production, Service, Unique }
    public Category category;
    public Resource[] costToBegin;
    public int turnsToComplete;
    public int populationChange;
    public AbilityScore abilityScore = AbilityScore.UNUSED;
    public Resource[] income = null, upkeep = null;
    public Sprite sprite;
    public ConstructionPlacer blueprint = null;

    public ServiceBuildingRequirement serviceBuildingRequirement;

    [System.Serializable]
    public abstract class Requirement
    {
        public abstract bool RequirementFullfilled(Player player);
    }
    [System.Serializable]
    public class ServiceBuildingRequirement : Requirement
    {
        public AbilityScore type = AbilityScore.UNUSED;
        public int value;

        public override bool RequirementFullfilled(Player player)
        {
            if (type == AbilityScore.UNUSED)
                return true;
            return value <= player.GetServices(type).Count;
        }
    }
}
