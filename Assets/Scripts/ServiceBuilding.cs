using UnityEngine;

public class ServiceBuilding : Building
{
    [SerializeField] float effectRadius = 3f;
    public const float FACTORINCREASE = 0.1f;

    private void OnEnable()
    {
        Residence.OnResidenceSpawned += CheckEffectOnSpawnedResidence;
    }

    private void OnDisable()
    {
        Residence.OnResidenceSpawned -= CheckEffectOnSpawnedResidence;
    }

    public override void Setup(Player player, Project project, int themeIndex)
    {
        base.Setup(player, project, themeIndex);
    }

    protected override void FinishConstruction()
    {
        base.FinishConstruction();

        AddChangeToNearbyResidences();
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

    void CheckEffectOnSpawnedResidence(Residence spawnedResidence)
    {
        if (!isFinished)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, effectRadius);

        foreach (var collider in colliders)
        {
            Residence residence = collider.GetComponent<Residence>();
            if (residence && residence == spawnedResidence)
            {
                residence.AddServiceBuilding(this);
            }
        }
    }
}
