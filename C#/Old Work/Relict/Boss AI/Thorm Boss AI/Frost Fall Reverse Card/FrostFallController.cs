using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostFallController : MonoBehaviour
{
    [Header("Spawn Settings And Refs")]
    [SerializeField] private float startSpawningIn = 2.5f;
    [SerializeField] private float spawnShardEvery = 5f;
    [SerializeField] private float spawnRadius = 25f;
    [SerializeField] private GameObject iceShard;
    
    [Header("Collision Settings")]
    [SerializeField, Range(0f, 1f)] private float slowBy = 0.5f;
    [SerializeField] private float slowCooldown = 10f;
    [SerializeField] private float damageOutput = 15f;

    private bool inSlowCooldown = false;
    private PlayerHealth playerHealth;
    private PlayerStats playerStats;

    private void Start()
    {
        var player = GameManager.instance.player;
        playerStats = player.GetComponent<PlayerStats>();
        playerHealth = player.GetComponent<PlayerHealth>();

        InvokeRepeating("SpawnShard", startSpawningIn, spawnShardEvery);
    }

    private void SpawnShard()
    {
        Vector3 spawnPos = this.transform.position;

        spawnPos.x += UnityEngine.Random.Range(-spawnRadius, spawnRadius);
        spawnPos.z += UnityEngine.Random.Range(-spawnRadius, spawnRadius);
        spawnPos.y += 45f;

        var shard = Instantiate(iceShard, spawnPos, Quaternion.identity);

        int randomScale = UnityEngine.Random.Range(1, 4);
        shard.transform.localScale *= randomScale;

        var shardController = shard.GetComponent<IceShardController>();

        shardController.frostFallController = this;
    }

    public void ShardHitPlayer(Vector3 hitPoint)
    {
        if (inSlowCooldown) return;

        playerHealth.TakeDamage(hitPoint, Color.white, damageOutput, false);

        playerStats.MovementSpeed.AddModifier(new StatModifier(-slowBy, StatModifier.StatModType.PercentMult, this));

        StartCoroutine(SlowCooldown());
    }

    private IEnumerator SlowCooldown()
    {
        inSlowCooldown = true;
        yield return new WaitForSeconds(slowCooldown);
        playerStats.MovementSpeed.RemoveAllModifiersFromSource(this);
        inSlowCooldown = false;
    }
}
