using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneController : MonoBehaviour
{
    [SerializeField] private float loadSceneIn = 2.5f;


    // Loads set scene
    public void LoadScene()
    {
        StartCoroutine(LoadSceneIn());
    }

    IEnumerator LoadSceneIn()
    {
        yield return new WaitForSeconds(loadSceneIn);
        LevelManager.instance.NextLevel();
    }
}
