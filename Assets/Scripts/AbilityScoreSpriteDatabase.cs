using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityScoreSpriteDatabase : MonoBehaviour
{
    public static Sprite[] AbilitySprites;
    public Sprite[] sprites;

    private void Awake()
    {
        AbilitySprites = sprites;
    }

    public static Sprite GetSprite(AbilityScore type)
    {
        if (AbilitySprites == null)
        {
            Debug.LogError("Sprite Database not set up");
            return null;
        }

        if ((int)type >= AbilitySprites.Length)
        {
            Debug.LogError("Sprite request out of range");
            return null;
        }

        return AbilitySprites[(int)type];
    }
}
