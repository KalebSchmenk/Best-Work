using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static ITakeDamage;

public class MiniScannerController : MonoBehaviour, ITakeDamage
{
    [Header("Scanner")]
    public float health = 100f;
    public float interactDistance = 20f;

    [Header("Scanner Indicators")]
    public GameObject indicator;
    public TextMeshPro timerText;
    public TextMeshPro healthText;

    [Header("Scanner Settings")]
    public float scannerTimer = 10f; // How long scan lasts

    [Header("Wave Settings")]
    public float spawnRandomWaveEvery = 25f; // When to spawn a new random wave
    public List<Transform> spawnPositions = new List<Transform>(); // Where to spawn enemies
   

    private List<LevelZone.Wave> waves = new List<LevelZone.Wave>(); // List of waves
    List<GameObject> aliveEnemies = new List<GameObject>(); // Enemies that this zone has spawned and that are alive
    [NonSerialized] public bool isActive = false;

    
    private MiniScannerZoneObjective objectiveController;
    private bool scannerReady = false;
    private bool scannerEnabled = false;
    private bool scannerComplete = false;
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
        EnemyEvents.OnEnemyDied += OnEnemyKilled;
    }
    private void OnDisable()
    {
        PlayerEvents.onInteract -= Interact;
        EnemyEvents.OnEnemyDied -= OnEnemyKilled;
    }
    #endregion

    protected void Start()
    {
        waves = LevelManager.instance.GetEnemyWave();
    }

    private void Update()
    {
        if (!scannerEnabled) return;

        timer -= Time.deltaTime;

        UpdateTimer();
    }

    public void SetAvailable(MiniScannerZoneObjective controller)
    {
        if (scannerComplete) return;

        print(this + " Scanner ready for interaction!");
        objectiveController = controller;
        indicator.SetActive(true);

        scannerReady = true;
    }

    public void SetUnavailable()
    {
        print(this + " Scanner no longer ready for interaction!");
        indicator.SetActive(false);

        scannerReady = false;
    }

    // Objective related funcs
    //
    public void FailedObjective()
    {
        print(this + " objective failed!");
        GameManager.instance.UpdateObjective("Scanner destroyed!");
        GameManager.instance.player.GetComponent<PlayerHealth>().SetPlayerHealth(0);
        isActive = false;
    }

    public void FinishObjective()
    {
        objectiveController.ScannerFinished(this);
        isActive = false;
        scannerComplete = true;
        DisableScanner();
    }

    // Scanner related funcs
    //
    public void StartScanner()
    {
        print(this + " Scanner started!");

        timerText.gameObject.SetActive(true);
        timer = scannerTimer;

        healthText.gameObject.SetActive(true);
        healthText.text = "" + (int)health;

        scannerEnabled = true;

        objectiveController.ScannerStarted(this);

        StartCoroutine(SpawnRandomWave());
        StartCoroutine(ObjectiveTimer());

        ScannerController.scannerStarted?.Invoke(this.transform);
    }

    public void DisableScanner()
    {
        print(this + " Scanner disabled!");
        indicator.SetActive(false);
        timerText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        scannerEnabled = false;
        ScannerController.scannerFinished?.Invoke();
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
        FailedObjective();
    }

    // Updates timer
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

    // On player interact with this scanner
    public void Interact()
    {
        if (!scannerReady || scannerEnabled) return; // Not ready for interaction

        Vector3 playerPos = GameManager.instance.player.transform.position;

        if (Vector3.Distance(playerPos, this.transform.position) < interactDistance)
        {
            StartScanner();
        }
    }
  
    // Enemy/Wave related funcs
    //
    // Spawns enemy wave
    private void SpawnWave(LevelZone.Wave wave)
    {
        print("Spawning " + wave.name);

        int i = 0;

        while (i < wave.enemySpawnCount)
        {
            int maxWeight = 0;
            foreach (var enemy in wave.Enemies)
            {
                maxWeight += enemy.weight;
            }
            int randomNum = UnityEngine.Random.Range(0, maxWeight + 1);

            foreach (var enemy in wave.Enemies)
            {
                if (randomNum < enemy.weight)
                {
                    int randomSpawnIndex = UnityEngine.Random.Range(0, spawnPositions.Count);
                    GameObject spawnedEnemy = Instantiate(enemy.enemyObj, spawnPositions[randomSpawnIndex].position, Quaternion.identity);
                    spawnedEnemy.name += " " + i;
                    aliveEnemies.Add(spawnedEnemy);
                    AIMain aiMain;
                    if(spawnedEnemy.TryGetComponent<AIMain>(out aiMain))
                    {
                        if(aiMain.targetOverride != spawnedEnemy.transform && !aiMain.targetOverrides.Contains(spawnedEnemy.transform))
                        {
                            aiMain.OverrideTarget(this.transform);
                        }
                    }

                    i++;
                    break;
                }

                randomNum -= enemy.weight;
            }
        }
    }

    // When an enemy in the scene is killed
    private void OnEnemyKilled(GameObject enemy)
    {
        foreach (var enemyInList in aliveEnemies)
        {
            try
            {
                if (GameObject.ReferenceEquals(enemyInList, enemy)) // Is this an enemy we spawned and need to track?
                {
                    aliveEnemies.Remove(enemyInList);

                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    // Spawns random wave every set seconds
    IEnumerator SpawnRandomWave()
    {
        while (true)
        {
            int randomNum = UnityEngine.Random.Range(0, waves.Count);
            SpawnWave(waves[randomNum]); // Spawn random wave
            yield return new WaitForSeconds(spawnRandomWaveEvery);
            if (!isActive) break;
        }
    }

    // Objective timer
    IEnumerator ObjectiveTimer()
    {
        while (scannerTimer >= 0)
        {
            scannerTimer -= Time.deltaTime;
            yield return null;
        }

        FinishObjective();
    }
}
