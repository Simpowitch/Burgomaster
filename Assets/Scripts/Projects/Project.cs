using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Project", menuName = "ScriptableObjects/Project")]
public class Project : ScriptableObject
{
    public string projectName;
    public enum Category { Housing, Production, Service, Unique }
    public Category category;
    public Resource[] costToBegin;
    public int turnsToComplete;
    public int populationChange;
    public Resource[] income = null, upkeep = null;
    public Sprite sprite;
    public ConstructionPlacer blueprint = null;
}
