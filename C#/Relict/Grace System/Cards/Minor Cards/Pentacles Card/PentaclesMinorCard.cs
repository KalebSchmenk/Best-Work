using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentaclesMinorCard : MinorCardBase
{
    private GameObject player; // Reference to player game object
    private PlayerStats playerStats; // Reference to player stats

    #region Modifer Info
    [Header("Tier Modifiers")]
    [Tooltip("Expecting one modifier to apply to Minor Arcana Drop Rate")]
    public MinorCardModiferContainer.MultiModifer tierOneModifier;
    [Tooltip("Expecting one modifier to apply to Minor Arcana Drop Rate")]
    public MinorCardModiferContainer.MultiModifer tierTwoModifier;
    [Tooltip("Expecting one modifier to apply to Minor Arcana Drop Rate")]
    public MinorCardModiferContainer.MultiModifer tierThreeModifier;
    [Tooltip("Expecting one modifier to apply to Minor Arcana Drop Rate")]
    public MinorCardModiferContainer.MultiModifer tierFourModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Minor Arcana Drop Rate and Element 1 will apply to Discount On Purchase")]
    public MinorCardModiferContainer.MultiModifer tierFiveModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Minor Arcana Drop Rate and Element 1 will apply to Discount On Purchase")]
    public MinorCardModiferContainer.MultiModifer tierSixModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Minor Arcana Drop Rate and Element 1 will apply to Discount On Purchase")]
    public MinorCardModiferContainer.MultiModifer tierSevenModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Minor Arcana Drop Rate and Element 1 will apply to Discount On Purchase")]
    public MinorCardModiferContainer.MultiModifer tierEightModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Minor Arcana Drop Rate and Element 1 will apply to Discount On Purchase")]
    public MinorCardModiferContainer.MultiModifer tierNineModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Minor Arcana Drop Rate, Element 1 will apply to Discount On Purchase, and Element 2 will apply to Money Doubling Chance")]
    public MinorCardModiferContainer.MultiModifer tierTenModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Minor Arcana Drop Rate, Element 1 will apply to Discount On Purchase, and Element 2 will apply to Money Doubling Chance")]
    public MinorCardModiferContainer.MultiModifer tierElevenModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Minor Arcana Drop Rate, Element 1 will apply to Discount On Purchase, and Element 2 will apply to Money Doubling Chance")]
    public MinorCardModiferContainer.MultiModifer tierTwelveModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Minor Arcana Drop Rate, Element 1 will apply to Discount On Purchase, and Element 2 will apply to Money Doubling Chance")]
    public MinorCardModiferContainer.MultiModifer tierThirteenModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Minor Arcana Drop Rate, Element 1 will apply to Discount On Purchase, and Element 2 will apply to Money Doubling Chance")]
    public MinorCardModiferContainer.MultiModifer tierFourteenModifier;
    protected StatModifier AddAPlayerModifier(MinorCardModiferContainer.MultiModifer modifer, int modIndex)
    {
        StatModifier newStat = new StatModifier(modifer.mods[modIndex].modiferValue, modifer.mods[modIndex].modiferType, modifer.mods[modIndex].order, this);
        playerStatModifierStack.Push(newStat);
        return newStat;
    }

    protected StatModifier AddAWeaponModifier(MinorCardModiferContainer.MultiModifer modifer, int modIndex)
    {
        StatModifier newStat = new StatModifier(modifer.mods[modIndex].modiferValue, modifer.mods[modIndex].modiferType, modifer.mods[modIndex].order, this);
        weaponStatModifierStack.Push(newStat);
        return newStat;
    }
    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        print(this + " added to player.");

        playerStats = player.GetComponent<PlayerStats>();
    }

    private void Start()
    {
        var newStatModOne = AddAPlayerModifier(tierOneModifier, 0);
        playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);
    }

    // Gets all the stats that this card modifies and returns the values are a string along with the name
    public override string GetStats()
    {
        string result = "";

        float minorArcanaDropValue = Mathf.Abs(playerStats.MinorArcanaDropRate.Value - playerStats.MinorArcanaDropRate.BaseValue);
        if (minorArcanaDropValue != 0f) 
        {
            result += "Minor Arcana Drop Chance: \n+" + minorArcanaDropValue.ToString("0.00") + " \n\n";
        }

        float discountValue = Mathf.Abs(playerStats.DiscountOnPurchase.Value - playerStats.DiscountOnPurchase.BaseValue);
        if (discountValue != 0f) 
        {
            result += "Purchase Discount: \n" + discountValue.ToString("0.00") + " \n\n";
        }

        float doubleMoneyValue = Mathf.Abs(playerStats.ChanceToDoubleMoney.Value - playerStats.ChanceToDoubleMoney.BaseValue);
        if (doubleMoneyValue != 0f)
        {
            result += "Double Money Chance: \n+" + doubleMoneyValue.ToString("0.00") + " ";
        }

        return result;
    }

    // Upgrades card
    public override int Upgrade()
    {
        if (currentTier >= 14) return 0;

        print(this + " got upgraded!");
        currentTier++;
        currentCardTier = (MinorCardTier)currentTier; // Cast tier enum to type

        StatModifier newStatModOne;
        StatModifier newStatModTwo;
        StatModifier newStatModThree;

        // Switch statement that applies correct modifiers depending on what the tier is
        switch (currentCardTier)
        {
            case MinorCardTier.TierOne:
                break;

            case MinorCardTier.TierTwo:
                newStatModOne = AddAPlayerModifier(tierTwoModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierThree:
                newStatModOne = AddAPlayerModifier(tierThreeModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFour:
                newStatModOne = AddAPlayerModifier(tierFourModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFive:
                newStatModOne = AddAPlayerModifier(tierFiveModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierFiveModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSix:
                newStatModOne = AddAPlayerModifier(tierSixModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierSixModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSeven:
                newStatModOne = AddAPlayerModifier(tierSevenModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierSevenModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierEight:
                newStatModOne = AddAPlayerModifier(tierEightModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierEightModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierNine:
                newStatModOne = AddAPlayerModifier(tierNineModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierNineModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierTen:
                newStatModOne = AddAPlayerModifier(tierTenModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierTenModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierTenModifier, 2);
                playerStats.ChanceToDoubleMoney.RemoveAllModifiersFromSource(this);
                playerStats.ChanceToDoubleMoney.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierEleven:
                newStatModOne = AddAPlayerModifier(tierElevenModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierElevenModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierElevenModifier, 2);
                playerStats.ChanceToDoubleMoney.RemoveAllModifiersFromSource(this);
                playerStats.ChanceToDoubleMoney.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierTwelve:
                newStatModOne = AddAPlayerModifier(tierTwelveModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierTwelveModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierTwelveModifier, 2);
                playerStats.ChanceToDoubleMoney.RemoveAllModifiersFromSource(this);
                playerStats.ChanceToDoubleMoney.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierThirteen:
                newStatModOne = AddAPlayerModifier(tierThirteenModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierThirteenModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierThirteenModifier, 2);
                playerStats.ChanceToDoubleMoney.RemoveAllModifiersFromSource(this);
                playerStats.ChanceToDoubleMoney.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierFourteen:
                newStatModOne = AddAPlayerModifier(tierFourteenModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierFourteenModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierFourteenModifier, 2);
                playerStats.ChanceToDoubleMoney.RemoveAllModifiersFromSource(this);
                playerStats.ChanceToDoubleMoney.AddModifier(newStatModThree);
                break;
        }
        print(GetStats());
        return 1; // Success
    }

    // Downgrades card
    public override int Downgrade()
    {
        if (playerStatModifierStack.Count <= 0 || currentTier == 1)
        {
            print("This card has no modifiers!");
            return 0; // Fail
        }

        print(this + " got downgraded!");
        currentTier--;
        currentCardTier = (MinorCardTier)currentTier; // Cast current tier number to the enum type

        StatModifier newStatModOne;
        StatModifier newStatModTwo;
        StatModifier newStatModThree;

        // Switch statement that removes modifiers depending on what the tier now is
        switch (currentCardTier)
        {
            case MinorCardTier.TierOne:
                newStatModOne = AddAPlayerModifier(tierOneModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierTwo:
                newStatModOne = AddAPlayerModifier(tierTwoModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierThree:
                newStatModOne = AddAPlayerModifier(tierThreeModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFour:
                newStatModOne = AddAPlayerModifier(tierFourModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFive:
                newStatModOne = AddAPlayerModifier(tierFiveModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierFiveModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSix:
                newStatModOne = AddAPlayerModifier(tierSixModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierSixModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSeven:
                newStatModOne = AddAPlayerModifier(tierSevenModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierSevenModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierEight:
                newStatModOne = AddAPlayerModifier(tierEightModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierEightModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierNine:
                newStatModOne = AddAPlayerModifier(tierNineModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierNineModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);

                playerStats.ChanceToDoubleMoney.RemoveAllModifiersFromSource(this);
                break;

            case MinorCardTier.TierTen:
                newStatModOne = AddAPlayerModifier(tierTenModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierTenModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierTenModifier, 2);
                playerStats.ChanceToDoubleMoney.RemoveAllModifiersFromSource(this);
                playerStats.ChanceToDoubleMoney.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierEleven:
                newStatModOne = AddAPlayerModifier(tierElevenModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierElevenModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierElevenModifier, 2);
                playerStats.ChanceToDoubleMoney.RemoveAllModifiersFromSource(this);
                playerStats.ChanceToDoubleMoney.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierTwelve:
                newStatModOne = AddAPlayerModifier(tierTwelveModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierTwelveModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierTwelveModifier, 2);
                playerStats.ChanceToDoubleMoney.RemoveAllModifiersFromSource(this);
                playerStats.ChanceToDoubleMoney.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierThirteen:
                newStatModOne = AddAPlayerModifier(tierThirteenModifier, 0);
                playerStats.MinorArcanaDropRate.RemoveAllModifiersFromSource(this);
                playerStats.MinorArcanaDropRate.AddModifier(newStatModOne);

                newStatModTwo = AddAPlayerModifier(tierThirteenModifier, 1);
                playerStats.DiscountOnPurchase.RemoveAllModifiersFromSource(this);
                playerStats.DiscountOnPurchase.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierThirteenModifier, 2);
                playerStats.ChanceToDoubleMoney.RemoveAllModifiersFromSource(this);
                playerStats.ChanceToDoubleMoney.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierFourteen:      
                break;
        }
        print(GetStats());
        return 1; // Success
    }
}
