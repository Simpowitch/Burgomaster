using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Project
{
    public string projectName;
    public int turnsToComplete;
    public Resource[] costToBegin;
    public Sprite sprite;
    public UnityEvent OnProjectSelected;
}
