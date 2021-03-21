using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Toggle tutorialToggle;
    public GameObject tutorialScene, mainMenu;

    private float tutorialTimer;

    void Start(){
        audioMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 0.75f)) * 20);
        audioMixer.SetFloat("SFXVol", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 0.75f)) * 20);
        tutorialTimer = .1f;
        tutorialToggle.isOn = PlayerPrefs.GetInt("tutorial", 1) != 0;
    }

    public void Update() {
        if(tutorialScene.activeSelf) {
            if(tutorialTimer <= 0) {
                if(Input.anyKey) {
                    PlayerPrefs.SetInt("tutorial", 0);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            } else {
                tutorialTimer -= Time.deltaTime;
            }
        }
    }

    public void PlayGame(){
        if(!tutorialToggle.isOn)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else {
            tutorialScene.SetActive(true);
            mainMenu.SetActive(false);
        }
    }

    public void SetTogglePrefab(Toggle change) {
        if(change.isOn)
            PlayerPrefs.SetInt("tutorial", 1);
        else
            PlayerPrefs.SetInt("tutorial", 0);
    }

    public void QuitGame(){
        Application.Quit();
    }

    
}
