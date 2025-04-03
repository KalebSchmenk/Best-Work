using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollisionEvent : MonoBehaviour
{
    public UnityEvent OnCollision;

    public float delayInvoke = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                CollisionDetected();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                CollisionDetected();
            }
        }
    }

    private void CollisionDetected()
    {
        if (delayInvoke > 0f)
        {
            StartCoroutine(CallEventIn());
        }
        else
        {
            OnCollision?.Invoke();
        }
    }

    private IEnumerator CallEventIn()
    {
        yield return new WaitForSeconds(delayInvoke);
        OnCollision?.Invoke();
    }
}
