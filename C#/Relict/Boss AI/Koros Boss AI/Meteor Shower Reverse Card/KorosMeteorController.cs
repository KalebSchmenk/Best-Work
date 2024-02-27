using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KorosMeteorController : MonoBehaviour
{
    public float damageOutput = 10f;
    public float speed = 10f;
    
    
    void Update()
    {
        this.gameObject.transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PP"))
        {
            Debug.Log("Collided with PP!");
            Physics.IgnoreCollision(other, GetComponent<Collider>());
            return;
        }

        if (other.gameObject.CompareTag("Player")) // If trigger hit a player
        {
            var damageable = other.gameObject.GetComponent<ITakeDamage>();

            if (damageable == null)
            {
                Debug.LogError("Player missing ITakeDamage interface...");
                return;
            }
            
            damageable.TakeDamage(other.ClosestPoint(transform.position), Color.white, damageOutput, true);
        }

        AudioManager.instance.PlaySfx("MeteorImpact"); // Plays meteor impact sound
        Destroy(this.gameObject);
    }
}
