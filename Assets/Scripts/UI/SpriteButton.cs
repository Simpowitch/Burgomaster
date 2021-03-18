using UnityEngine;
using System;
using UnityEngine.UI;

public class SpriteButton : MonoBehaviour
{
    private Action onClick;
    public Image spriteRenderer;

    public void Setup(Sprite sprite, Action onClick)
    {
        spriteRenderer.sprite = sprite;
        this.onClick = onClick;
    }

    public void Click() => onClick?.Invoke();
}
