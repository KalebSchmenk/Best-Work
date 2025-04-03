using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class RangedEnemyController : MonoBehaviour, EnemyHealthInterface
{
    [Header("Player Information")]
    [System.NonSerialized] public GameObject _player;
    private PlayerController _playerScript;

    [Header("Animator")]
    public Animator _anim;

    [Header("Health")]
    private int _health = 5;
    public int health { get { return _health; } set { _health = value; } }

    [Header("Spell Information")]
    [SerializeField] protected Transform _lightningStrikeLocation;
    public Transform hitLocation { get { return _lightningStrikeLocation; } set { _lightningStrikeLocation = value; } }

    public Transform _spellCastLocation;
    public GameObject _fireballPrefab;
    public GameObject _lightningStrikePrefab;

    [Header("Audio")]
    public AudioSource _enemySpottedObject;
    public AudioClip _enemySpottedClip;

    public RangedEnemyBaseState currentState;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerScript = _player.GetComponent<PlayerController>();

        _enemySpottedObject.clip = _enemySpottedClip;

        currentState = this.AddComponent<RangedEnemyLookAroundState>();
        currentState.EnterState(this);
    }

    void Update()
    {
        CheckHealth();

        // Calls the update function of the current state
        currentState.UpdateState();
    }

    // Destroys enemy if they are under or equal 0 HP
    private void CheckHealth()
    {
        if (_health <= 0)
        {
            // Kill enemy
            Destroy(this.gameObject);
        }
    }

    // Take damage interface implementation
    public void TakeDamage(int damage)
    {
        _anim.SetTrigger("Hit");
        _health -= damage;
        CheckHealth();
    }
}
