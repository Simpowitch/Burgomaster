using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string masterVolumeParam = "MasterVolume", musicVolumeParam = "MusicVolume", ambienceVolumeParam = "AmbienceVolume", sfxVolumeParam = "SFXVolume", uiVolumeParam = "UIVolume";
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionTexts = new List<string>();

    int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            resolutionTexts.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(resolutionTexts);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }


    public void SetMasterVolume(float volume) => audioMixer.SetFloat(masterVolumeParam, Mathf.Log10(volume) * 20);
    public void SetMusicVolume(float volume) => audioMixer.SetFloat(musicVolumeParam, Mathf.Log10(volume) * 20);
    public void SetAmbienceVolume(float volume) => audioMixer.SetFloat(ambienceVolumeParam, Mathf.Log10(volume) * 20);
    public void SetSFXVolume(float volume) => audioMixer.SetFloat(sfxVolumeParam, Mathf.Log10(volume) * 20);
    public void SetUIVolume(float volume) => audioMixer.SetFloat(uiVolumeParam, Mathf.Log10(volume) * 20);

    public void ToggleFullscreen(bool value) => Screen.fullScreen = value;
    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
