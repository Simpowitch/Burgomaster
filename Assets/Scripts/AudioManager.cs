using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonHoover, buttonClick, confirm, close;

    public void PlayButtonHoover() => audioSource.PlayOneShot(buttonHoover);
    public void PlayButtonClick() => audioSource.PlayOneShot(buttonClick);
    public void PlayConfirm() => audioSource.PlayOneShot(confirm);
    public void PlayClose() => audioSource.PlayOneShot(close);
}
