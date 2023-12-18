using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    //our audio refrences
    public AudioSource ourMusic;
    public AudioSource pauseMusic;
    //our ui refrences that we set activate or false
    public GameObject pauseMenu;
    public GameObject controlMenu;
    public static PauseManager instance;
    public bool isPaused; //bool to check if it has been paused the game
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
  
    public void Update()
    {
        //if we press the escape key and it is not paused we can pause
        if (Input.GetKey(KeyCode.Escape) && isPaused==false) 
        {
            Pause();
        }
        //if we press the escape key and it is  paused we can resume the game
        else if (Input.GetKey(KeyCode.Escape) && isPaused==true){
            Resume();
        }
    }
    //we pause the game and we set the time scale to 0 so it freezes everything and pauses main music and plays pause music
    public void Pause()
    {
        isPaused = true;
        ourMusic.Pause();
        pauseMusic.Play();
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    //we enable the howtoplay menu and disable the pause menu ui
    public void ControlsMenu()
    {
        controlMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }
    //we disable the howtoplay menu and enable the pause menu ui
    public void PauseMenuButtons()
    {

        controlMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    //we restart the game and we set the time scale to 1
    public void RestartLevel()
    {
        GameManager.instance.Restart();
        Time.timeScale = 1;   
    }
    //we resume the game and play the correct music and set the timescale to 1 to unfreeze everything
    public void Resume()
    {
        ourMusic.UnPause();
        pauseMusic.Stop();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }
  
  //we quit the application
    public void ExitGame()
    {
        Application.Quit();
    }
   
  
}
