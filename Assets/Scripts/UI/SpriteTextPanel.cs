using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SpriteTextPanel : MonoBehaviour
{
    public Text text;
    public TextMeshProUGUI textPro;
    public Image image;

    public void Setup(string text, Sprite sprite)
    {
        if (this.text)
        {
            this.text.text = text;
        }
        if (textPro)
        {
            textPro.text = text;
        }
        image.sprite = sprite;
    }

    public void SetActive(bool value) => this.gameObject.SetActive(value);
}
