using UnityEngine;

public class ServiceBuilding : Building
{
    [SerializeField] SpriteRenderer effectRenderer = null;
    [SerializeField] float effectRadius = 3f;
    public float productivityInfluence;

    AbilityScore abilityScore;

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

    private void Awake()
    {
        effectRenderer.transform.localScale *= effectRadius / 2;
    }

    public override void Setup(Player player, Project project, int themeIndex)
    {
        base.Setup(player, project, themeIndex);
        abilityScore = project.abilityScore;

        foreach (var effect in projectInfo.completionEffects)
        {
            switch (effect.type)
            {
                case Effect.Type.ProductivityPercentage:
                    productivityInfluence = Utility.PercentageToFactor(effect.effectValue);
                    break;
                case Effect.Type.Population:
                case Effect.Type.Authority:
                case Effect.Type.Cunning:
                case Effect.Type.Diplomacy:
                case Effect.Type.Knowledge:
                case Effect.Type.Nature:
                case Effect.Type.Piety:
                    Debug.LogWarning("Unhandled effecttype for ServiceBuilding");
                    break;
            }
        }

        effectRenderer.enabled = false;
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

    protected override void Select()
    {
        effectRenderer.enabled = true;
        BuildingInspector.instance.Show(true);
        BuildingInspector.instance.SetupDefault(this.transform, projectInfo.name, projectInfo.sprite, projectInfo.completionEffects, income, upkeep);
    }

    protected override void DeSelect()
    {
        effectRenderer.enabled = true;
        BuildingInspector.instance.Show(false);
    }
}
