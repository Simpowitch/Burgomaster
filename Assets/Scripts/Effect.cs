using UnityEngine;

[System.Serializable]
public class Effect
{
    public enum Type 
    { 
        Population, 
        ProductivityPercentage,
        Authority,
        Cunning,
        Diplomacy,
        Knowledge,
        Nature,
        Piety
    }

    public Type type;
    public int effectValue;

    public Sprite Sprite => EffectSpriteDatabase.GetSprite(type);
    public bool ChangeIsPercentage => type == Type.ProductivityPercentage;

    public Effect(Type type, int effectValue)
    {
        this.type = type;
        this.effectValue = effectValue;
    }

    public override string ToString()
    {
        string description = "";
        if (effectValue >= 0)
            description += "+";
        description += effectValue.ToString();
        if (ChangeIsPercentage)
            description += "%";
        return description;
    }
}

public static class EffectExtension
{
    public static Effect Copy(this Effect effect, float multiplier = 1f)
    {
        return new Effect(effect.type, Mathf.RoundToInt(effect.effectValue * multiplier));
    }

    public static Effect[] Copy(this Effect[] resources, float multiplier = 1f)
    {
        Effect[] newArray = new Effect[resources.Length];
        for (int i = 0; i < resources.Length; i++)
        {
            newArray[i] = resources[i].Copy(multiplier);
        }
        return newArray;
    }
}
