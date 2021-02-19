public enum AbilityScore
{
    Authoritarian,
    Cunning,
    Diplomatic,
    Liberal,
    Nature,
    Religious,
    UNUSED,
}

[System.Serializable]
public class AbilityScoreBlock
{
    public static readonly int ABILITYSCORES = 6;

    public int authoritarian,
    cunning,
    diplomatic,
    liberal,
    nature,
    religious;

    public int GetAbilityScore(AbilityScore abilityScore)
    {
        switch (abilityScore)
        {
            case AbilityScore.Authoritarian:
                return authoritarian;
            case AbilityScore.Cunning:
                return cunning;
            case AbilityScore.Diplomatic:
                return diplomatic;
            case AbilityScore.Liberal:
                return liberal;
            case AbilityScore.Nature:
                return nature;
            case AbilityScore.Religious:
                return religious;
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