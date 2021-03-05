using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChallengeResolvementSystem : MonoBehaviour
{
    private enum State { Setup, Rolling, Done}

    public Dice dice;
    public GameObject challengeResolvementMainPanel;
    public TextMeshProUGUI challengeText, modifierText, totalText;
    public Button diceButton, cancelButton;
    public Image modifierShield;
    public Sprite[] modifierShields = new Sprite[6], challengeShields = new Sprite[6];

    int DiceResult => dice.DiceResult;
    int ResultTotal => DiceResult + statModifier;
    int challengeRating;
    int statModifier;

    public Action OnSucessConfirmed, OnFailConfirmed;
    public UnityEvent OnSucessRolled, OnFailRolled;

    private void Start()
    {
        dice.OnDiceResultChanged += UpdateNumbers;
    }

    void UpdateNumbers()
    {
        totalText.text = ResultTotal.ToString();
    }

    public void SetupChallenge(int challengeRating, int statModifier, AbilityScore abilityScore)
    {
        //Reset old actions
        OnSucessConfirmed = null;
        OnFailConfirmed = null;

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

                diceButton.interactable = true;
                break;
            case State.Rolling:
                diceButton.interactable = false;
                break;
            case State.Done:
                cancelButton.gameObject.SetActive(true);

                if (ResultTotal >= challengeRating)
                    OnSucessRolled?.Invoke();
                else
                    OnFailRolled?.Invoke();
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
            OnSucessConfirmed?.Invoke();
        }
        else
        {
            Debug.Log("Fail");
            OnFailConfirmed?.Invoke();
        }
    }
}
