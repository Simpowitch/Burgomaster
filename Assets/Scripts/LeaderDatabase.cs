using UnityEngine;

public class LeaderDatabase : MonoBehaviour
{
    [SerializeField] Sprite[] banners = null;
    [SerializeField] RacePreset[] racePresets = null;

    public static Sprite[] Banners;
    public static Sprite[][] Portraits;
    public static RacePreset[] RacePresets;

    static bool initialized = false;

    private void Awake()
    {
        if (initialized)
            return;

        Banners = banners;
        Portraits = new Sprite[racePresets.Length][];
        for (int i = 0; i < Portraits.GetLength(0); i++)
        {
            Portraits[i] = racePresets[i].race.sprites;
        }
        RacePresets = racePresets;

        initialized = true;
    }

    public static Sprite GetPortraitByRace(int raceIndex, int portraitIndex)
    {
        return Portraits[raceIndex] [portraitIndex];
    }
}
