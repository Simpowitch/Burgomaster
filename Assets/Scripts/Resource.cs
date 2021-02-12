using UnityEngine;

public enum ResourceType { Gold, Wood, Food, Iron, MAX = 4 }
[System.Serializable]
public class Resource
{
    public string Name { get => resourceType.ToString(); }
    bool canBeNegative;

    public ResourceType resourceType;
    public int value;
    public Resource(ResourceType type, int startValue = 0, bool canBeNegative = false)
    {
        this.resourceType = type;
        this.value = startValue;
        this.canBeNegative = canBeNegative;
    }
    public bool IsAffordable(int request) => request <= value;
    public void AddValue(int add)
    {
        value += add;
    }

    public void RemoveValue(int remove)
    {
        value -= remove;
        if (!canBeNegative)
            value = Mathf.Max(value, 0);
    }

    public static Resource GetResourceByType(Resource[] resources, ResourceType requestedType)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if (resources[i].resourceType == requestedType)
                return resources[i];
        }
        return null;
    }

    public static bool IsAffordable(Resource[] costs, Resource[] availableResources)
    {
        for (int i = 0; i < costs.Length; i++)
        {
            Resource resourceToCheck = GetResourceByType(availableResources, costs[i].resourceType);
            if (costs[i].value > resourceToCheck.value)
                return false;
        }
        return true;
    }
}
