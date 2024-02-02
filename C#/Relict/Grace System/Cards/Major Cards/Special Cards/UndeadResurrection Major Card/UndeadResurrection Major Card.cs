using System.Collections;
using UnityEngine;

public class UndeadResurrectionMajorCard : MajorCardBase
{
    [Header("Undead Resurrection References")]
    public float maxTargetDistance = 6f; // max resurrection distance
    public LayerMask enemyMask;
    public float resurrectionTime = 5f;

    private GameObject enemy;

    Coroutine resurrectionIndicatorCoroutine; // reference for the raycast coroutine

    GameObject cam; // camera reference
    AIMain aiMain;
    
    // on ability key down create the ressurect indicator raycast line.
    public override void AbilityKeyDown()
    {
        if (GetCooldown()) return; // check if we're still coolingdown

        if(resurrectionIndicatorCoroutine == null)
        {
            resurrectionIndicatorCoroutine = StartCoroutine(ResurrectIndicator());
        }

        print("Undead Resurrection Key Down");
        ResurrectIndicator();
    }

    public override void AbilityKeyUp()
    {
        if (GetCooldown() || resurrectionIndicatorCoroutine == null) return; // if cooling down guard claause
        print("Undead Resurrection Key Up");

        StopCoroutine(resurrectionIndicatorCoroutine);
        resurrectionIndicatorCoroutine = null;

        if (aiMain != null)
        {
            if (aiMain.health <= 0)
            {
                Debug.Log("Dead Enemey");
                Resurrect(); // call the resurrection code
                StartCooldown();
                enemy = aiMain.gameObject;
                SpawnVFX();
            }
        }
        else return;
    }

    // Grabs references when added to player inventory
    public override void OnAdd()
    {
        base.OnAdd();

        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Prints to console that this card was removed
    public override void OnRemove()
    {
        base.OnRemove();

        // Code to kill the enemies again.
    }

    // raycast out to see if the target is dead and is an enemy able to be resurrected
    IEnumerator ResurrectIndicator()
    {
        while (true)
        {
            var rayHit = RayCast(cam.transform.position, cam.transform.forward, maxTargetDistance);

            if (rayHit.collider != null && rayHit.collider.gameObject.TryGetComponent<AIMain>(out AIMain main)) // if the ray hits something and it has AIMain indicating an enemy ai.
            {
                aiMain = main;
                // nest an if statement that checks if main.isDead before calling resurrect()
                print("Enemy Component found");
            
            }

            yield return null;
        }

    }

    // Enemy resurrection code
    void Resurrect()
    {
        aiMain.isResurrected = true;

        print("Resurrect function called");
        // resurrect dead enemy. Start timer that when it runs out kills them again.

        AudioManager.instance.PlaySfx("UndeadResurrection"); // Plays undead resurrection sound
    }


    private RaycastHit RayCast(Vector3 from, Vector3 dir, float len)
    {
        // container for hit data
        RaycastHit hit;

        Debug.DrawRay(transform.position, cam.transform.forward, Color.green);

        if (Physics.Raycast(from, dir, out hit, len))
        {
            return hit;
        }

        return new RaycastHit();
    }

    public override void SpawnVFX()
    {
        Instantiate(vfx, enemy.transform.position, Quaternion.identity, enemy.transform);
    }
}
