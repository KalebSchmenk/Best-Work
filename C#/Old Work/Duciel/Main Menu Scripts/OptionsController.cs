using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{
    public static float _screenBrightness = 1;
    public static float _soundEffectsVolume = 0.5f;
    public static float _musicVolume = 0.5f;
    public static float _xSensitivityValue;
    public static float _ySensitivityValue;
    public static float _sensSliderValue = 0.5f;
    [Header("Slider Objects")]
    [SerializeField] Slider brightnessSliderObject;
    [SerializeField] Slider sfxSliderObject;
    [SerializeField] Slider musicSliderObject;
    [SerializeField] Slider sensSliderObject;
    [Header("Brightness and sensitivty")]
    [SerializeField] Volume volume;
    ColorAdjustments colorAdjustments;
    public CinemachineFreeLook cinemachineFL;
    float camBaseY = 0.001f;
    float camBaseX = 0.1f;
    float defaultSensSlider = 0.5f;
    float defaultMusicVolume = 0.5f;
    float defaultSFXVolume = 0.5f;
    float defaultBrightness = 1f;
    

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        ScreenBrightnessSlider(OptionsController._screenBrightness);
        brightnessSliderObject.value = OptionsController._screenBrightness;
        MusicVolumeSlider(OptionsController._musicVolume);
        musicSliderObject.value = OptionsController._musicVolume;
        SFXVolumeSlider(OptionsController._soundEffectsVolume);
        sfxSliderObject.value = OptionsController._soundEffectsVolume;
        SensitivitySlider(OptionsController._sensSliderValue);
        sensSliderObject.value = OptionsController._sensSliderValue;
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if(sceneName != "MainMenu"){
            cinemachineFL.m_XAxis.m_MaxSpeed = _xSensitivityValue;
            cinemachineFL.m_YAxis.m_MaxSpeed = _ySensitivityValue;
        }

        


    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScreenBrightnessSlider(float value){
        _screenBrightness = value;
            if(volume.profile.TryGet<ColorAdjustments>(out colorAdjustments)){
            colorAdjustments.postExposure.value = _screenBrightness;
        }
        
        

    }

    public void MusicVolumeSlider(float value){
        _musicVolume = value;
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audioSources){
            if(a.tag == "Music"){
                a.volume = _musicVolume;
                
                
            }
        }
    }

    public void SFXVolumeSlider(float value){
        _soundEffectsVolume = value;
        AudioSource[] audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach (AudioSource a in audioSources){
            if(a.tag == "SFX"){
                a.volume = _soundEffectsVolume;
                
            }
        }

    }

    public void SensitivitySlider(float value){
        _sensSliderValue = value;
        _xSensitivityValue = camBaseX * value;
        _ySensitivityValue = camBaseY * value;

    }

    public void ResetSettings(){
        SFXVolumeSlider(defaultSFXVolume);
        sfxSliderObject.value = OptionsController._soundEffectsVolume;
        SensitivitySlider(defaultSensSlider);
        sensSliderObject.value = OptionsController._sensSliderValue;
        MusicVolumeSlider(defaultMusicVolume);
        musicSliderObject.value = OptionsController._musicVolume;
        ScreenBrightnessSlider(defaultBrightness);
        brightnessSliderObject.value = OptionsController._screenBrightness;
    }
}
