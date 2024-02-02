using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;

public class PauseMenuController : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private InputAction pause;
    

    [SerializeField] AudioSource menuSoundsObject;
    [SerializeField] AudioClip buttonPressClip;


    [SerializeField] GameObject pauseMenu;

    private float storedCinemachineXSpeed;
    private float storedCinemachineYSpeed;
    public CinemachineFreeLook cinemachineFL;
    [SerializeField] GameObject playerObject;
    private PlayerController playerScript;
    private OptionsController optionsInfo;
    //bool _gameOver;
    bool _win;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioClip menuMusicClip;
    [SerializeField] GameObject howToPlayMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject gameOverMenu;
    float sensChangeX;
    float sensChangeY;
    [SerializeField] GameObject quitConformObject;


    [SerializeField] GameObject kbmControlView;
    [SerializeField] GameObject gamepadControlView;

 
    bool triggerOnce = true;

    

    [Header("Buttons")]
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject optionsButton;
    [SerializeField] GameObject htpButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject respawnButton;

    [Header("First Selected Buttons")]
    [SerializeField] GameObject optionsFirstButton;
    [SerializeField] GameObject htpFirstButton;
    [SerializeField] GameObject quitFirstButton;

    private void Awake() 
    {
        playerControls = new PlayerInputActions();
        storedCinemachineXSpeed = cinemachineFL.m_XAxis.m_MaxSpeed;
        storedCinemachineYSpeed = cinemachineFL.m_YAxis.m_MaxSpeed;
        playerScript = playerObject.GetComponent<PlayerController>();
        menuSoundsObject.clip = buttonPressClip;
        menuMusic.clip = menuMusicClip;
        
    }



    private void OnEnable() 
    {
        pause = playerControls.Player.Pause;
        pause.Enable();
    }

    private void OnDisable() 
    {
         if (pause != null) pause.Disable();
    
    }

    public void Update() 
    {
        //_gameOver = playerScript._gameOver;
        _win = playerScript._win;
            if(pause.triggered)
            {
                if (pauseMenu.activeSelf == false && gameOverMenu.activeSelf == false && _win == false && howToPlayMenu.activeSelf == false && optionsMenu.activeSelf == false && quitConformObject.activeSelf == false){
                PauseGame();
            }
            else{
                if(gameOverMenu.activeSelf == false && _win == false){
                    ResumeGame();
                    
                }   
            }
        }
        sensChangeX = OptionsController._xSensitivityValue;
        sensChangeY = OptionsController._ySensitivityValue;

        if(gameOverMenu.activeSelf == true){
            if(triggerOnce == true){
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(respawnButton);
                triggerOnce = false;
            }
        }
    }

    public void PauseGame()
    {


            Debug.Log("Game Paused");
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource a in audioSources){
                a.mute = true;
            }

            Debug.Log("Setting active");
            pauseMenu.SetActive(true);
            menuMusic.mute = false;
            menuMusic.Play();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            cinemachineFL.m_XAxis.m_MaxSpeed = 0.0f;
            cinemachineFL.m_YAxis.m_MaxSpeed = 0.0f;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton);

        
    }

    public void ResumeGame()
    {
        menuSoundsObject.Play();
        
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audioSources){
            a.mute = false;
        }
        menuMusic.mute = true;
        menuMusic.Stop();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
        optionsMenu.SetActive(false);
        quitConformObject.SetActive(false);
        gameOverMenu.SetActive(false);
        cinemachineFL.m_XAxis.m_MaxSpeed = sensChangeX;
        cinemachineFL.m_YAxis.m_MaxSpeed = sensChangeY;
        EventSystem.current.SetSelectedGameObject(null);
        triggerOnce = true;
        //_gameOver = false;
    }

    public void QuitConfirm()
    {
        StartCoroutine(SoundBeforeMainMenu());
    }

    private IEnumerator SoundBeforeMainMenu()
    {
        Time.timeScale = 1;
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene("MainMenu");
        cinemachineFL.m_XAxis.m_MaxSpeed = sensChangeX;
        cinemachineFL.m_YAxis.m_MaxSpeed = sensChangeY;
        
    }

    public void HowToPlay(){
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        howToPlayMenu.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(htpFirstButton);
    }

    public void BackButton(){
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        howToPlayMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        quitConformObject.SetActive(false);
    }

    public void OptionsBackButton(){
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        howToPlayMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsButton);
    }

    public void HTPBackButton(){
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        howToPlayMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(htpButton);
    }

    public void QuitBackButton(){
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        howToPlayMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(quitButton);
    }

    public void OptionsMenu(){
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        howToPlayMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        
        
    }

    public void QuitGame(){
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        howToPlayMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        quitConformObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(quitFirstButton);
    }

    public void KBMControlSwitch()
    {
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        kbmControlView.SetActive(true);
        gamepadControlView.SetActive(false);
    }

    public void GamepadControlSwitch()
    {
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
        gamepadControlView.SetActive(true);
        kbmControlView.SetActive(false);
    }

    public void ButtonPressSound(){
        menuSoundsObject.mute = false;
        menuSoundsObject.Play();
    }









}
