using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelZoneItemManager : MonoBehaviour
{
    [NonSerialized] public bool zoneSelected = false; // Is a zone selected
    [SerializeField] private GameObject levelZoneItemPrefab; // UI item prefab
    [SerializeField] private GameObject zoneOneContentObj; // Where to spawn level 1 content
    [SerializeField] private GameObject zoneTwoContentObj; // Where to spawn level 2 content
    [SerializeField] private GameObject zoneThreeContentObj; // Where to spawn level 3 content
    public Vector2Int currentZoneID; // x = level, y = zone

    public Scrollbar levelOneRect; // Level 1 scrollbar
    public Scrollbar levelTwoRect; // Level 2 scrollbar
    public Scrollbar levelThreeRect; // Level 3 scrollbar

    private void Start()
    {
        // Get refs
        var levelsList = LevelManager.instance.levelDB.Levels;
        List<LevelZone> levelOneZones = new List<LevelZone>();
        List<LevelZone> levelTwoZones = new List<LevelZone>();
        List<LevelZone> levelThreeZones = new List<LevelZone>();

        for (int i = 0; i < levelsList.Count; i++)
        {
            switch (i)
            {
                case 0:
                    levelOneZones = (levelsList[i].ZoneList);
                    break;

                case 1:
                    levelTwoZones = (levelsList[i].ZoneList);
                    break;

                case 2:
                    levelThreeZones = (levelsList[i].ZoneList);
                    break;
            }
        }

        // Generate content
        for (int i = 0; i < levelOneZones.Count; i++)
        {
            var newMenuItem = Instantiate(levelZoneItemPrefab, zoneOneContentObj.transform);
            LevelZoneItem controller = newMenuItem.GetComponent<LevelZoneItem>();
            controller.listManager = this;
            controller.levelAndZoneID = new Vector2Int(0, i);
            controller.nameText.text = "" + levelOneZones[i].name + " " + i;
        }
        for (int i = 0; i < levelTwoZones.Count; i++)
        {
            var newMenuItem = Instantiate(levelZoneItemPrefab, zoneTwoContentObj.transform);
            LevelZoneItem controller = newMenuItem.GetComponent<LevelZoneItem>();
            controller.listManager = this;
            controller.levelAndZoneID = new Vector2Int(1, i);
            controller.nameText.text = "" + levelTwoZones[i].name + " " + i;
        }
        for (int i = 0; i < levelThreeZones.Count; i++)
        {
            var newMenuItem = Instantiate(levelZoneItemPrefab, zoneThreeContentObj.transform);
            LevelZoneItem controller = newMenuItem.GetComponent<LevelZoneItem>();
            controller.listManager = this;
            controller.levelAndZoneID = new Vector2Int(2, i);
            controller.nameText.text = "" + levelThreeZones[i].name + " " + i;
        }

        // Set scroll bars to top
        levelOneRect.value = 1;
        levelTwoRect.value = 1;
        levelThreeRect.value = 1;
    }

    public void OnSelectLevel()
    {
        if (currentZoneID.x == -1 || currentZoneID.y == -1) return; // Guard Clause

        LevelManager.instance.ForceLoadZone(currentZoneID.x, currentZoneID.y);
    }
}
