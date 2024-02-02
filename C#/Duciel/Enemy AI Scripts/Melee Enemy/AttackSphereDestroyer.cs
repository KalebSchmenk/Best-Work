using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSphereDestroyer : MonoBehaviour
{
    [SerializeField] private float _destroyIn = 0.1f;

    private void Awake()
    {
        StartCoroutine(DestroyIn());
    }

    private IEnumerator DestroyIn()
    {
        yield return new WaitForSeconds(_destroyIn);

        Destroy(this.gameObject);
    }
}
