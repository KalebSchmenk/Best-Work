using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThormLightningController : MonoBehaviour
{
    public GameObject lightningObj;
    public GameObject reticleObj;
    public float spawnIn = 5f;
    public float destroyAfterSpawnIn = 1.5f;
    public float damageOutput = 15f;

    bool hitSomething = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLightningIn());
    }

    private IEnumerator SpawnLightningIn()
    {
        yield return new WaitForSeconds(spawnIn);
        reticleObj.SetActive(false);
        lightningObj.SetActive(true);
        GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(destroyAfterSpawnIn);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitSomething) return;

        if (other.TryGetComponent<ITakeDamage>(out ITakeDamage damageable))
        {
            damageable.TakeDamage(other.transform.position, Color.white, damageOutput, true);
            hitSomething = true;
        }
    }
}
