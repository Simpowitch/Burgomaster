using UnityEngine;

public class ServiceBuilding : Building
{
    [SerializeField] float effectRadius = 3f;
    public const float FACTORINCREASE = 0.1f;

    Player.AbilityScore abilityScore;


    protected override void OnEnable()
    {
        base.OnEnable();
        TurnManager.OnUpdateServiceEffects += UpdateServiceEffect;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TurnManager.OnUpdateServiceEffects -= UpdateServiceEffect;
    }

    public override void Setup(Player player, Project project, int themeIndex)
    {
        base.Setup(player, project, themeIndex);
        abilityScore = project.abilityScore;
    }

    void UpdateServiceEffect(object sender, TurnManager.OnTurnEventArgs e)
    {
        if (!isFinished)
            return;

        AddChangeToNearbyResidences();
    }

    protected override void FinishConstruction()
    {
        base.FinishConstruction();

        AddChangeToNearbyResidences();
        player.AddService(abilityScore, this);
    }

    void AddChangeToNearbyResidences()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, effectRadius);

        foreach (var collider in colliders)
        {
            Residence residence = collider.GetComponent<Residence>();
            if (residence)
            {
                residence.AddServiceBuilding(this);
            }
        }
    }
}
