using UnityEngine;

public class TagSpriteDatabase : MonoBehaviour
{
    public static Sprite[] TagSprites;
    public Sprite[] sprites;

    private void Awake()
    {
        TagSprites = sprites;
    }

    public static Sprite GetSprite(AbilityScore type)
    {
        if (TagSprites == null)
        {
            Debug.LogError("Sprite Database not set up");
            return null;
        }

        if ((int)type >= TagSprites.Length)
        {
            Debug.LogError("Sprite request out of range");
            return null;
        }

        return TagSprites[(int)type];
    }
}
