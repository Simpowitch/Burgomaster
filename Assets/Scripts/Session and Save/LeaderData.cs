
[System.Serializable]
public class LeaderData
{
    public string leaderName;
    public int[] abilityScores;
    public int raceIndex;
    public int backgroundIndex;
    public int portraitIndex;
    public int bannerIndex;

    public LeaderData(string leaderName, AbilityScoreBlock scoreBlock, int raceIndex, int backgroundIndex, int portraitIndex, int bannerIndex)
    {
        this.leaderName = leaderName;
        this.abilityScores = scoreBlock.GetAbilityScores();
        this.raceIndex = raceIndex;
        this.backgroundIndex = backgroundIndex;
        this.portraitIndex = portraitIndex;
        this.bannerIndex = bannerIndex;
    }
}
