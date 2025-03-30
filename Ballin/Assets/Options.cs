using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public GameObject mainOptionsPanel;
    public GameObject optionsPanel;
    public GameObject keybindsPanel;
    public GameObject mainMenu;
    public Slider mainVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public AudioMixer audioMixer;


    private void Start()
    {
        ShowMainOptions();
    }

    public void setMainVolume()
    {
        float volume = mainVolumeSlider.value;
        audioMixer.SetFloat("Master", Mathf.Log10(volume)*20);
    
    }
    public void setMusicVolume()
    {
        float volume = musicVolumeSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }
    public void setSFXVolume()
    {
        float volume = sfxVolumeSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
    public void ShowMainOptions()
    {
        keybindsPanel.SetActive(false);
        mainOptionsPanel.SetActive(true);
    }

    public void ShowKeybinds()
    {
        mainOptionsPanel.SetActive(false);
        keybindsPanel.SetActive(true);
    }

    //Back To Settings
    public void Settings()
    {
        optionsPanel.SetActive(false);
        mainMenu.SetActive(true);
    }
}
