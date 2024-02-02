using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInController : MonoBehaviour
{
    public float destroyIn;

    private void Start()
    {
        StartCoroutine(DestroyIn(destroyIn));
    }

    private IEnumerator DestroyIn(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);
    }
}
