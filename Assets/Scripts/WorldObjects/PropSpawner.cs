using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : Spawner
{
    Prop prop = null;

    public override void ChangeTheme(bool next) => prop.ChangeTheme(next);
    public override void ChangeTheme(int index) => prop.ChangeTheme(index);
    public override int CurrentTheme => prop.ThemeIndex;

    void Awake()
    {
        prop = GetComponent<Prop>();
    }

    public override void Spawn(Blueprint blueprint, Transform spawnParent, Player player)
    {
        GameObject newSpawn = Instantiate(blueprint.prefab.gameObject, this.transform.position, this.transform.rotation, spawnParent);
        Prop spawnedProp = newSpawn.GetComponent<Prop>();
        spawnedProp.Setup(player, blueprint as PropBlueprint, CurrentTheme);
    }
}
