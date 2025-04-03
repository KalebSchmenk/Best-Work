using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    [SerializeField] private float damageOutput = 45f;
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float moveUpValue = 2f;

    private float initY;
    private bool stopMoving = false;

    private void Start()
    {
        initY = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stopMoving) return;

        transform.position += transform.up * moveSpeed;
        print(transform.position.y - initY);
        if (transform.position.y - initY > moveUpValue) stopMoving = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (stopMoving) return;

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<ITakeDamage>(out ITakeDamage damageable))
            {
                damageable.TakeDamage(collision.GetContact(0).point, Color.white, damageOutput, true);
            }
        }
        stopMoving = true;
    }
}
