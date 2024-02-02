using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraController : MonoBehaviour
{
    private GameObject mainCam;

    private void Start()
    {
        mainCam = GameManager.instance.mainCamera;
    }

    void Update()
    {
        this.gameObject.transform.LookAt(mainCam.transform.position);
    }
}
