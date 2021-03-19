using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : WorldObject
{
    [SerializeField] bool autoAssignMaterialAtStart = false;

    private void Start()
    {
        if (autoAssignMaterialAtStart)
            Setup(null, null, 0);
    }

    public override void Setup(Player player, Blueprint project, int themeIndex)
    {
        base.Setup(player, project, themeIndex);
        visuals.material = constructedMaterial;
        OnCompletionEvents?.Invoke();
    }
}
