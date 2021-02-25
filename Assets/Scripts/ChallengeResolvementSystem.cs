using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ChallengeResolvementSystem : MonoBehaviour
{
    public Dice dice;
    public GameObject challengeResolvementMainPanel;
    public TextMeshProUGUI challengeText, modifierText;
    int DiceResult => dice.DiceResult;
    int ResultTotal => DiceResult + statModifier;
    int challengeRating;
    int statModifier;

    public Action OnSucess, OnFail;
    public float postResolvementWait = 2f;

    public void SetupChallenge(int challengeRating, int statModifier)
    {
        challengeResolvementMainPanel.SetActive(true);

        //Save for result calculation
        this.challengeRating = challengeRating;
        this.statModifier = statModifier;

        //Display numbers
        challengeText.text = challengeRating.ToString();
        modifierText.text = statModifier.ToString();
    }

    public void RollDice() => StartCoroutine(RollDiceCoroutine());

    private IEnumerator RollDiceCoroutine()
    {
        yield return dice.RollTheDice();
        SendResult();

        yield return new WaitForSeconds(postResolvementWait);
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
