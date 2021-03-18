using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationInspector : MonoBehaviour
{
    public TextMeshProUGUI title, description;
    public Image icon, background;
    public AudioSource audioPlayer;

    public void Setup(NotificationInformation info)
    {
        this.gameObject.SetActive(true);
        this.title.text = info.title;
        this.description.text = info.description;
        this.icon.sprite = info.icon;
        this.background.sprite = info.panelBackground;

        audioPlayer.clip = info.inspectSound;
        audioPlayer.Play();
    }

    private void OnDisable()
    {
        audioPlayer.Stop();
    }
}
