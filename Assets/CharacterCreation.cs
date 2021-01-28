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

    public Character[] characters;
    Character currentCharacter;
    int currentCharacterVisualIndex = 0;

    private void Start()
    {
        SetCharacter(Random.Range(0, characters.Length));
    }

    public void ChangeCharacter(bool next)
    {
        int newIndex = currentCharacterVisualIndex;

        if (next)
        {
            newIndex++;
            if (newIndex >= characters.Length)
                newIndex = 0;
        }
        else
        {
            newIndex--;
            if (newIndex < 0)
                newIndex = characters.Length - 1;
        }
        SetCharacter(newIndex);
    }


    private void SetCharacter(int index)
    {
        currentCharacterVisualIndex = index;
        currentCharacter = characters[index];

        raceText.text = currentCharacter.race.ToString();
        characterVisualsRenderer.sprite = currentCharacter.sprite;

        strength.text = currentCharacter.strength.ToString();
        constitution.text = currentCharacter.constitution.ToString();
        wisdom.text = currentCharacter.wisdom.ToString();
        intelligence.text = currentCharacter.intelligence.ToString();
        diplomacy.text = currentCharacter.diplomacy.ToString();
        charisma.text = currentCharacter.charisma.ToString();
    }
}
