using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorCardModiferContainer
{
    // Minor card modifiers container 

    [System.Serializable]
    public struct Modifier // Single modifier 
    {
        public float modiferValue;
        public StatModifier.StatModType modiferType;
        public int order;
    }
    [System.Serializable]
    public struct MultiModifer // Multiple modifiers
    {
        public List<Modifier> mods;
    }
}
