using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorTriggerController : MonoBehaviour
{
    [SerializeField] private GameObject _door;

    private NewDoorController _doorController;
    
    void Start()
    {
        _doorController = _door.GetComponent<NewDoorController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CanManipulate"))
        {
            _doorController.OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CanManipulate"))
        {
            _doorController.CloseDoor();
        }
    }
}
