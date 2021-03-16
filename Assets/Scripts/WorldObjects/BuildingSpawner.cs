using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : Spawner
{
    Building building = null;
    public override void ChangeTheme(bool next) => building.ChangeTheme(next);
    public override int CurrentTheme => building.ThemeIndex;

    void Awake()
    {
        building = GetComponent<Building>();
    }

    public override void Spawn(Blueprint blueprint, Transform spawnParent, Player player)
    {
        GameObject newSpawn = Instantiate(blueprint.prefab.gameObject, this.transform.position, this.transform.rotation, spawnParent);
        Building newBuilding = newSpawn.GetComponent<Building>();
        newBuilding.Setup(player, blueprint as BuildingBlueprint, CurrentTheme);
    }
}
