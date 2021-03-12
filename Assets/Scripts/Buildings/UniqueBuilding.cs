
public class UniqueBuilding : Building
{
    public override void Setup(Player player, Project project, int themeIndex)
    {
        base.Setup(player, project, themeIndex);

        player.RemoveUniqueProject(project);
    }
}
