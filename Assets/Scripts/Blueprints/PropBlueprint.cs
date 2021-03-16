using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Prop Blueprint", menuName = "ScriptableObject/Prop Blueprint")]
public class PropBlueprint : Blueprint
{
    public string propName;
    public Sprite uiSprite;

    public enum Category { Tree, Stone, Misc}
    public Category category;

    public override Resource[] Cost => null;
}
