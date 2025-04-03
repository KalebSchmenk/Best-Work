using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] GameObject mainMenuObject;
    [SerializeField] GameObject howtoplayObject;
    [SerializeField] AudioSource menuSounds;
    [SerializeField] AudioClip buttonPressClip;
    [SerializeField] GameObject optionsObject;
    [SerializeField] GameObject kbmControlView;
    [SerializeField] GameObject gamepadControlView;
    [SerializeField] GameObject quitConformObject;
    [SerializeField] GameObject creditsObject;

    [Header("First Selected Buttons")]
    [SerializeField] GameObject menuFirstButton;
    [SerializeField] GameObject optionFirstButton;
    [SerializeField] GameObject htpFirstButton;
    [SerializeField] GameObject creditsFirstButton;
    [SerializeField] GameObject quitFirstSelected;

    [Header("Main Menu Buttons")]
    [SerializeField] GameObject optionsButton;
    [SerializeField] GameObject htpButton;
    [SerializeField] GameObject creditsButton;
    [SerializeField] GameObject quitButton;
    // Start is called before the first frame update

    private void Start() {
        menuSounds.clip = buttonPressClip;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstButton);

    }
    public void StartGame()
    {
        GameManager._hasCompletedTutorial = false;
        GameManager._completedCombatLevel = false;
        GameManager._completedPuzzleLevel = false;
        GameManager._completedFinalLevel = false;

        StartCoroutine(SoundBeforeSceneChange("StartCutscene"));
    }

    public void QuitGame(){
        menuSounds.Play();               
        quitConformObject.SetActive(true);
        mainMenuObject.SetActive(false);
        howtoplayObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(quitFirstSelected);
    }


    public void HowToPlay(){
        menuSounds.Play();
        mainMenuObject.SetActive(false);
        howtoplayObject.SetActive(true);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(htpFirstButton);
    }

    public void BackButton(){
        menuSounds.Play();
        howtoplayObject.SetActive(false);
        optionsObject.SetActive(false);
        mainMenuObject.SetActive(true);
        creditsObject.SetActive(false);
        quitConformObject.SetActive(false);
    }


    public void OptionsBackButton(){
        menuSounds.Play();
        howtoplayObject.SetActive(false);
        optionsObject.SetActive(false);
        mainMenuObject.SetActive(true);
        creditsObject.SetActive(false);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsButton);
    }

    public void HTPBackButton(){
        menuSounds.Play();
        howtoplayObject.SetActive(false);
        optionsObject.SetActive(false);
        mainMenuObject.SetActive(true);
        creditsObject.SetActive(false);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(htpButton);
    }
    public void CreditsBackButton(){
        menuSounds.Play();
        howtoplayObject.SetActive(false);
        optionsObject.SetActive(false);
        mainMenuObject.SetActive(true);
        creditsObject.SetActive(false);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsButton);
    }

    public void QuitBackButton(){
        menuSounds.Play();
        howtoplayObject.SetActive(false);
        optionsObject.SetActive(false);
        mainMenuObject.SetActive(true);
        creditsObject.SetActive(false);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(quitButton);
    }

    private IEnumerator SoundBeforeSceneChange(string scene){
        menuSounds.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        if(scene == "quit"){
            Application.Quit();   
        }
        else{
            SceneManager.LoadScene(scene);
        }
    }

    public void Options(){
        menuSounds.Play();
        mainMenuObject.SetActive(false);
        optionsObject.SetActive(true);
        quitConformObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionFirstButton);
    }

     public void KBMControlSwitch() {
        menuSounds.Play();
        kbmControlView.SetActive(true);
        gamepadControlView.SetActive(false);
        }

    public void GamepadControlSwitch() {
        menuSounds.Play();
        gamepadControlView.SetActive(true);
        kbmControlView.SetActive(false);
        }

    public void QuitConfirm(){
        StartCoroutine(SoundBeforeSceneChange("quit"));
    }

    public void Credits()
    {
        menuSounds.Play();
        creditsObject.SetActive(true);
        mainMenuObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsFirstButton);
    }

}
