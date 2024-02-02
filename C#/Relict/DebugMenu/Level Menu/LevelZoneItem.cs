using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class LevelZoneItem : MonoBehaviour
{
    [NonSerialized] public LevelZoneItemManager listManager;
    [SerializeField] private GameObject selectedHighlight;
    public TextMeshProUGUI nameText;
    public Vector2Int levelAndZoneID;
    private bool isSelected = false;

    // On this selected
    public void OnSelect()
    {
        if (isSelected) // If already selected
        {
            // Turn off
            selectedHighlight.SetActive(false);
            isSelected = false;

            listManager.zoneSelected = false;
            listManager.currentZoneID = new Vector2Int (-1, -1);
        }
        else // If not selected
        {
            if (listManager.zoneSelected) return; // Something else is already selected

            // Tell manager we are now selected
            selectedHighlight.SetActive(true);
            isSelected = true;
            listManager.zoneSelected = true;
            listManager.currentZoneID = levelAndZoneID;
        }
    }
}
