using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public LeaderData LeaderData { get; private set; }
    public CityData CityData { get; private set; }

    public SaveData(LeaderData leaderData, CityData cityData)
    {
        LeaderData = leaderData;
        CityData = cityData;
    }
}
