using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Slider slider;
    private int randomValue;
    public GameObject mainButtons;
    public GameObject howToPlayButtonsMenu;
    private bool isSliderLoading = false;
    void Start()
    {
        slider.value = 0;
      slider.gameObject.SetActive(false);
    }

    public void BeginGameButton()
    {
        mainButtons.SetActive(false);
        slider.gameObject.SetActive(true);
        isSliderLoading = true;
    }
    public void LoadHowToPlayButtons()
    {
        howToPlayButtonsMenu.SetActive(true);
        mainButtons.SetActive(false);
    }
    public void ReturnToMainButtons()
    {
        howToPlayButtonsMenu.SetActive(false);
        mainButtons.SetActive(true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("CombatScene");
    }
    private void Update()
    {
        if (isSliderLoading == true)
        {
            SliderLoading();
        }
    }
    public void SliderLoading()
    {
        randomValue = Random.Range(18, 23);
        slider.value += randomValue * Time.deltaTime;
        if (slider.value >= 100)
        {
            slider.gameObject.SetActive(false);
            StartGame();
            isSliderLoading = false;
        }
    }
}
