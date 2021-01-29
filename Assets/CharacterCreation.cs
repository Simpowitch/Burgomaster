using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCreation : MonoBehaviour
{
    public TextMeshProUGUI raceText;
    public Image characterVisualsRenderer;
    public TextMeshProUGUI strength, constitution, wisdom, intelligence, diplomacy, charisma;

    public RacePreset[] racePresets;
    RacePreset currentRacePreset;

    int currentRacePresetIndex = 0;
    int currentSpriteIndex = 0;

    private void Start()
    {
        currentRacePresetIndex = Random.Range(0, racePresets.Length);
        SetRacePreset(racePresets[currentRacePresetIndex]);
    }

    public void ChangeRace(bool next)
    {
        RacePreset newRacePreset = next ? racePresets.Next(ref currentRacePresetIndex) : racePresets.Previous(ref currentRacePresetIndex);
        SetRacePreset(newRacePreset);
    }

    public void ChangeCharacterVisuals(bool next)
    {
        Sprite newSprite = next ? currentRacePreset.sprites.Next(ref currentSpriteIndex) : currentRacePreset.sprites.Previous(ref currentSpriteIndex);
        characterVisualsRenderer.sprite = newSprite;
    }

    private void SetRacePreset(RacePreset racePreset)
    {
        currentRacePreset = racePreset;

        raceText.text = currentRacePreset.race.ToString();

        currentSpriteIndex = 0;
        characterVisualsRenderer.sprite = currentRacePreset.sprites[currentSpriteIndex];

        strength.text = currentRacePreset.strength.ToString();
        constitution.text = currentRacePreset.constitution.ToString();
        wisdom.text = currentRacePreset.wisdom.ToString();
        intelligence.text = currentRacePreset.intelligence.ToString();
        diplomacy.text = currentRacePreset.diplomacy.ToString();
        charisma.text = currentRacePreset.charisma.ToString();
    }
}
