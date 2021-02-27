using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Dice : MonoBehaviour
{
    public int DiceResult { get; private set; }
    public Action OnDiceResultChanged;

    // Array of dice sides sprites to load from Resources folder
    public Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    [SerializeField] Image rend;

    [SerializeField] int minIterations = 10, maxIterations = 40;
    [SerializeField] AnimationCurve diceSpeedInverted = null;
    [SerializeField] float speedModififer = 2f;

    [SerializeField] Transform rotationObject = null;

    // Coroutine that rolls the dice
    public IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Loops to switch dice sides ramdomly
        // before final side appears.
        int iterations = UnityEngine.Random.Range(minIterations, maxIterations + 1);

        float factor = 0f;

        for (int i = 0; i < iterations; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = UnityEngine.Random.Range(0, diceSides.Length);

            // Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            DiceResult = randomDiceSide + 1;
            OnDiceResultChanged?.Invoke();

            factor = i * 1f / iterations;
            float timeToNextIteration = diceSpeedInverted.Evaluate(factor) / speedModififer;

            rotationObject.eulerAngles = new Vector3(0, 0, factor * 360f);

            // Pause before next itteration
            yield return new WaitForSeconds(timeToNextIteration);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        DiceResult = randomDiceSide + 1;
        OnDiceResultChanged?.Invoke();

        // Show final dice value in Console
        Debug.Log(DiceResult);
    }
}

