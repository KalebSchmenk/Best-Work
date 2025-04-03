using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateController : MonoBehaviour
{
    private enum TypeOfObject
    {
        FloatingPlatform,
        Boulder
    }
    [SerializeField] private TypeOfObject _objectType;

    private Rigidbody rb;

    private bool _isLevitating = false;
    private bool _fallingDown = false;
    private Vector3 _startPos;
    private Vector3 _endPos;

    private float _startTime;
    private float _speed = 3.0f;
    private float _journeyLength;

    private void Start()
    {
        if (_objectType == TypeOfObject.Boulder)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        if (_isLevitating)
        {
            float distCovered = (Time.time - _startTime) * _speed;

            float fracOfJourney = distCovered / _journeyLength;

            transform.position = Vector3.Slerp(_startPos, _endPos, fracOfJourney);
        }

        if (_fallingDown)
        {
            float distCovered = (Time.time - _startTime) * _speed;

            float fracOfJourney = distCovered / _journeyLength;

            transform.position = Vector3.Slerp(_startPos, _endPos, fracOfJourney);

            if (transform.position == _endPos)
            {
                _fallingDown = false;
            }
        }
    }

    public void BeginLevitating()
    {
        _isLevitating = true;
        _startPos = this.transform.position;
        _endPos = new Vector3(_startPos.x, _startPos.y + 8, _startPos.z);

        _startTime = Time.time;
        _journeyLength = Vector3.Distance(_startPos, _endPos);

        Debug.Log("Levitating Object");

        StartCoroutine(StopLevitatingCooldown());
    }

    public bool IsLevitating()
    {
        return _isLevitating;
    }

    private IEnumerator StopLevitatingCooldown()
    {
        yield return new WaitForSeconds(10.0f);

        Debug.Log("Levitation spell dropped!");

        _isLevitating = false;

        if (_objectType == TypeOfObject.Boulder)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            _startPos = this.transform.position;
            _endPos = new Vector3(_startPos.x, _startPos.y - 8, _startPos.z);

            _startTime = Time.time;
            _journeyLength = Vector3.Distance(_startPos, _endPos);
            _fallingDown = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && _objectType == TypeOfObject.FloatingPlatform)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.SetParent(this.transform);

            player.GetComponent<PlayerController>()._onPlatform = true;
            player.GetComponent<Rigidbody>().isKinematic = true;
        }
    }


    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && _objectType == TypeOfObject.FloatingPlatform)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.parent = null;

            player.GetComponent<PlayerController>()._onPlatform = false;
            player.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
