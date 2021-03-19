using UnityEngine;

public class ServiceBuilding : Building
{
    [SerializeField] SpriteRenderer effectRenderer = null;
    [SerializeField] float effectRadius = 3f;
    public float effectivityInfluence;

    AbilityScore abilityTag;

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

    protected override void Awake()
    {
        base.Awake();
        effectRenderer.transform.localScale *= effectRadius / 2;
    }

    public override void Setup(Player player, Blueprint blueprint, int themeIndex)
    {
        base.Setup(player, blueprint, themeIndex);
        abilityTag = projectInfo.abilityTag;

        foreach (var effect in projectInfo.completionEffects)
        {
            switch (effect.type)
            {
                case Effect.Type.ProductivityPercentage:
                    effectivityInfluence = Utility.PercentageToFactor(effect.effectValue);
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

    public override void Despawn()
    {
        base.Despawn();
        UpdateEffectivityOfNearbyBuildings(false);
        this.enabled = false;
    }

    void UpdateServiceEffect(object sender, TurnManager.OnTurnEventArgs e)
    {
        if (!isFinished)
            return;

        UpdateEffectivityOfNearbyBuildings(true);
    }

    protected override void FinishConstruction()
    {
        base.FinishConstruction();

        UpdateEffectivityOfNearbyBuildings(true);
        player.AddService(abilityTag, this);
    }

    void UpdateEffectivityOfNearbyBuildings(bool positive)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, effectRadius);

        foreach (var collider in colliders)
        {
            Residence residence = collider.GetComponent<Residence>();
            if (residence)
            {
                if (positive)
                {
                    residence.AddServiceBuilding(this);
                }
                else
                {
                    residence.RemoveServiceBuilding(this);
                }
            }
        }
    }

    protected override void Select()
    {
        base.Select();
        effectRenderer.enabled = true;
    }

    protected override void DeSelect()
    {
        base.DeSelect();
        effectRenderer.enabled = false;
    }
}
