using UnityEngine;

public enum ResourceType { Gold, Wood, Food, Iron, MAX = 4 }
[System.Serializable]
public class Resource
{
    public string Name { get => resourceType.ToString(); }
    public bool CanBeNegative { get; private set; }
    public Sprite Sprite => ResourceSpriteDatabase.GetSprite(resourceType);

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
        if (costs == null)
            return true;
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
        return value.ToString();
    }
}

public static class ResourceExtension
{
    public static Resource Copy(this Resource resource, float multiplier = 1f)
    {
        return new Resource(resource.resourceType, Mathf.RoundToInt(resource.value * multiplier), resource.CanBeNegative);
    }

    public static Resource[] Copy(this Resource[] resources, float multiplier = 1f)
    {
        Resource[] newArray = new Resource[resources.Length];
        for (int i = 0; i < resources.Length; i++)
        {
            newArray[i] = resources[i].Copy(multiplier);
        }
        return newArray;
    }
}
