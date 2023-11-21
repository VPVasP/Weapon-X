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
    public AudioSource myFX;
    public AudioClip hoverOverFX;
    public AudioClip clickFX;
    [SerializeField] Slider volumeSlider;
    public TextMeshProUGUI dropdownText;
    public static PauseManager instance;
    public bool isPaused;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }
    }
    public void HoverSound()
    {
        myFX.PlayOneShot(hoverOverFX);
    }
    public void ClickSound()
    {
        myFX.PlayOneShot(clickFX);
    }
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
  
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }
    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
   
    public void WindowedMode()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }
    public void FullWindow()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
}
