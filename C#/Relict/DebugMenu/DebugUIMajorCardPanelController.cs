using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUIMajorCardPanelController : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels; // List of panels

    int currentPanel = 0; // Iterator

    // Goes to next panel for Major Card selecting
    public void NextPanel()
    {
        panels[currentPanel].SetActive(false);

        currentPanel++;
        if (currentPanel >= panels.Count) // Rolls back to first panel
        {
            currentPanel = 0;
        }

        panels[currentPanel].SetActive(true);
    }

    // Goes to previous panel for Major Card selecting

    public void PreviousPanel()
    {
        panels[currentPanel].SetActive(false);

        currentPanel--;
        if (currentPanel < 0) // Noves forward to last panel
        {
            currentPanel = panels.Count - 1;
        }

        panels[currentPanel].SetActive(true);
    }
}
