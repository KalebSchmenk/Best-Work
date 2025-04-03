using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelProgressionManager : MonoBehaviour
{
    [SerializeField] private Image _blackImg;
    [SerializeField] private Animator _anim;

    private enum CurrentLevel
    {
        CombatLevel,
        PuzzleLevel,
        FinalLevel,
        HUBLevel
    }

    [Header("Current Scene")]
    [SerializeField] private CurrentLevel _currentLevel;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            {
                switch (_currentLevel)
                {
                    case CurrentLevel.CombatLevel:

                        GameManager._completedCombatLevel = true;

                        StartCoroutine(Fading());

                        //SceneManager.LoadScene("HUBLevel");

                        break;

                    case CurrentLevel.PuzzleLevel:

                        GameManager._completedPuzzleLevel = true;

                        StartCoroutine(Fading());

                        //SceneManager.LoadScene("HUBLevel");

                        break;

                    case CurrentLevel.FinalLevel:

                        GameManager._completedFinalLevel = true;

                        StartCoroutine(Fading());

                        //SceneManager.LoadScene("HUBLevel");

                        break;

                    case CurrentLevel.HUBLevel:

                        GameManager._hasCompletedTutorial = true;

                        break;
                }
            }
        }
    }

    private IEnumerator Fading()
    {
        _anim.SetBool("Fade", true);
        yield return new WaitUntil(()=>_blackImg.color.a == 1);
        SceneManager.LoadScene("HubLevel");
    }
}
