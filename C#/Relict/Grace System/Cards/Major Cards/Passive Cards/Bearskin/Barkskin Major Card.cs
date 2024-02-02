using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkskinMajorCard : MajorCardBase
{
    [Header("Card References")]
    [SerializeField] private float damageReduction;
    PlayerHealth playerHealth; 

    public override void OnAdd()
    {
        base.OnAdd();
        player.GetComponent<PlayerHealth>().DamageToBeTaken += Barkskin;
        
    }

    public override void OnRemove()
    {
        base.OnRemove();
        player.GetComponent<PlayerHealth>().DamageToBeTaken -= Barkskin;
       
    }

    private void Barkskin(ref float damage)
    {
        damage *= (1 - damageReduction);
        print(damage);
    }
}

// what needs to happen? The player should be able to set a number in the inspector. When the player takes damage (aka damagetobetaken is called) it subscribes to the barkskin function
// this function takes that number, and reduces it by the percentage entered in the inspector. 