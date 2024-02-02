using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorCardBase : MonoBehaviour
{
    // Minor card tier enum
    public enum MinorCardTier
    {
        TierOne = 1, TierTwo = 2, TierThree = 3, TierFour = 4, 
        TierFive = 5, TierSix = 6, TierSeven = 7, TierEight = 8, 
        TierNine = 9, TierTen = 10, TierEleven = 11, TierTwelve = 12,
        TierThirteen = 13, TierFourteen = 14
    }

    [System.NonSerialized] public MinorCardTier currentCardTier = MinorCardTier.TierOne; // Current tier of card
    [System.NonSerialized] public int currentTier = 1; // Current tier of card as an int


    protected Stack<StatModifier> weaponStatModifierStack = new Stack<StatModifier>(); // Stack of all weapon modifiers this card has active
    protected Stack<StatModifier> playerStatModifierStack = new Stack<StatModifier>(); // Stack of all player modifiers this card has active


    private void Awake()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    public virtual int Upgrade()
    {
        // Upgrades card to next tier
        return 0;
    }
    public virtual int Downgrade()
    {
        // Downgrades card to previous tier
        return 0;
    }

    public virtual string GetStats()
    {
        // Returns card stats
        return "Empty";
    }
}
