using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// DESIGN PATTERN: Mediator and Observer
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [Header("Factory Products")]
    public GameObject easyEnemy;
    public GameObject hardEnemy;

    [Header("Player Check Distance")]
    [SerializeField] private float distanceCheck = 20f;

    [Header("Enemy Spawn Points")]
    [SerializeField] private List<Transform> enemySpawnPoints;

    [Header("Enemy To Spawn When Player In Range")]
    [SerializeField] private string enemyType;

    private EnemyFactory enemyFactory;

    private void Start()
    {
        enemyFactory = this.AddComponent<EnemyFactory>();

        enemyFactory.RegisterProduct("EasyEnemy", easyEnemy);
        enemyFactory.RegisterProduct("HardEnemy", hardEnemy);

        StartCoroutine(PlayerInRangeCheck());
    }

    /// <summary>
    /// Spawns an enemy via our enemy factory. Listens for its death to recycle.
    /// </summary>
    /// <param name="enemy">Enemy type to spawn.</param>
    /// <returns>Spawned enemy.</returns>
    private GameObject SpawnEnemy(string enemy)
    {
        var spawnedEnemy = enemyFactory.CreateEnemy(enemy);

        BaseEnemyController baseEnemyController = spawnedEnemy.GetComponent<BaseEnemyController>();
        baseEnemyController.OnEnemyDied += EnemyDied;

        return spawnedEnemy;
    }

    /// <summary>
    /// Callback function that listens for any enemies death to recycle it. Returns it to factory.
    /// </summary>
    /// <param name="enemy">Enemy to recycle.</param>
    private void EnemyDied(GameObject enemy)
    {
        BaseEnemyController baseEnemyController = enemy.GetComponent<BaseEnemyController>();
        baseEnemyController.OnEnemyDied -= EnemyDied;

        enemyFactory.ReturnEnemy(enemy);
    }

    /// <summary>
    /// If player gets in range of enemy manager, spawn enemies at spawn positions.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerInRangeCheck()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        while (true)
        {
            yield return new WaitForSeconds(.75f);
            float distance = Vector3.Distance(this.transform.position, player.transform.position);

            if (distance < distanceCheck)
            {
                foreach (Transform t in enemySpawnPoints)
                {
                    var enemy = SpawnEnemy(enemyType);
                    enemy.GetComponent<NavMeshAgent>().Warp(t.position);
                }
                yield break;
            }
        }
    }
}
