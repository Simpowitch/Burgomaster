using UnityEngine;
public enum Race { Elf, Dwarf, Human }

[System.Serializable]
public class RacePreset
{
    public Race race;
    public Sprite[] sprites;
    public int strength = 10, constitution = 10, wisdom = 10, intelligence = 10, diplomacy = 10, charisma = 10;
}
