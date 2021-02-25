using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public int DiceResult { get; private set; }

    // Array of dice sides sprites to load from Resources folder
    public Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    [SerializeField] Image rend;

    [SerializeField] int minIterations = 10, maxIterations = 40;
    [SerializeField] float timeBetweenIterations = 0.05f;


    // Coroutine that rolls the dice
    public IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        DiceResult = 1;

        // Loop to switch dice sides ramdomly
        // before final side appears.
        int iterations = Random.Range(minIterations, maxIterations + 1);

        for (int i = 0; i < iterations; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, diceSides.Length);

            // Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            DiceResult = randomDiceSide + 1;

            // Pause before next itteration
            yield return new WaitForSeconds(timeBetweenIterations);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        DiceResult = randomDiceSide + 1;

        // Show final dice value in Console
        Debug.Log(DiceResult);
    }
}

