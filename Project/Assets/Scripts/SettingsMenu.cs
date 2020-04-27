using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu instance;
    private void Awake()
    {
        //Audio Design
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public AudioMixer mainMixer;
    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVolume", volume);
    }
    public void SetFXVolume(float volume)
    {
        mainMixer.SetFloat("FXVolume", volume);
    }
    public void SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("MasterVolume", volume);
    }
}
