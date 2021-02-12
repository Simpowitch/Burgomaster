using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    public int startGold = 1000;

    CityResource gold;
    CityResource goldIncomes;
    CityResource goldExpenses;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI goldNetIncomeText;


    private void Start()
    {
        gold = new CityResource { resourceType = ResourceType.Gold, value = startGold };
        goldIncomes = new CityResource { resourceType = ResourceType.Gold, value = 0 };
        goldExpenses = new CityResource { resourceType = ResourceType.Gold, value = 0 };

        goldText.text = gold.value.ToString();

        TurnManager.OnNewTurnBegun += NewTurn;
    }

    private void NewTurn(object sender, TurnManager.OnTurnEventArgs e)
    {
        SimulateEconomy();
    }

    #region Economy
    private void SimulateEconomy()
    {
        int goldDifference = goldIncomes.value - goldExpenses.value;

        if (goldDifference > 0)
            gold.AddValue(goldDifference);
        else
            gold.RemoveValue(goldDifference);
        UpdateEconomyUI();
    }

    private void UpdateEconomyUI()
    {
        goldText.text = gold.value.ToString();
        int goldDifference = goldIncomes.value - goldExpenses.value;

        string goldDifferenceText = goldDifference > 0 ? "+ " : "";
        goldNetIncomeText.text = goldDifferenceText + goldDifference.ToString();
    }

    public void AddResources(List<CityResource> cityResources)
    {
        foreach (var resource in cityResources)
        {
            switch (resource.resourceType)
            {
                case ResourceType.Gold:
                    gold.AddValue(resource.value);
                    break;
            }
        }
        UpdateEconomyUI();
    }

    public void RemoveResources(List<CityResource> cityResources)
    {
        foreach (var resource in cityResources)
        {
            switch (resource.resourceType)
            {
                case ResourceType.Gold:
                    gold.RemoveValue(resource.value);
                    break;
            }
        }
        UpdateEconomyUI();
    }

    public void IncreaseTurnIncome(List<CityResource> cityResources)
    {
        foreach (var resource in cityResources)
        {
            switch (resource.resourceType)
            {
                case ResourceType.Gold:
                    goldIncomes.AddValue(resource.value);
                    break;
            }
        }
        UpdateEconomyUI();
    }

    public void IncreaseTurnExpenses(List<CityResource> cityResources)
    {
        foreach (var resource in cityResources)
        {
            switch (resource.resourceType)
            {
                case ResourceType.Gold:
                    goldExpenses.AddValue(resource.value);
                    break;
            }
        }
        UpdateEconomyUI();
    }
    #endregion
}

public enum ResourceType { Gold }
[System.Serializable]
public class CityResource
{
    const int MINVALUE = 0;
    
    public int value;
    public ResourceType resourceType;
    public bool IsAffordable(int request) => request <= value;
    public void AddValue(int add)
    {
        value += add;
    }

    public void RemoveValue(int remove)
    {
        value -= remove;
        value = Mathf.Max(value, MINVALUE);
    }
}
