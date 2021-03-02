using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider sliderEffects;
    public Slider sliderMusic;
    public AudioMixer audioMixer;
    public Toggle toggle;
    private int currentToggleState;

    private void Start()
    {
        sliderEffects.value = PlayerPrefs.GetFloat("effectsValue",1f);
        sliderMusic.value = PlayerPrefs.GetFloat("musicValue", 1f);
        toggle.isOn = PlayerPrefs.GetInt("FullScreenToggle") == 1 ? true : false;
    }
    public void SetVolumeMusic(float music)
    {   
        audioMixer.SetFloat("music", Mathf.Log10(music) * 20);
        PlayerPrefs.SetFloat("musicValue", music);
        
    }

    public void SetVolumeEffects(float effects)
    {
        
        audioMixer.SetFloat("effects", Mathf.Log10(effects) * 20);
        PlayerPrefs.SetFloat("effectsValue", effects);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        
        if (toggle.isOn == true)
        {
            currentToggleState = 1;
        }
        else
        {
            currentToggleState = 0;
        }

        PlayerPrefs.SetInt("FullScreenToggle", currentToggleState);
    }


}
