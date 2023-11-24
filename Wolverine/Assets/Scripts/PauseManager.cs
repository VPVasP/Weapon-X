using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public AudioSource ourMusic;
    public AudioSource pauseMusic;
    public GameObject pauseMenu;
    public GameObject controlMenu;
    public static PauseManager instance;
    public bool isPaused;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
  
    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        ourMusic.Pause();
        pauseMusic.Play();
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void ControlsMenu()
    {
        controlMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void MainMenuButtons()
    {

        controlMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void RestartLevel()
    {
        GameManager.instance.Restart();
        Time.timeScale = 1;   
    }
    public void Resume()
    {
        ourMusic.UnPause();
        pauseMusic.Stop();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }
  
  
    public void ExitGame()
    {
        Application.Quit();
    }
   
  
}
