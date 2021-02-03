using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public TextMeshProUGUI turntext;
    public int day = 1;
    public int month = 1;
    public int year = 1510;

    const int DAYSPERMONTH = 3;
    const int MONTHSPERYEAH = 4;

    public UnityEvent OnNewTurn;

    public void ChangeTurn()
    {
        day++;

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
        OnNewTurn?.Invoke();
        turntext.text = $"{day} / {month} / {year}";
    }
}
