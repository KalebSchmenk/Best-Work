using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupText : MonoBehaviour
{
    Transform playerObject;
    [SerializeField] private GameObject _planeRef;
    [SerializeField] GameObject pickupInfo;
  
    bool inRange;



    private void Update() {
        if(inRange){
            FacePlayer();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            pickupInfo.SetActive(true);
            inRange = true;
            Debug.Log("Player Entered");
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player"))
            pickupInfo.SetActive(false);
            inRange = false;
    }

    private void OnTriggerStay(Collider other) {
        playerObject = other.transform;
    }

    private void FacePlayer(){
         var lookPos = playerObject.transform.position - _planeRef.transform.position;
        lookPos.y = 0;
        lookPos.x = -lookPos.x;
        lookPos.z = -lookPos.z;

        var rotation = Quaternion.LookRotation(lookPos);
        

        _planeRef.transform.rotation = Quaternion.Slerp(_planeRef.transform.rotation, rotation, Time.deltaTime * 5.0f);
    }
}

