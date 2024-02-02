using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LeftGateTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject leftGate;
   public int count = 0;

    [SerializeField] private Animator openLeftGateAnimationController;

    void OnTriggerStay(Collider other)
    {
      if (other.CompareTag("Boulder"))
        {
            count = count + 1;
        }
    
    if (count == 3)
             openLeftGateAnimationController.SetBool("OpenLeft", true);

        }

    void OnTriggerExit(Collider other)
    {
      if (other.CompareTag("Boulder"))
        {
            count = count - 1;
       
        }
    if (count >= 2)

            openLeftGateAnimationController.SetBool("OpenLeft", false);
        }
}
    