
public class UniqueBuilding : Building
{
    public override void Setup(Player player, Blueprint blueprint, int themeIndex)
    {
        base.Setup(player, blueprint, themeIndex);

        player.RemoveUniqueBlueprint(projectInfo);
    }
}
