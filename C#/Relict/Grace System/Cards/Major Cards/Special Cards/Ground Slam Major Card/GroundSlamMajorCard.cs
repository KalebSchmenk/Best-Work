using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlamMajorCard : MajorCardBase
{
    public AudioClip slamSound;
    public float upForce = 15f;
    public float downforce = 35f;
    public float moveForce = 2.5f;
    public float waitInAirFor = 1.25f;
    public float collisionSphereRadius = 12.5f;
    public float sphereDamageOutput = 25f;
    public float knockbackForce = 2.5f;
    public StatusEffectData dazeEffect;

    private bool canSpawnDamageSphere = false;
    private PlayerController playerController;

    // On ability key down 
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // Guard clause. If we are cooling down - return

        print("Ground slam key down");
    }

    // On ability key up 
    public override void AbilityKeyUp()
    {
        if (GetCooldown() || !playerController.GetGroundedPlayer()) return; // Guard clause. If we are cooling down or player is not on the ground - return.

        print(this + " called its ability");

        Slam();

        PlayerEvents.OnAbilityUsed?.Invoke(this);

        StartCooldown(); // Ability cooldown
    }

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        playerController = GameManager.instance.player.GetComponent<PlayerController>();
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();
    }

    // Ground Slam
    private void Slam()
    {
        StartCoroutine(GroundSlam());

        // Plays dash sound and disables footstep sounds momentarily
        playerController.PlaySound(playerController.playerData.Player_Dash);
        playerController.footstepsSound.enabled = false;
        playerController.sprintSound.enabled = false;
    }

    // Ground slam coroutine for applying physics to player
    private IEnumerator GroundSlam()
    {
        playerController.canMove = false;
        canSpawnDamageSphere = true;
        playerController.playerVelocity = new Vector3(player.transform.forward.x * moveForce, player.transform.up.y * upForce, player.transform.forward.z * moveForce);

        yield return new WaitForSeconds(waitInAirFor);

        StartCoroutine(IsPlayerOnGround());
        playerController.playerVelocity = new Vector3(0f, player.transform.up.y * -downforce, 0f);
    }

    // If player is on ground, slam if we can
    private IEnumerator IsPlayerOnGround()
    {
        while (true)
        {
            if (playerController.GetGroundedPlayer()) break;
            yield return null;
        }

        if (!canSpawnDamageSphere) yield break; // Guard clause
        playerController.canMove = true;
        SpawnDamageSphere();
    }

    // Spawns overlap sphere to find enemies to effect
    private void SpawnDamageSphere()
    {
        canSpawnDamageSphere = false;

        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, collisionSphereRadius, LayerMask.GetMask("Enemy"));

        if (slamSound != null)
        {
            playerController.PlaySound(slamSound);
        }

        foreach (Collider collider in hitColliders)
        {
            if (!collider.gameObject.CompareTag("Enemy")) continue; // Guard clause if not enemy

            GameObject enemy = collider.gameObject;

            // Knockback
            var knockbackDir = enemy.transform.position - player.transform.position;
            knockbackDir.y = 0;
            knockbackDir.Normalize();

            float forceMultiplier = 5f;
            if (enemy.GetComponent<CrabMain>() != null)
                forceMultiplier = 200f;
            else if (enemy.GetComponent<EelMain>() != null)
                forceMultiplier = 5f;
            else if (enemy.GetComponent<AIMain>() != null)
                forceMultiplier = 5f;

            enemy.GetComponent<Rigidbody>().AddForce(knockbackDir * (knockbackForce * forceMultiplier), ForceMode.Impulse);

            // Deal Damage
            if (enemy.TryGetComponent<ITakeDamage>(out ITakeDamage damageable))
            {
                damageable.TakeDamage(enemy.transform.position, Color.white, sphereDamageOutput, true);
            }

            // Try for status effect
            if (enemy.TryGetComponent<IEffectable>(out IEffectable effectable))
            {
                effectable.AddStatusEffect(dazeEffect);
            }     
        }
    }
}
