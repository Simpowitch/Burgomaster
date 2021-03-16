using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Blueprint : ScriptableObject
{
    public Spawner prefab = null;
    public abstract Resource[] Cost { get; }
}
