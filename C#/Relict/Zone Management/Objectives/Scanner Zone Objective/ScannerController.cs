using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ITakeDamage;

public class ScannerController : MonoBehaviour, ITakeDamage, IInteractable
{
    [Header("Scanner")]
    public float health = 100f;
    public float interactDistance = 20f;

    [Header("Scanner Indicators")]
    public GameObject indicator;
    public TextMeshPro timerText;
    public TextMeshPro healthText;

    private ScannerZoneObjective objectiveController;
    private bool scannerReady = false;
    private bool scannerEnabled = false;
    private float timer = 1000f;

    public delegate void ScannerStarted(Transform target);
    public static ScannerStarted scannerStarted;
    public delegate void ScannerFinished();
    public static ScannerFinished scannerFinished;

    #region Events
    // Events
    public AboutToTakeDamage AboutToBeDamagedEvent;
    public AboutToTakeDamage AboutToBeDamaged { get => AboutToBeDamagedEvent; set => AboutToBeDamagedEvent = value; }

    public FinalDamage DamageToBeTaken;
    public FinalDamage BroadcastDamageToBeTaken { get => DamageToBeTaken; set => DamageToBeTaken = value; }

    private void OnEnable()
    {
        PlayerEvents.onInteract += Interact;
    }
    private void OnDisable()
    {
        PlayerEvents.onInteract -= Interact;
    }
    #endregion

    private void Update()
    {
        if (!scannerEnabled) return;

        timer -= Time.deltaTime;

        UpdateTimer();
    }

    public void SetAvailable(ScannerZoneObjective controller)
    {
        print(this + " Scanner ready for interaction!");
        objectiveController = controller;
        indicator.SetActive(true);

        scannerReady = true;
    }

    public void StartScanner()
    {
        print(this + " Scanner started!");

        timerText.gameObject.SetActive(true);
        timer = objectiveController.scannerTimer;

        healthText.gameObject.SetActive(true);
        healthText.text = "" + (int)health;

        objectiveController.StartScanner();

        scannerEnabled = true;

        scannerStarted?.Invoke(this.transform);
    }

    public void DisableScanner()
    {
        print(this + " Scanner disabled!");
        indicator.SetActive(false);
        timerText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        scannerEnabled = false;
        scannerFinished?.Invoke();
    }

    public void DamageScanner(float damage)
    {
        health -= damage;

        if (health < 0)
        {
            health = 0;
            ScannerDestroyed();
        }

        healthText.text = "" + (int)health;
    }

    public void ScannerDestroyed()
    {
        print("Scanner destroyed!");
        DisableScanner();
        objectiveController.FailedObjective();
    }

    private void UpdateTimer()
    {
        if (timer < 0f) timer = 0f;
        timerText.text = "" + (int)timer;
    }

    #region Damageable
    // Damages scanner
    public virtual void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, bool invokeEvent)
    {
        float blankCritVal = -1;
        float blankDamageMul = -1; // Blank values so that we can still invoke event

        if (invokeEvent) AboutToBeDamaged?.Invoke(ref damage, ref blankCritVal, ref blankDamageMul);

        print("Scanner Damage taken: " + damage);

        DamageNumberSpawner.SpawnDamageNumber(damageLocation, damage, damageNumberColor);
        DamageScanner(damage);
    }

    // Not scanner related
    public virtual void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float minClamp)
    {
        throw new System.NotImplementedException();
    }

    // Not scanner related
    public virtual void TakeDamage(Vector3 damageLocation, Color damageNumberColor, float damage, float baseCritChanceValue, float damageMultiplier)
    {
        float blankCritVal = -1;
        float blankDamageMul = -1; // Blank values so that we can still invoke event

        AboutToBeDamaged?.Invoke(ref damage, ref blankCritVal, ref blankDamageMul);

        print("Scanner Damage taken: " + damage);

        DamageNumberSpawner.SpawnDamageNumber(damageLocation, damage, damageNumberColor);
        DamageScanner(damage);
    }
    #endregion

    public void Interact()
    {
        if (!scannerReady || scannerEnabled) return; // Not ready for interaction

        Vector3 playerPos = GameManager.instance.player.transform.position;

        if (Vector3.Distance(playerPos, this.transform.position) < interactDistance)
        {
            StartScanner();
        }
    }
}
