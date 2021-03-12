using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Project", menuName = "ScriptableObject/Project")]
public class Project : ScriptableObject
{
    public string projectName;
    public string description;
    public Sprite sprite;

    public enum Category { Housing, Production, Service, Unique }
    public Category category;

    public int turnsToComplete;
    public AbilityScore abilityTag = AbilityScore.UNUSED;
    public ConstructionPlacer blueprint = null;

    public ServiceBuildingRequirement serviceBuildingRequirement;

    public Resource[] cost, income, upkeep;
    public Effect[] completionEffects;

    public Project levelUpProject;
    public bool HasLevelUpProject => levelUpProject != null;

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
