using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BossData")]
public class BossData : ScriptableObject
{
    public GameObject attackOnePrefab; // Attack prefab one
    public GameObject attackTwoPrefab; // Attack prefab two
    public GameObject attackThreePrefab; // Attack prefab three

    // More can be added here if needed
}
