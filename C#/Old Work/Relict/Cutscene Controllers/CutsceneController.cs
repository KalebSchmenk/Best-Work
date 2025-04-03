using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer; // Video player ref
    [SerializeField] int buildIndexToLoad = 0; // Loads level at this build index
    float vidLength; // Length of video held in order to load the level the second the video ends
   

    private void Start()
    {
        LockCursor();
        vidLength = (float)videoPlayer.length;
        Invoke("LoadLevel", vidLength); // Call the load level function the second the video ends
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.InteractInputPressed)
        {
            print("Skipping cutscene :(");
            LoadLevel(); // Skips cutscene
        }
    }

    // Loads level
    public void LoadLevel()
    {
        SceneManager.LoadScene(buildIndexToLoad);
    }

    // Locks cursor
    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
