using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [Header("Plane Reference")]
    [SerializeField] private GameObject _planeRef;

    [Header("Particle Reference")]
    [SerializeField] private GameObject _part;

    private Camera _mainCam;

    private bool _playerInSphere = false;

    void Start()
    {
        _planeRef.SetActive(false);

        _part.SetActive(false);

        _mainCam = Camera.main;
    }

    void Update()
    {
        if (_playerInSphere)
        {
            RotateToPlayer();
        }
    }
    private void RotateToPlayer()
    {
        var lookPos = _mainCam.transform.position - _planeRef.transform.position;
        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);

        _planeRef.transform.rotation = Quaternion.Slerp(_planeRef.transform.rotation, rotation, Time.deltaTime * 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _planeRef.SetActive(true);
            _part.SetActive(true);
            _playerInSphere = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _planeRef.SetActive(false);
            _part.SetActive(false);
            _playerInSphere = false;
        }
    }
}
