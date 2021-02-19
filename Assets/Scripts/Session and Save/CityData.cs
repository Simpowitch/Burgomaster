using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityData
{
    public int gold, wood, food, iron;

    public CityData(int gold, int wood, int food, int iron)
    {
        this.gold = gold;
        this.wood = wood;
        this.food = food;
        this.iron = iron;
    }
}
