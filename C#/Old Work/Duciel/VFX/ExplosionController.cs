using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private int _damageOutput = 3;
    [SerializeField] GameObject _explosionEffect;
    private GameObject objRef;

    void Start()
    {
        objRef = Instantiate(_explosionEffect, transform.position, Quaternion.identity);

        StartCoroutine(DestroyIn());
    }

    private IEnumerator DestroyIn()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(objRef);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthInterface script = other.transform.gameObject.GetComponent<EnemyHealthInterface>();

            if (script != null) script.TakeDamage(_damageOutput);
        }
    }
}
