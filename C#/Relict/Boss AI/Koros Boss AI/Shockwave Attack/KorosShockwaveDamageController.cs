using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KorosShockwaveDamageController : MonoBehaviour
{
    public KorosShockwaveController manager; // Manager ref
    public float speed = 20f; // Move speed

    private void Start()
    {
        Destroy(this.gameObject, 150f);
    }

    private void Update()
    {
        this.gameObject.transform.Translate(this.transform.forward * Time.deltaTime * speed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // If trigger hit a player
        {
            var damageable = other.gameObject.GetComponent<ITakeDamage>();

            if (damageable == null)
            {
                Debug.LogError("Player missing ITakeDamage interface...");
                return;
            }

            manager.HitPlayer(damageable, other.ClosestPoint(transform.position));
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
