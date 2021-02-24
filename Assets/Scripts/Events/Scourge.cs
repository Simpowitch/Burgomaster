using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Scourge
{
    public string title;
    [TextArea(3, 10)]
    public string description;
    public Resource[] activeResourceDrain;
    public Option[] options;
    [HideInInspector] public bool onCooldown;
    public Sprite eventSprite;

    public Scourge(Scourge preset)
    {
        this.title = preset.title;
        this.description = preset.description;
        activeResourceDrain = preset.activeResourceDrain;
        options = preset.options;
        this.eventSprite = preset.eventSprite;
    }

    [System.Serializable]
    public class Option
    {
        public string optionText;
        public Resource[] cost;
        public AbilityScore checkType;
        public int challengeRating = 10;
    }
}
