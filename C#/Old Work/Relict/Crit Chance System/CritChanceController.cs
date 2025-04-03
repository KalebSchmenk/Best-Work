using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritChanceController : MonoBehaviour
{
    public static CritChanceController instance;

    private void Awake()
    {
        HandleSingleton();
    }

    private void HandleSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Crit chance system
    public float TryCritHit(float damage, float baseCritChanceValue, float damageMultiplier)
    {
        int randomNum = UnityEngine.Random.Range(0, 100);

        if (randomNum < baseCritChanceValue) // Crit!
        {
            damage = damage * (1f + (damageMultiplier / 100f));
            print("Crit! Damage output is: " + damage);
            return damage;
        }
        else
        {
            //print("Didn't Crit");
            return -1;
        }
    }
}
