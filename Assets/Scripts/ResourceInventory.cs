using UnityEngine;
using System;

public enum ResourceType { Gold, Wood, Food, MAX = 2, NONE }

[Serializable]
public class ResourceInventory
{
    public delegate void InventoryChange(ResourceType type, int change, int newValue);
    public InventoryChange OnInventoryChanged;

    public static float WOODTOGOLD = 1f;
    public static float FOODTOGOLD = 2f;

    public int capacity;
    public int gold;
    public int wood;
    public int food;

    public int SpaceUsed => gold + wood + food;
    public int SpaceLeft => capacity - SpaceUsed;
    public bool IsFull => SpaceLeft == 0;
    public bool IsEmpty => SpaceUsed == 0;
    public float FillFactor => SpaceUsed / (float)capacity;

    public void Add(ResourceType type, int numberToAdd)
    {
        if (SpaceLeft < numberToAdd)
        {
            numberToAdd = SpaceLeft;
            Debug.LogWarning($"Tried to add more {type} than space left");
        }
        switch (type)
        {
            case ResourceType.Gold:
                gold += numberToAdd;
                break;
            case ResourceType.Wood:
                wood += numberToAdd;
                break;
            case ResourceType.Food:
                food += numberToAdd;
                break;
        }
        OnInventoryChanged?.Invoke(type, numberToAdd, GetResourceNumber(type));
    }
    public bool Remove(ResourceType type, int numberToRemove)
    {
        if (numberToRemove > GetResourceNumber(type))
        {
            Debug.LogWarning($"Tried to remove more {type} than currently having");
            return false;
        }
        switch (type)
        {
            case ResourceType.Gold:
                gold -= numberToRemove;
                break;
            case ResourceType.Wood:
                wood -= numberToRemove;
                break;
            case ResourceType.Food:
                food -= numberToRemove;
                break;
        }
        OnInventoryChanged?.Invoke(type, -numberToRemove, GetResourceNumber(type));
        return true;
    }
    public bool Remove(Cost cost)
    {
        if (!Remove(ResourceType.Gold, cost.gold))
            return false;
        if (!Remove(ResourceType.Wood, cost.wood))
            return false;
        if (!Remove(ResourceType.Food, cost.food))
            return false;
        return true;
    }
    public int GetResourceNumber(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Gold:
                return gold;
            case ResourceType.Wood:
                return wood;
            case ResourceType.Food:
                return food;
            default:
                Debug.LogWarning($"Type not defined {type}");
                return int.MinValue;
        }
    }
    public bool HasResource(ResourceType type) => GetResourceNumber(type) > 0;

    public void ClearInventory()
    {
        for (ResourceType i = 0; i <= ResourceType.MAX; i++)
        {
            Remove(i, GetResourceNumber(i));
        }
    }
}
