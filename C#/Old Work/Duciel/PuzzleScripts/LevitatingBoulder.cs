using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitatingBoulder : MonoBehaviour
{
    [SerializeField]
    GameObject LevBoulder;

    [SerializeField] private Animator doorAnimationController;

    void OnTriggerStay(Collider other)
    {
      if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.L))
                {
                doorAnimationController.SetBool("LevitatingBoulder", true);
                 }
            }
    }

    void OnTriggerExit(Collider other)
    {
         if (other.CompareTag("Player"))
            {
                doorAnimationController.SetBool("LevitatingBoulder2", true);
            }
        }
    }

