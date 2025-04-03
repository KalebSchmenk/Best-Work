using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : MonoBehaviour
{
    Animator lev_Anim;
    // Start is called before the first frame update
    void Start()
    {
       lev_Anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        lev_Anim.SetBool("LevUp", true);
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
         lev_Anim.SetBool("LevDown", true);
        other.transform.SetParent(null);
    }
}