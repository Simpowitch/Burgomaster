using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Building Blueprint", menuName = "ScriptableObject/Building Blueprint")]
public class BuildingBlueprint : Blueprint
{
    public string buildingName;
    public string description;
    public Sprite uiSprite;

    public enum Category { Housing, Production, Service, Unique }
    public Category category;

    public int turnsToComplete;
    public AbilityScore abilityTag = AbilityScore.UNUSED;

    public ServiceBuildingRequirement serviceBuildingRequirement;
    public bool HasRequirement => serviceBuildingRequirement != null && serviceBuildingRequirement.type != AbilityScore.UNUSED && serviceBuildingRequirement.value > 0;

    public Resource[] cost, income, upkeep, demolishRefund;
    public Effect[] completionEffects;

    [Header("Level Up")]
    public BuildingBlueprint levelUpBlueprint;
    public NotificationInformation levelUpNotification;
    public bool HasLevelUpBlueprint => levelUpBlueprint != null;

    public override Resource[] Cost => cost;


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

        public string GetRequirementTextState(Player player)
        {
            if (player == null)
                return null;
            if (type == AbilityScore.UNUSED)
                return null;
            return $"{player.GetServices(type).Count} / {value}";
        }
    }

}
