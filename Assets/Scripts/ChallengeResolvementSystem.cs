using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ChallengeResolvementSystem : MonoBehaviour
{
    private enum State { Setup, Rolling, Done}

    public Dice dice;
    public GameObject challengeResolvementMainPanel;
    public TextMeshProUGUI challengeText, modifierText, rollText, totalText;
    public Button rollButton, diceButton, cancelButton;
    public Image modifierShield, challengeShield;
    public Sprite[] modifierShields = new Sprite[6], challengeShields = new Sprite[6];

    int DiceResult => dice.DiceResult;
    int ResultTotal => DiceResult + statModifier;
    int challengeRating;
    int statModifier;

    public Action OnSucess, OnFail;


    private void Start()
    {
        dice.OnDiceResultChanged += UpdateNumbers;
    }

    void UpdateNumbers()
    {
        rollText.text = DiceResult.ToString();
        totalText.text = ResultTotal.ToString();
    }

    public void SetupChallenge(int challengeRating, int statModifier, AbilityScore abilityScore)
    {
        ChangeState(State.Setup);

        challengeResolvementMainPanel.SetActive(true);

        //Save for result calculation
        this.challengeRating = challengeRating;
        this.statModifier = statModifier;

        //Display numbers
        challengeText.text = challengeRating.ToString();
        modifierText.text = $"+ {statModifier}";

        //Update shields
        modifierShield.sprite = modifierShields[(int)abilityScore];
        challengeShield.sprite = challengeShields[(int)abilityScore];

        UpdateNumbers();
    }

    public void RollDice() => StartCoroutine(RollDiceCoroutine());

    private IEnumerator RollDiceCoroutine()
    {
        ChangeState(State.Rolling);
        yield return dice.RollTheDice();
        ChangeState(State.Done);
    }

    private void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.Setup:
                cancelButton.gameObject.SetActive(false);

                rollButton.gameObject.SetActive(true);
                rollButton.interactable = true;
                diceButton.interactable = true;
                break;
            case State.Rolling:
                rollButton.interactable = false;
                diceButton.interactable = false;
                break;
            case State.Done:
                cancelButton.gameObject.SetActive(true);

                rollButton.gameObject.SetActive(false);
                break;
        }
    }

    public void Dismiss()
    {
        SendResult();
        challengeResolvementMainPanel.SetActive(false);
    }

    private void SendResult()
    {
        if (ResultTotal >= challengeRating)
        {
            Debug.Log("Success");
            OnSucess?.Invoke();
        }
        else
        {
            Debug.Log("Fail");
            OnFail?.Invoke();
        }
    }
}
