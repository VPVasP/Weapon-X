using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Slider slider; //our loading game slider
    private int randomValue; //the random value 
    public GameObject mainButtons; //the main menu buttons
    public GameObject howToPlayButtonsMenu; //how to play menu buttons
    private bool isSliderLoading = false; //bool to check if it is loading
    void Start()
    {
        slider.value = 0;
      slider.gameObject.SetActive(false);
    }
    //begin game function that handles the ui 
    public void BeginGameButton()
    {
        mainButtons.SetActive(false);
        slider.gameObject.SetActive(true);
        isSliderLoading = true;
    }
    //loading the howtoplay menu and disabling the main menu
    public void LoadHowToPlayButtons()
    {
        howToPlayButtonsMenu.SetActive(true);
        mainButtons.SetActive(false);
    }
    //returning to the main menu ui
    public void ReturnToMainButtons()
    {
        howToPlayButtonsMenu.SetActive(false);
        mainButtons.SetActive(true);
    }
    //we load the scene to begin the game
    public void StartGame()
    {
        SceneManager.LoadScene("CombatScene");
    }
    private void Update()
    {
        //we call the slider loading function
        if (isSliderLoading == true)
        {
            SliderLoading();
        }
    }
    public void SliderLoading()
    {
        randomValue = Random.Range(18, 23); //we assign our randomvalue a random value from 18 to 23 to load the slider
        slider.value += randomValue * Time.deltaTime; //we update the slider each frame
        if (slider.value >= 100) //if the slider value is complete we call the startgame function
        {
            slider.gameObject.SetActive(false);
            StartGame();
            isSliderLoading = false;
        }
    }
}
