using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThemeSelector : MonoBehaviour
{
    List<SpriteButton> themeButtons = new List<SpriteButton>();
    public SpriteButton themeButtonBP;
    public Transform viewportContent;

    public void Setup(Sprite[] sprites, Action[] actions)
    {
        if (actions.Length != sprites.Length)
        {
            Debug.LogError("Wrongfully setup theme selector. The number of button actions does not correspond to the number of sprites");
            SetActive(false);
            return;
        }

        foreach (var item in themeButtons)
        {
            Destroy(item.gameObject);
        }
        themeButtons.Clear();

        for (int i = 0; i < sprites.Length; i++)
        {
            SpriteButton newButton = Instantiate(themeButtonBP, viewportContent);
            newButton.Setup(sprites[i], actions[i]);
            themeButtons.Add(newButton);
        }
        SetActive(true);
    }

    public void SetActive(bool value) => this.gameObject.SetActive(value);
}
