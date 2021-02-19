using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SessionCreator : MonoBehaviour
{
    public TextMeshProUGUI raceText;
    public TextMeshProUGUI goldText;
    public Image characterVisualsRenderer;
    public AbilityScoreBlockUI abilityScoreBlockUI;
    public Image bannerVisualsRenderer;
    public TextMeshProUGUI backgroundNameText;
    public TextMeshProUGUI backgroundDescriptionText;

    RacePreset[] racePresets;
    public BackgroundPreset[] backgroundPresets;
    Sprite[] banners;

    Race ChosenRace => racePresets[raceIndex].race;
    Background ChosenBackground => backgroundPresets[backgroundIndex].background;

    int startGold;

    string leaderName;
    AbilityScoreBlock abilityScoreBlock;
    int raceIndex = 0;
    int portraitIndex = 0;
    int backgroundIndex = 0;
    int bannerIndex = 0;

    private void Start()
    {
        racePresets = LeaderDatabase.RacePresets;
        raceIndex = Random.Range(0, racePresets.Length);
        SetRace(ChosenRace);

        backgroundIndex = Random.Range(0, backgroundPresets.Length);
        SetBackground(ChosenBackground);

        banners = LeaderDatabase.Banners;
        bannerIndex = Random.Range(0, banners.Length);
        bannerVisualsRenderer.sprite = banners[bannerIndex];
    }

    public void ChangeRace(bool next)
    {
        Race newRace = next ? racePresets.Next(ref raceIndex).race : racePresets.Previous(ref raceIndex).race;
        SetRace(newRace);
    }

    private void SetRace(Race race)
    {
        raceText.text = race.raceName;

        portraitIndex = 0;
        characterVisualsRenderer.sprite = race.sprites[portraitIndex];

        SetAbilityScores(race, ChosenBackground);
    }

    public void ChangeBackground(bool next)
    {
        Background newBackground = next ? backgroundPresets.Next(ref backgroundIndex).background : backgroundPresets.Previous(ref backgroundIndex).background;
        SetBackground(newBackground);
    }

    private void SetBackground(Background background)
    {
        backgroundNameText.text = background.backgroundTitle;
        backgroundDescriptionText.text = background.backgroundDescription;

        startGold = background.startCity.gold;

        goldText.text = startGold.ToString();

        SetAbilityScores(ChosenRace, background);
    }

    public void ChangeCharacterVisuals(bool next)
    {
        Sprite newSprite = next ? ChosenRace.sprites.Next(ref portraitIndex) : ChosenRace.sprites.Previous(ref portraitIndex);
        characterVisualsRenderer.sprite = newSprite;
    }

    public void ChangeBanner(bool next)
    {
        Sprite newSprite = next ? banners.Next(ref bannerIndex) : banners.Previous(ref bannerIndex);
        bannerVisualsRenderer.sprite = newSprite;
    }

    public void SetLeaderName(string value)
    {
        leaderName = value;
    }

    private void SetAbilityScores(Race racePreset, Background backgroundPreset)
    {
        abilityScoreBlock = new AbilityScoreBlock()
        {
            authoritarian = racePreset.abilityScores.authoritarian + backgroundPreset.scoreImprovements.authoritarian,
            cunning = racePreset.abilityScores.cunning + backgroundPreset.scoreImprovements.cunning,
            diplomatic = racePreset.abilityScores.diplomatic + backgroundPreset.scoreImprovements.diplomatic,
            liberal = racePreset.abilityScores.liberal + backgroundPreset.scoreImprovements.liberal,
            nature = racePreset.abilityScores.nature + backgroundPreset.scoreImprovements.nature,
            religious = racePreset.abilityScores.religious + backgroundPreset.scoreImprovements.religious,
        };

        //Display abilityscores
        abilityScoreBlockUI.UpdateUI(abilityScoreBlock);
    }

    public void StartNewSession()
    {
        CityData cityData = new CityData(ChosenBackground.startCity.gold, ChosenBackground.startCity.wood, ChosenBackground.startCity.food, ChosenBackground.startCity.iron);
        LeaderData leaderData = new LeaderData(leaderName, abilityScoreBlock, raceIndex, backgroundIndex, portraitIndex, bannerIndex);
        SaveData saveData = new SaveData(leaderData, cityData);
        ActiveSessionHolder.ActiveSession = saveData;
    }
}
