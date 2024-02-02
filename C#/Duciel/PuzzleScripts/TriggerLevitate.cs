using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLevitate : MonoBehaviour
{
    LevBoulder boulder;
    // Start is called before the first frame update
    void Start()
    {
        boulder = GetComponent<LevBoulder>();
    }


    private void OnTriggerEnter(Collider other)
    {
                boulder.levitate = true;
                other.transform.SetParent(transform);
                Debug.Log("Levitate");


     }


    private void OnTriggerExit(Collider other)


    {
        other.transform.SetParent(null);
    }
}        
   
