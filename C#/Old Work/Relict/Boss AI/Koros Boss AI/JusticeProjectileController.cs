using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JusticeProjectileController : MonoBehaviour
{
    private enum ProjectileState
    {
        Launch,
        Chase,
        Destroyed
    } private ProjectileState projState;

    public float damageOnHit = 25f;
    public float launchSpeed = 5.0f;
    public float moveSpeed = 10f;
    public float rotSpeed = 45f;
    Rigidbody rb;
    Transform playerTrans;

    // Audio code
    AudioSource audioSource;
    public AudioSource projectileFlightSound;
    public AudioClip projectileImpactSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTrans = GameManager.instance.player.transform;
        StartCoroutine(StartChaseIn());

        audioSource = GetComponent<AudioSource>(); // Audio source code
        projectileFlightSound.enabled = true; // Plays projectile flight sound
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(projState)
        {
            case ProjectileState.Launch:
                Launch();
                break;

            case ProjectileState.Chase:
                Chase();
                break;

            case ProjectileState.Destroyed:
                Destroyed();
                break;
        }
    }

    public void Launch()
    {
        rb.velocity = transform.forward * launchSpeed;
    }

    public void Chase()
    {
        var playerPosUp = playerTrans.position;
        playerPosUp.y += 1.5f;

        var lookAt = playerPosUp - this.transform.position;
        var rotQuat = Quaternion.LookRotation(lookAt);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotQuat, rotSpeed * Time.deltaTime);

        rb.velocity = transform.forward * moveSpeed;
    }

    public void Destroyed()
    {
        // temp until way to break apart is implemented
        rb.useGravity = true;
        Destroy(this.gameObject, 3f);
    }

    IEnumerator StartChaseIn()
    {
        yield return new WaitForSeconds(2.5f);
        if (projState != ProjectileState.Destroyed) projState = ProjectileState.Chase;
        this.gameObject.transform.parent = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (projState == ProjectileState.Destroyed) return;

        if (collision.gameObject.TryGetComponent<ITakeDamage>(out ITakeDamage damageable))
        {
            damageable.TakeDamage(collision.contacts[0].point, Color.white, damageOnHit, playerTrans);
        }

        projState = ProjectileState.Destroyed;

        PlaySound(projectileImpactSound); // Plays projectile impact sound upon contact
        projectileFlightSound.enabled = false; // Disables flight sound after contact
    }

    public void PlaySound(AudioClip clip) // Allows the projectile to play sounds
    {
        audioSource.PlayOneShot(clip);
    }
}
