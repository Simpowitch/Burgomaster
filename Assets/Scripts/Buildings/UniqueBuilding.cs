
public class UniqueBuilding : Building
{
    AbilityScore abilityScore;
    public int level;
    bool CanLevelUp => isFinished && player.IsAffordable(projectInfo.upgradeCost);

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

    private void LevelUp()
    {
        level++;
        player.ChangeAbilityScore(abilityScore);
    }


    protected override void Select()
    {
        SetupBuildingInspector();
        BuildingInspector.instance.Show(true);
        player.OnEconomyChanged += SetupBuildingInspector;
    }

    private void SetupBuildingInspector()
    {
        BuildingInspector.instance.SetupUpgradeable(this.transform, projectInfo.name, projectInfo.sprite, projectInfo.completionEffects, income, upkeep, LevelUp, projectInfo.costToBegin, CanLevelUp);
    }

    protected override void DeSelect()
    {
        BuildingInspector.instance.Show(false);
        player.OnEconomyChanged -= SetupBuildingInspector;
    }
}
