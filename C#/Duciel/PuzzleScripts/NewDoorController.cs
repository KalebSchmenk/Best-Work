using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorController : MonoBehaviour
{
    private Rigidbody _rb;

    private bool _openDoor = false;

    private Vector3 _startPos;
    private Vector3 _endPos;

    private float _startTime;
    private float _speed = 3.0f;
    private float _journeyLength;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_openDoor == true)
        {
            float distCovered = (Time.time - _startTime) * _speed;

            float fracOfJourney = distCovered / _journeyLength;

            transform.position = Vector3.Slerp(_startPos, _endPos, fracOfJourney);
        }
    }

    public void OpenDoor()
    {
        _openDoor = true;
        _rb.useGravity = false;

        _startPos = this.transform.position;
        _endPos = new Vector3(_startPos.x, _startPos.y + 8, _startPos.z);

        _startTime = Time.time;
        _journeyLength = Vector3.Distance(_startPos, _endPos);
    }

    public void CloseDoor()
    {
        _openDoor = false;
        _rb.useGravity= true;
    }
}
