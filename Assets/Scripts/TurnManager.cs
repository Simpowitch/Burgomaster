using UnityEngine;
using TMPro;
using System;

public class TurnManager : MonoBehaviour
{
    public class OnTurnEventArgs : EventArgs
    {
        public int turn;
    }

    public TextMeshProUGUI turntext;

    public enum SeasonPart { Early, Mid, Late, END }
    public enum Season { Spring, Summer, Autumn, Winter, END }
    public SeasonPart seasonPart;
    public Season season;

    public int turn = 1;
    //public int day = 1;
    //public int month = 1;
    public int year = 1510;

    //const int DAYSPERMONTH = 3;
    //const int MONTHSPERYEAH = 4;

    
    public static event EventHandler<OnTurnEventArgs> OnEndTurn;
    public static event EventHandler<OnTurnEventArgs> OnUpdateConstruction;
    public static event EventHandler<OnTurnEventArgs> OnUpdateServiceEffects;
    public static event EventHandler<OnTurnEventArgs> OnUpdateAdvisors;
    public static event EventHandler<OnTurnEventArgs> OnProduceIncome;


    private void Start()
    {
        UpdateUI();
    }

    public void ChangeTurn()
    {
        OnEndTurn?.Invoke(this, new OnTurnEventArgs { turn = turn });
        OnProduceIncome?.Invoke(this, new OnTurnEventArgs { turn = turn });
        OnUpdateConstruction?.Invoke(this, new OnTurnEventArgs { turn = turn });
        OnUpdateServiceEffects?.Invoke(this, new OnTurnEventArgs { turn = turn });
        turn++;
        OnUpdateAdvisors?.Invoke(this, new OnTurnEventArgs { turn = turn });

        seasonPart++;
        if (seasonPart == SeasonPart.END)
        {
            seasonPart = SeasonPart.Early;
            season++;
            if (season == Season.END)
            {
                season = Season.Spring;
                year++;
            }
        }

        //day++;
        //if (day > DAYSPERMONTH)
        //{
        //    day = 1;
        //    month++;
        //    if (month > MONTHSPERYEAH)
        //    {
        //        month = 1;
        //        year++;
        //    }
        //}

        UpdateUI();
    }

    private void UpdateUI()
    {
        turntext.text = $"{seasonPart} {season} {year}";
        //turntext.text = $"{day} / {month} / {year}";
    }
}