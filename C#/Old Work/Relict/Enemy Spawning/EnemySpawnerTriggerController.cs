using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerTriggerController : MonoBehaviour
{
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public GameObject vfxToSpawnUnderEnemy;

    private void Start()
    {
        // Ensures all enemies are turned off on start
        foreach (GameObject enemy in enemiesToSpawn)
        {
            enemy.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach(GameObject enemy in enemiesToSpawn)
            {
                enemy.SetActive(true);
                Instantiate(vfxToSpawnUnderEnemy, enemy.transform.position, Quaternion.identity);
            }

            Destroy(this.gameObject);
        }
    }
}
