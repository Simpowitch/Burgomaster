using UnityEngine;
using TMPro;

public class ResourcePanelUI : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI netIncomeText;

    public void UpdatePanel(int value, int income, int expense)
    {
        valueText.text = value.ToString();
        int difference = income - expense;

        string differenceText = difference > 0 ? "+ " : "";
        netIncomeText.text = differenceText + difference.ToString();
    }
}
