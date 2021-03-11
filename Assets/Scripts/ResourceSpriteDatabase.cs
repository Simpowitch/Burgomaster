using UnityEngine;

public class ResourceSpriteDatabase : MonoBehaviour
{
    public static Sprite[] EffectSprites;
    public Sprite[] sprites;

    private void Awake()
    {
        EffectSprites = sprites;
    }

    public static Sprite GetSprite(ResourceType type)
    {
        if (EffectSprites == null)
        {
            Debug.LogError("Sprite Database not set up");
            return null;
        }

        if ((int)type >= EffectSprites.Length)
        {
            Debug.LogError("Sprite request out of range");
            return null;
        }

        return EffectSprites[(int)type];
    }
}
