using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Project", menuName = "ScriptableObjects/Project")]
public class Project : ScriptableObject
{
    public string projectName;
    public enum Category { Housing, Production, Service, Unique }
    public Category category;
    public int turnsToComplete;
    public Resource[] costToBegin;
    public Sprite sprite;
    public ConstructionPlacer blueprint = null;
}
