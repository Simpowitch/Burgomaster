using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string masterVolumeParam = "MasterVolume", musicVolumeParam = "MusicVolume", sfxVolumeParam = "SFXVolume", uiVolumeParam = "UIVolume";
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


    public void SetMasterVolume(float volume) => audioMixer.SetFloat(masterVolumeParam, volume);
    public void SetMusicVolume(float volume) => audioMixer.SetFloat(musicVolumeParam, volume);
    public void SetSFXVolume(float volume) => audioMixer.SetFloat(sfxVolumeParam, volume);
    public void SetUIVolume(float volume) => audioMixer.SetFloat(uiVolumeParam, volume);

    public void ToggleFullscreen(bool value) => Screen.fullScreen = value;
    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
