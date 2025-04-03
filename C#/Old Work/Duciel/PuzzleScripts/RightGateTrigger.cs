using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RightGateTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject rightGate;
   public int count = 0;

    [SerializeField] private Animator openRightGateAnimationController;

    void OnTriggerStay(Collider other)
    {
      if (other.CompareTag("Boulder"))
        {
            count = count + 1;
        }
    
    if (count == 3)
             openRightGateAnimationController.SetBool("OpenRight", true);

        }

    void OnTriggerExit(Collider other)
    {
      if (other.CompareTag("Boulder"))
        {
            count = count - 1;
       
        }
    if (count >= 2)

            openRightGateAnimationController.SetBool("OpenRight", false);
        }
}
    