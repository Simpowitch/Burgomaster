using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResourcePanelUI : MonoBehaviour
{
    public Sprite[] sprites;

    public TextMeshProUGUI valueText;
    public TextMeshProUGUI netIncomeText;
    public Image image;

    public void Setup(ResourceType type)
    {
        image.sprite = sprites[(int)type];
    }

    public void UpdatePanel(int value, int income, int expense)
    {
        valueText.text = value.ToString();

        netIncomeText.enabled = true;
        int difference = income - expense;
        string differenceText = difference > 0 ? "+ " : "";
        netIncomeText.text = differenceText + difference.ToString();
    }

    public void UpdatePanel(int value)
    {
        valueText.text = value.ToString();
        if (netIncomeText != null)
            netIncomeText.enabled = false;
    }

    public void Show(bool show)
    {
        this.gameObject.SetActive(show);
    }
}
