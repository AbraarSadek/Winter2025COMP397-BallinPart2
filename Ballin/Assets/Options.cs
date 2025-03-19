using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    public GameObject mainOptionsPanel;
    public GameObject keybindsPanel;

    private void Start()
    {
        ShowMainOptions();
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
        SceneManager.LoadScene("MainMenu");
    }
}
