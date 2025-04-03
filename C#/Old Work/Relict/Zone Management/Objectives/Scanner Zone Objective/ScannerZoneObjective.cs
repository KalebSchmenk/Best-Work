using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerZoneObjective : ObjectiveBase
{
    [Header("Wave Settings")]
    public float spawnRandomWaveEvery = 25f; // When to spawn a new random wave
    public List<Transform> spawnPositions = new List<Transform>(); // Where to spawn enemies

    [Header("Scanner Settings")]
    public float scannerTimer = 10f; // How long scan lasts
    public ScannerController scannerController; // Reference to scanner in scene

    private List<LevelZone.Wave> waves = new List<LevelZone.Wave>(); // List of waves
    List<GameObject> aliveEnemies = new List<GameObject>(); // Enemies that this zone has spawned and that are alive

    #region Event Subscriptions
    private void OnEnable()
    {
        EnemyEvents.OnEnemyDied += OnEnemyKilled;
    }

    private void OnDisable()
    {
        EnemyEvents.OnEnemyDied -= OnEnemyKilled;
    }
    #endregion

    protected override void Start()
    {
        base.Start();

        waves = LevelManager.instance.GetEnemyWave();
    }

    public override ObjectiveBase StartObjective()
    {
        print("Objective " + this + " was started!");
        isActive = true;

        scannerController.SetAvailable(this);
        return this;
    }

    public override void FinishObjective()
    {
        base.FinishObjective();

        scannerController.DisableScanner();
    }

    public override ObjectiveBase FailedObjective()
    {
        print(this + " objective failed!");
        GameManager.instance.UpdateObjective("Scanner destroyed!");
        GameManager.instance.player.GetComponent<PlayerHealth>().SetPlayerHealth(0);
        return this;
    }

    public void StartScanner()
    {
        StartCoroutine(SpawnRandomWave());
        StartCoroutine(ObjectiveTimer());
    }

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
                    if (spawnedEnemy.TryGetComponent<AIMain>(out aiMain))
                    {
                        if (aiMain.targetOverride != spawnedEnemy.transform && !aiMain.targetOverrides.Contains(spawnedEnemy.transform))
                        {
                            aiMain.OverrideTarget(scannerController.transform);
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
