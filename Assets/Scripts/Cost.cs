
[System.Serializable]
public class Cost
{
    public int gold;
    public int wood;
    public int food;

    public bool IsAffordable(ResourceInventory resourceInventory, out string explanation)
    {
        bool affordable = true;
        explanation = "";
        if (resourceInventory.gold < gold)
        {
            affordable = false;
            explanation = Utility.AddTextOnNewLine(explanation, "Not enough gold");
        }
        if (resourceInventory.wood < wood)
        {
            affordable = false;
            explanation = Utility.AddTextOnNewLine(explanation, "Not enough wood");
        }
        if (resourceInventory.food < food)
        {
            affordable = false;
            explanation = Utility.AddTextOnNewLine(explanation, "Not enough food");
        }
        return affordable;
    }

    public override string ToString()
    {
        string text = "";
        if (gold > 0)
            text = Utility.AddTextOnNewLine(text, $"Gold: {gold}");
        if (wood > 0)
            text = Utility.AddTextOnNewLine(text, $"Wood: {wood}");
        if (food > 0)
            text = Utility.AddTextOnNewLine(text, $"Food: {food}");

        if (text == "")
            text += "Free!";
        return text;
    }
}
