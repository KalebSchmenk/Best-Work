using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 45f;

    private GenericGameObjectPool ownerPool;

    /// <summary>
    /// When bullet is fired. Tracks time in order to timeout and return to owner object pool.
    /// </summary>
    public void Fire()
    {
        StartCoroutine(TimeoutReturn());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += this.transform.forward * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IShootable>(out IShootable obj))
        {
            obj.Shot();
        }

        ownerPool.Return(this.gameObject);
        StopAllCoroutines();
    }

    public void SetObjectPool(GenericGameObjectPool pool)
    {
        this.ownerPool = pool;
    }

    private IEnumerator TimeoutReturn()
    {
        yield return new WaitForSeconds(6f);
        ownerPool.Return(this.gameObject);
    }
}
