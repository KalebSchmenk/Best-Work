using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Cards/Major Card")]
public class MajorCardSO : ScriptableObject
{
    // Types of Major Cards
    [Serializable]
    public enum MajorCardType
    {
        Weapon = 0,
        Movement = 1,
        Special = 2,
        Support = 3,
        Ultimate = 4,
        Passive = 5 // or 6
    }

    // Types of Major Card Arcana
    [Serializable]
    public enum MajorCardArcanaType
    {
        Strength,
        Death,
        Devil,
        World,
        Sun,
        Moon,
        Fool
    }


    public string cardName; // Card name

    public int cardID; // Card ID

    public MajorCardType cardType; // Card Type

    public MajorCardArcanaType arcanaType; // Arcana Type

    [TextArea] public string cardDescription; // Card description

    public Sprite cardImage; // Card sprite

    public GameObject cardPrefab; // Card prefab with script component
}
