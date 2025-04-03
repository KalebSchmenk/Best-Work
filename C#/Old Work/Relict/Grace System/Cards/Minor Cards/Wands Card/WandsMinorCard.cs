using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandsMinorCard : MinorCardBase
{
    private GameObject player; // Reference to player game object
    private WeaponStats weaponStats; // Reference to weapon stats
    private PlayerStats playerStats; // Reference to player stats

    #region Modifer Info
    [Header("Tier Modifiers")]
    [Tooltip("Expecting one modifier to apply to Cooldown Reduction")]
    public MinorCardModiferContainer.MultiModifer tierOneModifier;
    [Tooltip("Expecting one modifier to apply to Cooldown Reduction")]
    public MinorCardModiferContainer.MultiModifer tierTwoModifier;
    [Tooltip("Expecting one modifier to apply to Cooldown Reduction")]
    public MinorCardModiferContainer.MultiModifer tierThreeModifier;
    [Tooltip("Expecting one modifier to apply to Cooldown Reduction")]
    public MinorCardModiferContainer.MultiModifer tierFourModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Cooldown Reduction and Element 1 will apply to Reload Speed")]
    public MinorCardModiferContainer.MultiModifer tierFiveModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Cooldown Reduction and Element 1 will apply to Reload Speed")]
    public MinorCardModiferContainer.MultiModifer tierSixModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Cooldown Reduction and Element 1 will apply to Reload Speed")]
    public MinorCardModiferContainer.MultiModifer tierSevenModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Cooldown Reduction and Element 1 will apply to Reload Speed")]
    public MinorCardModiferContainer.MultiModifer tierEightModifier;
    [Tooltip("Expecting two modifiers. Element 0 will apply to Cooldown Reduction and Element 1 will apply to Reload Speed")]
    public MinorCardModiferContainer.MultiModifer tierNineModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Cooldown Reduction, Element 1 will apply to Reload Speed, and Element 2 will apply to Jump Height")]
    public MinorCardModiferContainer.MultiModifer tierTenModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Cooldown Reduction, Element 1 will apply to Reload Speed, and Element 2 will apply to Jump Height")]
    public MinorCardModiferContainer.MultiModifer tierElevenModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Cooldown Reduction, Element 1 will apply to Reload Speed, and Element 2 will apply to Jump Height")]
    public MinorCardModiferContainer.MultiModifer tierTwelveModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Cooldown Reduction, Element 1 will apply to Reload Speed, and Element 2 will apply to Jump Height")]
    public MinorCardModiferContainer.MultiModifer tierThirteenModifier;
    [Tooltip("Expecting three modifiers. Element 0 will apply to Cooldown Reduction, Element 1 will apply to Reload Speed, and Element 2 will apply to Jump Height")]
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

        // Grabs all needed card info
        currentCardTier = (MinorCardTier)currentTier;
        weaponStats = player.GetComponent<WeaponStats>();
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void Start()
    {
        var newStatModOne = AddAPlayerModifier(tierOneModifier, 0);
        playerStats.BaseCooldown.AddModifier(newStatModOne);
    }

    // Gets all the stats that this card modifies and returns the values are a string along with the name
    public override string GetStats()
    {
        string result = "";

        float baseCooldownValue = Mathf.Abs(playerStats.BaseCooldown.Value - playerStats.BaseCooldown.BaseValue);
        if (baseCooldownValue != 0f) 
        {
            result += "Cooldown Bonus: \n-" + baseCooldownValue.ToString("0.00") + "% \n\n";
        }

        float rechargeRateValue = Mathf.Abs(weaponStats.RechargeRate.Value - weaponStats.RechargeRate.BaseValue);
        if (rechargeRateValue != 0f)
        {
            result += "Recharge Rate Bonus: \n+" + rechargeRateValue.ToString("0.00") + " \n\n";
        }

        float jumpHeightValue = Mathf.Abs(playerStats.JumpHeight.Value - playerStats.JumpHeight.BaseValue);
        if (jumpHeightValue != 0f) 
        {
            result += "Jump Height Bonus: \n+" + jumpHeightValue.ToString("0.00") + " ";
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
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierThree:
                newStatModOne = AddAPlayerModifier(tierThreeModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFour:
                newStatModOne = AddAPlayerModifier(tierFourModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFive:
                newStatModOne = AddAPlayerModifier(tierFiveModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierFiveModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSix:
                newStatModOne = AddAPlayerModifier(tierSixModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierSixModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSeven:
                newStatModOne = AddAPlayerModifier(tierSevenModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierSevenModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierEight:
                newStatModOne = AddAPlayerModifier(tierEightModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierEightModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierNine:
                newStatModOne = AddAPlayerModifier(tierNineModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierNineModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);

                playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
                break;

            case MinorCardTier.TierTen:
                newStatModOne = AddAPlayerModifier(tierTenModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierTenModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierTenModifier, 2);
                playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
                playerStats.JumpHeight.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierEleven:
                newStatModOne = AddAPlayerModifier(tierElevenModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierElevenModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierElevenModifier, 2);
                playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
                playerStats.JumpHeight.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierTwelve:
                newStatModOne = AddAPlayerModifier(tierTwelveModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierTwelveModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierTwelveModifier, 2);
                playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
                playerStats.JumpHeight.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierThirteen:
                newStatModOne = AddAPlayerModifier(tierThirteenModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierThirteenModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierThirteenModifier, 2);
                playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
                playerStats.JumpHeight.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierFourteen:
                newStatModOne = AddAPlayerModifier(tierFourteenModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierFourteenModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierFourteenModifier, 2);
                playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
                playerStats.JumpHeight.AddModifier(newStatModThree);
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
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierTwo:
                newStatModOne = AddAPlayerModifier(tierTwoModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierThree:
                newStatModOne = AddAPlayerModifier(tierThreeModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFour:
                newStatModOne = AddAPlayerModifier(tierFourModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);
                break;

            case MinorCardTier.TierFive:
                newStatModOne = AddAPlayerModifier(tierFiveModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierFiveModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSix:
                newStatModOne = AddAPlayerModifier(tierSixModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierSixModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierSeven:
                newStatModOne = AddAPlayerModifier(tierSevenModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierSevenModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierEight:
                newStatModOne = AddAPlayerModifier(tierEightModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierEightModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierNine:
                newStatModOne = AddAPlayerModifier(tierNineModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierNineModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);
                break;

            case MinorCardTier.TierTen:
                newStatModOne = AddAPlayerModifier(tierTenModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierTenModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierTenModifier, 2);
                playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
                playerStats.JumpHeight.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierEleven:
                newStatModOne = AddAPlayerModifier(tierElevenModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierElevenModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierElevenModifier, 2);
                playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
                playerStats.JumpHeight.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierTwelve:
                newStatModOne = AddAPlayerModifier(tierTwelveModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierTwelveModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierTwelveModifier, 2);
                playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
                playerStats.JumpHeight.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierThirteen:
                newStatModOne = AddAPlayerModifier(tierThirteenModifier, 0);
                playerStats.BaseCooldown.RemoveAllModifiersFromSource(this);
                playerStats.BaseCooldown.AddModifier(newStatModOne);

                newStatModTwo = AddAWeaponModifier(tierThirteenModifier, 1);
                weaponStats.RechargeRate.RemoveAllModifiersFromSource(this);
                weaponStats.RechargeRate.AddModifier(newStatModTwo);

                newStatModThree = AddAPlayerModifier(tierThirteenModifier, 2);
                playerStats.JumpHeight.RemoveAllModifiersFromSource(this);
                playerStats.JumpHeight.AddModifier(newStatModThree);
                break;

            case MinorCardTier.TierFourteen:
                break;
        }
        print(GetStats());
        return 1; // Success
    }
}
