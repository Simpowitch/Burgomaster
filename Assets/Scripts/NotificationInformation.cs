using UnityEngine;

[System.Serializable]
public class NotificationInformation
{
    public string title;
    [TextArea(2, 8)]
    public string description;
    public Sprite icon, panelBackground;
    public AudioClip inspectSound, listPopupSound;
}
