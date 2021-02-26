public enum AbilityScore
{
    Authority,
    Cunning,
    Diplomacy,
    Knowledge,
    Nature,
    Piety,
    UNUSED,
}

[System.Serializable]
public class AbilityScoreBlock
{
    public static readonly int ABILITYSCORES = 6;

    public int authority,
    cunning,
    diplomacy,
    knowledge,
    nature,
    piety;

    public int GetAbilityScore(AbilityScore abilityScore)
    {
        switch (abilityScore)
        {
            case AbilityScore.Authority:
                return authority;
            case AbilityScore.Cunning:
                return cunning;
            case AbilityScore.Diplomacy:
                return diplomacy;
            case AbilityScore.Knowledge:
                return knowledge;
            case AbilityScore.Nature:
                return nature;
            case AbilityScore.Piety:
                return piety;
            case AbilityScore.UNUSED:
            default:
                throw new System.Exception("Wrong enum-type of abilityscore");
        }
    }

    public int[] GetAbilityScores()
    {
        int[] scores = new int[ABILITYSCORES];
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = GetAbilityScore((AbilityScore)i);
        }
        return scores;
    }
}