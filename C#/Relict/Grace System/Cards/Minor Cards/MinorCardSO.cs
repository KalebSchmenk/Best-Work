using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Cards/Minor Card")]
public class MinorCardSO : ScriptableObject
{
    public string cardName; // Card name

    public string cardDescription; // Card description

    public Sprite cardImage; // Card sprite

    public GameObject cardPrefab; // Card prefab with script component
}
