using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class MajorCardBase : MonoBehaviour
{
    [Header("Default Major Card Refs and Settings")]
    public float coolDownTimer = 2.5f; // Default cooldown time
    private bool inCooldown = false; // Bool for whether or not card is in cooldown

    protected GameObject player; // Player ref
    protected PlayerStats playerStats; // Player stats ref
    protected WeaponStats weaponStats; // Weapon stats ref

    public GameObject vfx;

    public bool isActive = false; // Is this card currently active


    // Calls OnRemove() when object is destroyed
    private void OnDestroy()
    {
        OnRemove();
    }

    public virtual void AbilityKeyDown()
    {
        // Abstract ability call key down
    }
    public virtual void AbilityKeyUp()
    {
        // Abstract ability call key up
    }

    // When card is set to be currently active
    public virtual void OnAdd()
    {
        print(this + " set active");
        player = GameManager.instance.player;
        playerStats = player.GetComponent<PlayerStats>();
        weaponStats = player.GetComponent<WeaponStats>();

        isActive = true;
    }

    // When card is set to no longer be active or is removed from player
    public virtual void OnRemove()
    {
        print(this + " set inactive");

        isActive = false;
    }

    // Returns whether or not the ability is currently in a cooldown
    public bool GetCooldown() { return inCooldown; }

    // Starts cooldown wait period
    public Coroutine StartCooldown()
    {
        return StartCoroutine(StartCooldown(coolDownTimer * playerStats.BaseCooldown.Value));
    }

    // Starts cooldown wait period
    protected IEnumerator StartCooldown(float cooldown)
    {
        inCooldown = true;
        yield return new WaitForSeconds(cooldown);
        print(this + " Out of cooldown");
        inCooldown = false;
    }

    public virtual void SpawnVFX()
    {

    }
}
