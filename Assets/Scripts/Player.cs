using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    public int startGold = 1000;

    CityResource gold;
    public TextMeshProUGUI goldText;


    private void Start()
    {
        gold = new CityResource { resourceType = ResourceType.Gold, value = startGold };

        gold.OnValueChanged += delegate (object sender, CityResource.OnResourceChangeArgs e) 
        { 
            goldText.text = e.value.ToString(); 
        };
    }
}

public enum ResourceType { Gold }
public class CityResource
{
    public class OnResourceChangeArgs : EventArgs
    {
        public int value;
    }

    public event EventHandler<OnResourceChangeArgs> OnValueChanged;
    public int value;
    public ResourceType resourceType;
    public bool IsAffordable(int request) => request <= value;
    public void AddValue(int add)
    {
        value += add;
        OnValueChanged?.Invoke(this, new OnResourceChangeArgs { value = value });
    }
}
