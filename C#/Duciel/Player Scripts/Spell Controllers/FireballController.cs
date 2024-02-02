using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField] private GameObject _fireballExplosion;

    [SerializeField] float _fireballSpeed = 750f;
    
    [SerializeField] Rigidbody rb;

    [SerializeField] private GameObject _fireballImpactSound;
    
    private Vector3 impactPosition;

    [Header("Player Sounds")]
    [SerializeField] AudioSource playerFireBallImpactObject;
    [SerializeField] AudioClip  playerFireBallImpactClip;

    public void Fire(Vector3 lookAt)
    {
        this.transform.LookAt(lookAt);

        rb.AddForce(transform.forward * _fireballSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_fireballExplosion, this.transform.position, Quaternion.identity);

        impactPosition = transform.position;
        Instantiate(_fireballImpactSound, impactPosition, Quaternion.identity);
        playerFireBallImpactObject.clip = playerFireBallImpactClip;
        playerFireBallImpactObject.Play();

        Destroy(this.gameObject);
    }

}
