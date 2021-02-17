
public class UniqueBuilding : Building
{
    Player.AbilityScore abilityScore;
    public int level;

    public override void Setup(Player player, Project project, int themeIndex)
    {
        base.Setup(player, project, themeIndex);

        this.abilityScore = project.abilityScore;
        player.RemoveUniqueProject(project);
    }

    protected override void FinishConstruction()
    {
        base.FinishConstruction();
        level++;
        player.ChangeAbilityScore(abilityScore);
    }

    public void LevelUp()
    {
        level++;
        player.ChangeAbilityScore(abilityScore);
    }
}
