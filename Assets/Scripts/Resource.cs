using UnityEngine;

public enum ResourceType { Gold, Wood, Food, Iron, MAX = 4 }
[System.Serializable]
public class Resource
{
    public string Name { get => resourceType.ToString(); }
    public bool CanBeNegative { get; private set; }

    public ResourceType resourceType;
    public int value;
    public Resource(ResourceType type, int startValue = 0, bool canBeNegative = false)
    {
        this.resourceType = type;
        this.value = startValue;
        this.CanBeNegative = canBeNegative;
    }
    public bool IsAffordable(int request) => request <= value;
    public void AddValue(int add)
    {
        value += add;
    }

    public void RemoveValue(int remove)
    {
        value -= remove;
        if (!CanBeNegative)
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

    public override string ToString()
    {
        string description = "";
        if (value >= 0)
            description += "+";
        description += value.ToString();
        return description;
    }
}

public static class ResourceExtension
{
    public static Resource Copy(this Resource resource)
    {
        return new Resource(resource.resourceType, resource.value, resource.CanBeNegative);
    }

    public static Resource[] Copy(this Resource[] resources)
    {
        Resource[] newArray = new Resource[resources.Length];
        for (int i = 0; i < resources.Length; i++)
        {
            newArray[i] = resources[i].Copy();
        }
        return newArray;
    }
}
