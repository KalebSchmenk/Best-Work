using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KorosMeteorShowerController : MonoBehaviour
{
    [SerializeField] private GameObject meteor;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 10f;

    [SerializeField] private bool spawnOnStart = false;
    [SerializeField] private float startSpawningIn = 3f;
    [SerializeField] private float spawnMeteorEvery = 2f;
    public float spawnRadius = 25f;

    private bool stopSpawning = false;

    private void Start()
    {
        if (spawnOnStart)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        InvokeRepeating("SpawnMeteor", startSpawningIn, spawnMeteorEvery); // Invokes function every 2 seconds
    }
    public void StopSpawning()
    {
        stopSpawning = true;
    }

    public void GameWon()
    {
        PlayerEvents.onWin?.Invoke();
    }

    // Spawns Meteor
    private void SpawnMeteor()
    {
        if (stopSpawning) return;

        Vector3 spawnPos = this.transform.position;

        spawnPos.x += UnityEngine.Random.Range(-spawnRadius, spawnRadius);
        spawnPos.z += UnityEngine.Random.Range(-spawnRadius, spawnRadius);
        spawnPos.y += 45f;

        var obj = Instantiate(meteor, spawnPos, Quaternion.identity);

        int randomScale = UnityEngine.Random.Range(1, 4);
        obj.transform.localScale *= randomScale;

        var controller = obj.GetComponent<KorosMeteorController>();

        controller.speed = speed;
        controller.damageOutput = damage;
    }
}
