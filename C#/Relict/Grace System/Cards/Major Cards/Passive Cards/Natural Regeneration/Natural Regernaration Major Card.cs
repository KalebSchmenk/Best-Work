using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NaturalRegernarationMajorCard : MajorCardBase
{
     [Header("Natural regeneration")] // Regeneration variables
    public float waitToRegenerate;
    private float currentTime;
    private bool regenerationActive = true;
    PlayerController playerController;
    PlayerHealth playerHealth;
    

    public override void OnAdd()
    {
        base.OnAdd();

        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        //StartCoroutine(NaturalRegeneration());
        currentTime = 5;
        regenerationActive = false;
    }

    public override void OnRemove()
    {
        base.OnRemove();

        StopAllCoroutines();
    }

    private void Update()
    {
        if (!isActive) return; // Guard clause

        if (playerController.GetGroundedPlayer() == true && InputManager.instance.MovementInput.x == 0 && InputManager.instance.MovementInput.y == 0)
        {
            if (regenerationActive)
            {
                if (playerHealth.GetPlayerHealth() <= playerHealth.GetPlayerMaxHealth())
                    playerHealth.ChangePlayerHealth(10f * Time.deltaTime);
            }
            else if (currentTime > 0)
            {
                regenerationActive = false;
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                    currentTime = 0;
                    regenerationActive = true;
                }
            }
        }
        else
        {
            regenerationActive = false;
            currentTime = waitToRegenerate;
        }
    }

    /*IEnumerator NaturalRegeneration()
    {
        while (regenerationActive == true)
        {
            if (playerController.GetGroundedPlayer() == true && InputManager.instance.MovementInput.x == 0 && InputManager.instance.MovementInput.y == 0)
            {
                // start or reset cooldown
                currentTime = Mathf.Max(0, currentTime - Time.deltaTime);
                Debug.Log("Current time:" + currentTime);
            }
            else currentTime = waitToRegenerate;

            if (currentTime <= 0 && regenerationActive)
            {
                playerHealth.ChangePlayerHealth(10);
                Debug.Log("Natural regeneration active");
                regenerationActive = false; // stop regeneration until cooldown is reset

                // wait for once second until allowing regeneration again
                yield return new WaitForSeconds(1f);
                regenerationActive = true;
            }
            yield return null; // note to self remember to yield to next frame otherwise coroutine doesn't work
        }
    }*/ 
}

