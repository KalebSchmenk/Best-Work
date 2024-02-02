using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullController : MonoBehaviour
{
    private LevitateController _levitateController;

    private Rigidbody rb;

    private void Start()
    {
        _levitateController = GetComponent<LevitateController>();

        rb = GetComponent<Rigidbody>();
    }


    public void Pull(GameObject player)
    {
        if (_levitateController.IsLevitating() == true) return;
        
        Debug.Log("Pull Object");

        rb.AddForce((player.transform.position - this.transform.position) * 150);
    }


}
