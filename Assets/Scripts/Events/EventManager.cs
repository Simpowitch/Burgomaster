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


    List<Scourge> activeScourges = new List<Scourge>();

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
        //Limit to the max number of scourges
        if (activeScourges.Count < maxScourges)
        {
            if (Utility.RandomizeBool(scourgeChance))
            {
                ScourgePreset preset = Utility.ReturnRandom(allScourges);
                Scourge newScourge = new Scourge(preset.scourge);
                activeScourges.Add(newScourge);
            }
        }

        foreach (var scourge in activeScourges)
        {
            player.RemoveResources(scourge.activeResourceDrain);
            scourge.onCooldown = false;
        }

        UpdateUI();
    }

    Scourge selectedScourge;
    public void ShowScourge(int index)
    {
        selectedScourge = activeScourges[index];

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
    }

    public void EventChoice(int choiceIndex)
    {
        selectedScourge.onCooldown = true;

        //TODO: Resolve selected Scourge/Event
        //PAY For choice etc.
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
