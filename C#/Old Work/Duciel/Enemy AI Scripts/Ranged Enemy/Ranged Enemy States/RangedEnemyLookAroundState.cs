using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class RangedEnemyLookAroundState : RangedEnemyBaseState
{
    private RangedEnemyController _rangedEnemyScript;

    private GameObject _player;

    private Vector3 _targetRot;

    private bool _lookingAround = true;
    private bool _canSeePlayer = false;

    private float _proximityRange = 70f;


    public override void EnterState(RangedEnemyController rangedEnemy)
    {
        this._rangedEnemyScript = rangedEnemy;

        this._player = _rangedEnemyScript._player;

        _targetRot = new Vector3(_rangedEnemyScript.gameObject.transform.rotation.x, Random.Range(0, 360), _rangedEnemyScript.gameObject.transform.rotation.z);

        StartCoroutine(LookForPlayer());
    }

    public override void UpdateState()
    {
        LookAround();

        if (_canSeePlayer)
        {
            _rangedEnemyScript._enemySpottedObject.Play();
            var newState = this.AddComponent<RangedEnemyAttackState>();
            newState.EnterState(_rangedEnemyScript);
            _rangedEnemyScript.currentState = newState;
            Destroy(this);
        }
    }

    // Rotates to a random y direction
    private void LookAround()
    {
        if (_lookingAround == false) return;


        var step = 15f * Time.deltaTime;

        _rangedEnemyScript.gameObject.transform.rotation = Quaternion.RotateTowards(_rangedEnemyScript.gameObject.transform.rotation, Quaternion.Euler(_targetRot), step);

        if (Mathf.Abs(_rangedEnemyScript.gameObject.transform.rotation.eulerAngles.y - _targetRot.y) <= 0.1)
        {
            StartCoroutine(LookAroundCooldown());
        }
    }

    // Cooldown after reaching the rotation goal
    private IEnumerator LookAroundCooldown()
    {
        _lookingAround = false;

        yield return new WaitForSeconds(5f);

        _targetRot = new Vector3(_rangedEnemyScript.gameObject.transform.rotation.x, Random.Range(0, 360), _rangedEnemyScript.gameObject.transform.rotation.z);

        _lookingAround = true;
    }

    // Looks for player every 0.2 seconds
    private IEnumerator LookForPlayer()
    {
        while (true)
        {
            RaycastHit hit;

            var rayOrigin = _rangedEnemyScript.gameObject.transform.position;
            var rayTarget = _player.transform.position - _rangedEnemyScript.gameObject.transform.position;
            rayOrigin.y += 2f;
            rayTarget.y -= 1;

            if (Physics.Raycast(rayOrigin, rayTarget, out hit, _proximityRange))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    _canSeePlayer = true;
                }
                else
                {
                    _canSeePlayer = false;
                }
            }
            else
            {
                _canSeePlayer = false;
            }

            yield return new WaitForSeconds(0.2f); 
        }
    }
}
