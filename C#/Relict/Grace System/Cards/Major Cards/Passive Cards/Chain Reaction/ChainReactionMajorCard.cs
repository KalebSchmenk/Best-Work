using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainReactionMajorCard : MajorCardBase
{
    [Header("Chain Reaction Refs and Settings")]
    [SerializeField] private float damage = 15f; // Extra damage when enemy hits terrain
    private List<AIMain> knockbackedEnemies = new List<AIMain>(); // List of knockbacked enemies



    // Deal extra damage to knockbacked enemies and deal damage to enemy it hits
    public void AddDamage(GameObject enemy, GameObject collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Enemy") || collision.CompareTag("Environment"))
        {
            print("Collision");
            AIMain ai = enemy.GetComponent<AIMain>();

            if (ai.isStunned)
            {
                print("Chain Reaction hurting enemy " + damage);

                enemy.GetComponent<ITakeDamage>().TakeDamage(enemy.transform.position, Color.white, damage, false);

                if (collision.CompareTag("Enemy"))
                {
                    if (!collision.GetComponent<AIMain>().aiDead.isDead) // If the other enemy is not dead, do damage
                    {
                        print("Enemy knocked into another! Dealing damage to that enemy");

                        collision.GetComponent<ITakeDamage>().TakeDamage(collision.transform.position, Color.white, damage, false);
                    }
                }
            }
        }
         
        ClearNonStunnedEnemies();
    }

    // Add knockbacked enemy to our list and subscribe to their collision event
    public void AddKnockbackedEnemy(AIMain enemyController)
    {
        knockbackedEnemies.Add(enemyController);

        enemyController.OnCollision += AddDamage;
    }

    // Listen for knockback hits here
    private void ListenForKnockback(GameObject enemy)
    {
        AIMain enemyAI = enemy.GetComponent<AIMain>();

        if (enemyAI.isStunned) AddKnockbackedEnemy(enemyAI);
    }

    // Clears non stunned enemies from list
    private void ClearNonStunnedEnemies()
    {
        List<AIMain> listToRemove = new List<AIMain>();
        foreach(var item in knockbackedEnemies)
        {
            if (!item.isStunned) // Mark for removal if enemy not stunned
            {
                listToRemove.Add(item);
            }
        }
        foreach (AIMain enemy in listToRemove)
        {
            enemy.OnCollision -= AddDamage;
        }
        knockbackedEnemies.RemoveAll(x => listToRemove.Contains(x)); // Removes everything from knockbackedEnemies that were marked in listToRemove
    }

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        EnemyEvents.OnEnemyKnockbacked += ListenForKnockback;
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();

        EnemyEvents.OnEnemyKnockbacked -= ListenForKnockback;

        foreach (var interactable in knockbackedEnemies)
        {
            if (interactable != null)
            {
                interactable.OnCollision -= AddDamage;
            }
        }
    }
}
