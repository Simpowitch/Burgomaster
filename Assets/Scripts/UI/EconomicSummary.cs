using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomicSummary : MonoBehaviour
{
    List<Line> lines = new List<Line>();

    public Transform lineParent;
    public TextMeshProUGUI lineTextObjectBP;

    private void OnEnable()
    {
        if (lines.Count < 1)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void AddMessage(string newTitle, int change)
    {
        //Look for existing
        foreach (var line in lines)
        {
            if (line.title == newTitle)
            {
                line.value += change;
                line.duplicates++;
                UpdateUI();
                return;
            }
        }

        //Create new line
        Line newLine = new Line
        {
            title = newTitle,
            value = change,
            textObject = Instantiate(lineTextObjectBP, lineParent),
        };
        lines.Add(newLine);
        UpdateUI();
    }

    public void RemoveMessage(string messageToRemove, int changeToRemove)
    {
        //Look for existing
        Line lineToRemove = null;

        foreach (var line in lines)
        {
            if (line.title == messageToRemove)
            {
                if (line.duplicates == 0)
                {
                    Destroy(line.LineObject);
                    lineToRemove = line;
                }
                else
                {
                    line.value -= changeToRemove;
                    line.duplicates--;
                }
            }
        }
        lines.Remove(lineToRemove);
        UpdateUI();
    }

    void UpdateUI()
    {
        foreach (var line in lines)
        {
            line.UpdateUI();
        }
    }

    private class Line
    {
        public string title;
        public int value;
        public int duplicates;
        public GameObject LineObject => textObject.gameObject;
        public TextMeshProUGUI textObject;

        public void UpdateUI()
        {
            textObject.text = $"{title}: {value}";
        }
    }
}
