using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{ public GameObject optionsMenu;
    public GameObject mainMenu;
    //Load Scene
    public Audio_Manager audioManager;
    public void Play()
    {
        Debug.Log("Loading Level");
        SceneManager.LoadScene("Level");
    }

    //Opens Options Menu
    public void Options()
    {
        optionsMenu.SetActive(true);    
        mainMenu.SetActive(false);


    }

    //Quit Game
    public void Quit()
    {
        Application.Quit();
    }
    private void Start()
    {
        audioManager.randomizeMainMenu();
    }
}