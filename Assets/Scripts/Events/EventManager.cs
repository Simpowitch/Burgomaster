using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] ScourgePreset[] allScourges = null;

    [SerializeField] Player player = null;
    [SerializeField] ChallengeResolvementSystem resolvementSystem = null;

    [Header("Settings")]
    [SerializeField] int maxScourges = 2;
    [SerializeField] [Range(0, 100)] int scourgeChance = 10;

    [Header("Visuals")]
    [SerializeField] GameObject[] scourgeButtons = null;
    [SerializeField] TextMeshProUGUI[] scourgeButtonTexts = null;

    [SerializeField] GameObject mainPanel = null;
    [SerializeField] Image eventImage = null;
    [SerializeField] TextMeshProUGUI eventTitle = null, eventDescription = null;
    [SerializeField] EventChoiceUI[] eventChoicePanels = null;
    [SerializeField] TextMeshProUGUI cancelButtonText = null;

    [SerializeField] string preChoiceCancel = "I'll attend this matter another time", postChoiceCancel = "Done";

    List<Scourge> activeScourges = new List<Scourge>();
    Scourge selectedScourge;

    private void OnEnable()
    {
        TurnManager.OnStartNewTurn += NewTurn;
    }

    private void OnDisable()
    {
        TurnManager.OnStartNewTurn -= NewTurn;
    }

    private void NewTurn(object sender, TurnManager.OnTurnEventArgs e)
    {
        foreach (var scourge in activeScourges)
        {
            player.RemoveResources(scourge.activeResourceDrain);
            scourge.onCooldown = false;
        }

        //Limit to the max number of scourges
        if (activeScourges.Count < maxScourges)
        {
            if (Utility.RandomizeBool(scourgeChance))
            {
                AddNewScourge();
            }
        }

        UpdateUI();
    }

    private void AddNewScourge()
    {
        Debug.Log("Adding new scourge");
        ScourgePreset preset = Utility.ReturnRandom(allScourges);
        Scourge newScourge = new Scourge(preset.scourge);
        activeScourges.Add(newScourge);
        player.ChangeTurnExpenses(newScourge.activeResourceDrain, true, newScourge.title);
        UpdateUI();
    }

    private void RemoveScourge(Scourge scourge)
    {
        activeScourges.Remove(scourge);
        player.ChangeTurnExpenses(scourge.activeResourceDrain, false, scourge.title);
        UpdateUI();
    }

    public void ShowScourge(int index) => ShowScourge(activeScourges[index]);

    private void ShowScourge(Scourge scourge)
    {
        selectedScourge = scourge;

        //Display event
        mainPanel.SetActive(true);

        eventTitle.text = selectedScourge.title;
        eventDescription.text = selectedScourge.description;
        eventImage.sprite = selectedScourge.eventSprite;

        for (int i = 0; i < eventChoicePanels.Length; i++)
        {
            if (i < selectedScourge.options.Length)
            {
                Resource[] cost = selectedScourge.options[i].cost;
                eventChoicePanels[i].Show(true);

                bool interactable = !selectedScourge.onCooldown && player.IsAffordable(cost);
                eventChoicePanels[i].UpdateUI(selectedScourge.options[i], interactable);
            }
            else
                eventChoicePanels[i].Show(false);
        }

        //Change default cancel-button text
        cancelButtonText.text = preChoiceCancel;
    }

    public void EventChoice(int choiceIndex)
    {
        selectedScourge.onCooldown = true;
        ShowScourge(selectedScourge);

        Scourge.Option choice = selectedScourge.options[choiceIndex];

        //Pay For choice
        player.RemoveResources(choice.cost);

        //Resolve selected Scourge/Event
        resolvementSystem.SetupChallenge(choice.challengeRating, player.GetAbilityScore(choice.checkType), choice.checkType);

        resolvementSystem.OnSucessConfirmed += () =>
        {
            //Show sucess message on event
            eventDescription.text = choice.sucessDescription;
            RemoveScourge(selectedScourge);
            selectedScourge = null;

            //Change default cancel-button text
            cancelButtonText.text = postChoiceCancel;
        };
        resolvementSystem.OnFailConfirmed += () =>
        {
            //Show fail message on event
            eventDescription.text = choice.failDescription;

            //Change default cancel-button text
            cancelButtonText.text = postChoiceCancel;
        };

        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < scourgeButtons.Length; i++)
        {
            bool active = activeScourges.Count > i;

            scourgeButtons[i].SetActive(active);

            if (active)
                scourgeButtonTexts[i].text = activeScourges[i].title;
        }
    }
}
