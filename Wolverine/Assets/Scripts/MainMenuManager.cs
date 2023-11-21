using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Slider slider;
    private int randomValue;
    public GameObject startButton;
    void Start()
    {
        slider.value = 0;
        startButton.SetActive(false);
}

    // Update is called once per frame
    void Update()
    {

        randomValue = Random.Range(18,23);

      
        slider.value += randomValue * Time.deltaTime;
        if (slider.value >= 100)
        {
            slider.gameObject.SetActive(false);
            StartGame();
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("CombatScene");
    }
}
