using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZoneObjective : ObjectiveBase
{ 
    public int enemiesLeft; // How many enemies are alive

    private List<LevelZone.Wave> waves = new List<LevelZone.Wave>();

    public List<Transform> spawnPositions = new List<Transform>(); // Where to spawn enemies

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

        StartCoroutine(SpawnWaveIn());

        return this;
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

                    i++;
                    break;
                }

                randomNum -= enemy.weight;
            }
        }

        enemiesLeft = wave.Enemies.Count;
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

                    enemiesLeft = aliveEnemies.Count;

                    break;
                }
            } catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
        }

        print("Enemies left " + enemiesLeft);

        if (enemiesLeft <= 0) 
        {
            waves.RemoveAt(0); // Wave complete, pop it

            print("Wave count " + waves.Count);

            if (waves.Count == 0)
            {
                FinishObjective(); // If no more waves, objective complete
            }
            else
            {
                StartCoroutine(SpawnWaveIn()); // Else, spawn next wave
            }
        }
    }

    IEnumerator SpawnWaveIn()
    {
        yield return new WaitForSeconds(2.5f);
        SpawnWave(waves[0]);
    }
}
