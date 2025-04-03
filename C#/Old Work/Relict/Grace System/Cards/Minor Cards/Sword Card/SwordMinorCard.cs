using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMinorCard : MinorCardBase
{
    private GameObject player; // Reference to player game object
    private WeaponStats weaponStats; // Reference to weapon stats

    #region Modifer Info
    [Header("Tier Modifiers")]
    [Tooltip("Expecting one modifier to apply to Attack Damage")]
    public MinorCardModiferContainer.MultiModifer tierOneModifier;
    [Tooltip("Expecting one modifier to apply to Attack Damage")]
    public MinorCardModiferContainer.MultiModifer tierTwoModifier;
    [Tooltip("Expecting one modifier to apply to Attack Damage")]
    public MinorCardModiferContainer.MultiModifer tierThreeModifier;
    [Tooltip("Expecting one modifier to apply to Attack Damage")]
    public MinorCardModiferContainer.MultiModifer tierFourModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Attack Damage and Element 1 will apply to Recharge Rate")]
    public MinorCardModiferContainer.MultiModifer tierFiveModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Attack Damage and Element 1 will apply to Recharge Rate")]
    public MinorCardModiferContainer.MultiModifer tierSixModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Attack Damage and Element 1 will apply to Recharge Rate")]
    public MinorCardModiferContainer.MultiModifer tierSevenModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Attack Damage and Element 1 will apply to Recharge Rate")]
    public MinorCardModiferContainer.MultiModifer tierEightModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Attack Damage and Element 1 will apply to Recharge Rate")]
    public MinorCardModiferContainer.MultiModifer tierNineModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Attack Damage, Element 1 will apply to Recharge Rate, and Element 2 will apply to Crit Chance")]
    public MinorCardModiferContainer.MultiModifer tierTenModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Attack Damage, Element 1 will apply to Recharge Rate, and Element 2 will apply to Crit Chance")]
    public MinorCardModiferContainer.MultiModifer tierElevenModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Attack Damage, Element 1 will apply to Recharge Rate, and Element 2 will apply to Crit Chance")]
    public MinorCardModiferContainer.MultiModifer tierTwelveModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Attack Damage, Element 1 will apply to Recharge Rate, and Element 2 will apply to Crit Chance")]
    public MinorCardModiferContainer.MultiModifer tierThirteenModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Attack Damage, Element 1 will apply to Recharge Rate, and Element 2 will apply to Crit Chance")]
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

        weaponStats = player.GetComponent<WeaponStats>();
    }

    private void Start()
    {
        var newStatModOne = AddAWeaponModifier(tierOneModifier, 0);
        weaponStats.Damage.AddModifier(newStatModOne);
    }

    // Gets all the stats that this card modifies and returns the values are a string along with the name
    public override string GetStats()
    {
        string result = "";

        float damageValue = Mathf.Abs(weaponStats.Damage.Value - weaponStats.Damage.BaseValue);
        if (damageValue != 0f) 
        {
            result += "Damage Bonus: \n+" + damageValue.ToString("0.00") + " \n\n";
        }

        float rateOfFireValue = Mathf.Abs(weaponStats.RateOfFire.Value - weaponStats.RateOfFire.BaseValue);
        if (rateOfFireValue != 0f) 
        {
            result += "Rate of Fire Bonus: \n+" + rateOfFireValue.ToString("0.00") + " \n\n";
        }

        float critChanceValue = Mathf.Abs(weaponStats.CritChance.Value - weaponStats.CritChance.BaseValue);
        if (critChanceValue != 0f) 
        {
            result += "Crit Chance Bonus: \n+" + critChanceValue.ToString("0.00") + " ";
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
                newStatModOne = AddAWeaponModifier(tierTwoModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierThree:
                newStatModOne = AddAWeaponModifier(tierThreeModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFour:
                newStatModOne = AddAWeaponModifier(tierFourModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFive:
                newStatModOne = AddAWeaponModifier(tierFiveModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierFiveModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSix:
                newStatModOne = AddAWeaponModifier(tierSixModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierSixModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSeven:
                newStatModOne = AddAWeaponModifier(tierSevenModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierSevenModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierEight:
                newStatModOne = AddAWeaponModifier(tierEightModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierEightModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierNine:
                newStatModOne = AddAWeaponModifier(tierNineModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierNineModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierTen:
                newStatModOne = AddAWeaponModifier(tierTenModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierTenModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);

                newStatModThree = AddAWeaponModifier(tierTenModifier, 2);
                weaponStats.CritChance.RemoveAllModifiersFromSource(this);
                weaponStats.CritChance.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierEleven:
                newStatModOne = AddAWeaponModifier(tierElevenModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierElevenModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);

                newStatModThree = AddAWeaponModifier(tierElevenModifier, 2);
                weaponStats.CritChance.RemoveAllModifiersFromSource(this);
                weaponStats.CritChance.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierTwelve:
                newStatModOne = AddAWeaponModifier(tierTwelveModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierTwelveModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);

                newStatModThree = AddAWeaponModifier(tierTwelveModifier, 2);
                weaponStats.CritChance.RemoveAllModifiersFromSource(this);
                weaponStats.CritChance.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierThirteen:
                newStatModOne = AddAWeaponModifier(tierThirteenModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierThirteenModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);

                newStatModThree = AddAWeaponModifier(tierThirteenModifier, 2);
                weaponStats.CritChance.RemoveAllModifiersFromSource(this);
                weaponStats.CritChance.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierFourteen:
                newStatModOne = AddAWeaponModifier(tierFourteenModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierFourteenModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);

                newStatModThree = AddAWeaponModifier(tierFourteenModifier, 2);
                weaponStats.CritChance.RemoveAllModifiersFromSource(this);
                weaponStats.CritChance.AddModifier(newStatModThree);
                break;
        }
        print(GetStats());
        return 1; // Success
    }

    // Downgrades card
    public override int Downgrade()
    {
        if (weaponStatModifierStack.Count <= 0 || currentTier == 1)
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
                newStatModOne = AddAWeaponModifier(tierOneModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierTwo:
                newStatModOne = AddAWeaponModifier(tierTwoModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierThree:
                newStatModOne = AddAWeaponModifier(tierThreeModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFour:
                newStatModOne = AddAWeaponModifier(tierFourModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFive:
                newStatModOne = AddAWeaponModifier(tierFiveModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierFiveModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSix:
                newStatModOne = AddAWeaponModifier(tierSixModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierSixModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSeven:
                newStatModOne = AddAWeaponModifier(tierSevenModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierSevenModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierEight:
                newStatModOne = AddAWeaponModifier(tierEightModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierEightModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierNine:
                newStatModOne = AddAWeaponModifier(tierNineModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierNineModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);

                weaponStats.CritChance.RemoveAllModifiersFromSource(this);
                break;

            case MinorCardTier.TierTen:
                newStatModOne = AddAWeaponModifier(tierTenModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierTenModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);

                newStatModThree = AddAWeaponModifier(tierTenModifier, 2);
                weaponStats.CritChance.RemoveAllModifiersFromSource(this);
                weaponStats.CritChance.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierEleven:
                newStatModOne = AddAWeaponModifier(tierElevenModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierElevenModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);

                newStatModThree = AddAWeaponModifier(tierElevenModifier, 2);
                weaponStats.CritChance.RemoveAllModifiersFromSource(this);
                weaponStats.CritChance.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierTwelve:
                newStatModOne = AddAWeaponModifier(tierTwelveModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierTwelveModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);

                newStatModThree = AddAWeaponModifier(tierTwelveModifier, 2);
                weaponStats.CritChance.RemoveAllModifiersFromSource(this);
                weaponStats.CritChance.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierThirteen:
                newStatModOne = AddAWeaponModifier(tierThirteenModifier, 0);
                weaponStats.Damage.RemoveAllModifiersFromSource(this);
                weaponStats.Damage.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierThirteenModifier, 1);
                weaponStats.RateOfFire.RemoveAllModifiersFromSource(this);
                weaponStats.RateOfFire.AddModifier(newStatModTwo);

                newStatModThree = AddAWeaponModifier(tierThirteenModifier, 2);
                weaponStats.CritChance.RemoveAllModifiersFromSource(this);
                weaponStats.CritChance.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierFourteen:
                break;
        }
        print(GetStats());
        return 1; // Success
    }
}
