using System.Collections;
using System.Collections.Generic;
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
    public int turn = 1;
    public int day = 1;
    public int month = 1;
    public int year = 1510;

    const int DAYSPERMONTH = 3;
    const int MONTHSPERYEAH = 4;

    public static event EventHandler<OnTurnEventArgs> OnNewTurnBegun;

    public void ChangeTurn()
    {
        day++;
        turn++;
        OnNewTurnBegun?.Invoke(this, new OnTurnEventArgs { turn = turn });

        if (day > DAYSPERMONTH)
        {
            day = 1;
            month++;
            if (month > MONTHSPERYEAH)
            {
                month = 1;
                year++;
            }
        }
        turntext.text = $"{day} / {month} / {year}";
    }
}
