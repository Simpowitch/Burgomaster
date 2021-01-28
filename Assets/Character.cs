using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public enum Race { Elf, Dwarf, Human}
    public Race race;
    public Sprite sprite;
    public int strength = 10, constitution = 10, wisdom = 10, intelligence = 10, diplomacy = 10, charisma = 10;
}
