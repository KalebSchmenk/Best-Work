using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangedEnemyAttackState : RangedEnemyBaseState
{
    private RangedEnemyController _rangedEnemyScript;

    private GameObject _player;
    
    private bool _inAttackCooldown = true;

    private float _attackCooldownTime = 5.0f;

    private bool _canSeePlayer = false;

    private float _proximityRange = 70f;

    private Animator _anim;



    void Start()
    {
        _anim = GetComponent<RangedEnemyController>()._anim;
    }
    
    public override void EnterState(RangedEnemyController rangedEnemy)
    {
        this._rangedEnemyScript = rangedEnemy;

        this._player = rangedEnemy._player;

        StartCoroutine(AttackCooldown());
        StartCoroutine(LookForPlayer());
    }

    public override void UpdateState()
    {
        RotateToPlayer();

        if (_canSeePlayer == false)
        {
            StartCoroutine(CantSeePlayer());
        }

        if (!_inAttackCooldown && _canSeePlayer)
        {
            CastSpellAttack();
        }
    }

    // Rotates enemy to face player
    private void RotateToPlayer()
    {
        var lookPos = _player.transform.position - _rangedEnemyScript.gameObject.transform.position;
        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);

        _rangedEnemyScript.gameObject.transform.rotation = Quaternion.Slerp(_rangedEnemyScript.gameObject.transform.rotation, rotation, Time.deltaTime * 5.0f);
    }

    // Casts a a spell attack
    private void CastSpellAttack()
    {
        _anim.SetTrigger("Spell");

        StartCoroutine(AttackCooldown());

        int randomNumber = Random.Range(1, 10);

        if (randomNumber >= 8)
        {
            CastLightningStrike();
        }
        else
        {
            CastFireball();
        }
    }

    // Cooldown for spell attacks
    private IEnumerator AttackCooldown()
    {
        _inAttackCooldown = true;
        yield return new WaitForSeconds(_attackCooldownTime);
        _inAttackCooldown = false;
    }

    public void CastFireball()
    {
        var fireball = Instantiate(_rangedEnemyScript._fireballPrefab, _rangedEnemyScript._spellCastLocation.position, Quaternion.identity);

        FireballController tempFireballControl = fireball.GetComponent<FireballController>();

        tempFireballControl.Fire(_player.transform.position);
    }

    public void CastLightningStrike()
    {
        Transform spawnAt;

        int randomNumber = Random.Range(1, 10);

        spawnAt = _player.GetComponent<PlayerController>()._lightningSpawnLocation.transform;

        // 50% chance to not hit player
        if (randomNumber >= 6)
        {
            spawnAt.position = new Vector3(spawnAt.position.x + Random.Range(1, 5), spawnAt.position.y, spawnAt.position.z + Random.Range(1, 5));
        }

        var randomRot = new Vector3(0, Random.Range(0, 360), 0);
        Instantiate(_rangedEnemyScript._lightningStrikePrefab, spawnAt.position, Quaternion.Euler(randomRot));
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

    private IEnumerator CantSeePlayer()
    {
        yield return new WaitForSeconds(3.5f);

        if (_canSeePlayer == false)
        {
            var newState = this.gameObject.AddComponent<RangedEnemyLookAroundState>();
            newState.EnterState(_rangedEnemyScript);
            _rangedEnemyScript.currentState = newState;
            Destroy(this);
        }
    }
}
