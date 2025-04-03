using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialGameControls : MonoBehaviour
{
    public KeyCode freezeGame = KeyCode.P;
    public KeyCode exitGame = KeyCode.Escape;

    bool gameFrozen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(freezeGame))
        {
            if (gameFrozen)
            {
                // Unfreeze game
                UnFreezeGame();
            }
            else
            {
                // Freeze game
                FreezeGame();
            }
        }

        if (Input.GetKeyDown(exitGame))
        {
            print("Game quick closed");
            //Application.Quit();
        }
    }

    private void FreezeGame()
    {
        gameFrozen = true;
        Time.timeScale = 0f;
    }

    private void UnFreezeGame()
    {
        gameFrozen = false;
        Time.timeScale = 1f;
    }
}
