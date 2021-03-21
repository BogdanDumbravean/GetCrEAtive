using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider sliderMusic;
    public Slider sliderSFX;


    void Start()
    {
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }

    public void SetVolume(float volume){
        
        audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetVolumeSFX(float volume){
        audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
