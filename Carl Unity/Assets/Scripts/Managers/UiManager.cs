using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AI))]
public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI speedometer, chronometer, placement, placementLetters;
    public MovementController player;
    public static bool GameIsPaused = false;
    public static bool GameInSettings = false;
    public GameObject pauseMenuUI;

    private AI ai;
    private float timer;
    private AudioSource[] audioSources;

    void Start()
    {
        ai = GetComponent<AI>();
        placement.text = ((int)ai.carNumber+1).ToString();
        timer = 0;
        audioSources = FindObjectsOfType<AudioSource>();
    }

    void Update()
    {
        if(GameManager.over)
            return;

        UpdatePlacement();
        UpdateChronometer();
        UpdateSpeedometer();
    }

    private void UpdatePlacement() {
        var count = 1;
        foreach (var car in ai.cars)
        {
            if(car.transform.position.x > player.transform.position.x)
                count++;
        }
        placement.text = count.ToString();

        if(count == 1) {
            placementLetters.text = "ST";
        } else if (count == 2) {
            placementLetters.text = "ND";
        } else if (count == 3) {
            placementLetters.text = "RD";
        } else {
            placementLetters.text = "TH";
        }
    }

    private void UpdateChronometer() {
        if(GameManager.begin) {
            timer += Time.deltaTime;
            chronometer.text = timer.ToString("F1");
        }
    }

    private void UpdateSpeedometer() {
        speedometer.text = (player.GetSpeed() / player.maxSpeed * 100).ToString("F1");
    }
    
    public void Resume(){
        GameIsPaused = false;
        Time.timeScale = 1f;
        foreach (var source in audioSources)
        {
            if(source.time != 0)
                source.Play();
        }
        pauseMenuUI.SetActive(false);
    }

    public void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        foreach (var source in audioSources)
        {
            source.Pause();
        }
        GameIsPaused = true;
    }

    public void GoSettings(){
        GameInSettings = true;
    }

    public void GoSettingsOut(){
        GameInSettings = false;
    }

    public void GoMenu(){
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
