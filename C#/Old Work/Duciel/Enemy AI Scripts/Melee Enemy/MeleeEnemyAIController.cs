using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MeleeEnemyAIController : MonoBehaviour, EnemyHealthInterface
{
    public enum AIState
    {
        Roam,
        Search,
        Attack,
        Dazed,
        Chase
    }

    [SerializeField] AudioSource EnemyAttackObject;
    [SerializeField] AudioClip  EnemyAttackClip; 

    [SerializeField] AudioSource EnemySpottedObject;
    [SerializeField] AudioClip  EnemySpottedClip; 

    bool playSpottedSound;

    
    
    public AIState _AIState;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rb;

    [SerializeField]
    [Range(0.0f, 250.0f)] private float _walkRadius;

    [SerializeField]
    [Range(1.0f, 15.0f)] private float _waitTime;

    [SerializeField]
    [Range(0.0f, 75.0f)] private float _proximityRange;

    [SerializeField] private float _proxCheckWaitTime;
    [SerializeField] private float _navMeshAgentSprintSpeed = 5.0f;
    [SerializeField] private float _navMeshAgentJogSpeed = 4.0f;
    [SerializeField] private float _alwaysSeeRadius = 10f;
    [SerializeField] private float _attackCooldown = 1.5f;
    private float _navMeshSpeed;

    private bool _canSeePlayer;
    private bool _IAmWaiting = false;

    private Vector3 _lastPosition;
    private Vector3 _lastKnownLocation;

    private GameObject _player;
    private PlayerController _playerScript;

    [SerializeField] private GameObject _attackSphere;
    [SerializeField] private Transform _attackSphereLocation;

    [SerializeField] public int _health = 5;
    [SerializeField] private float _dazedFor = 2.5f;
    public int health { get { return _health; } set { _health = value; } }

    [SerializeField] protected Transform _lightingStrikeLocation;
    public Transform hitLocation { get { return _lightingStrikeLocation; } set { _lightingStrikeLocation = value; } }

    private bool _inAttackCooldown = false;

    [SerializeField] Animator _anim;

    [SerializeField] public bool _isFinalBoss = false;
    public int _bossHealth = 15;

    [SerializeField] AudioSource bossSoundsObject;
    [SerializeField] AudioClip bossSoundsClip;
    [SerializeField] GameObject droppedHammer;
    public bool _bossDead;
    bool spawnedHammer = false;
   

    void Start()
    {
        if (_isFinalBoss){
            _health = 15;
            _bossHealth = 15;
        } 

        _AIState = AIState.Roam;
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _player = GameObject.FindGameObjectWithTag("Player");

        _navMeshSpeed = _navMeshAgent.speed;

        _rb = GetComponent<Rigidbody>();


        StartCoroutine(ProximityCheck());
    }

    void Update()
    {

        CheckDistance();
        CheckHealth();

        _rb.angularVelocity = Vector3.zero;

        switch (_AIState)
        {
            // Roam around the nav mesh randomly state
            case AIState.Roam:
                Wander();

                if (_IAmWaiting == true)
                {
                    _anim.Play("Idle");
                }
                else
                {
                    _anim.Play("Walk");
                }

                if (_canSeePlayer == true) _AIState = AIState.Chase;
                break;

            // Chase the player state
            case AIState.Chase:
                Chase();

                _anim.Play("Walk");

                if (_canSeePlayer == false)
                {
                    _AIState = AIState.Search;
                    _lastKnownLocation = _player.transform.position;
                }
                break;

            // Search for the player at their last known position state
            case AIState.Search:
                Search();

                _anim.Play("Walk");

                if (_canSeePlayer == true) _AIState = AIState.Chase;

                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _canSeePlayer == false)
                {
                    _AIState = AIState.Roam;
                }
                break;

            // Attacks player
            case AIState.Attack:
                AttackPlayer();

                _anim.Play("Idle");

                _rb.velocity = Vector3.zero;

                if (_canSeePlayer == false)
                {
                    _navMeshAgent.enabled = true;
                    _AIState = AIState.Search;
                    _lastKnownLocation = _player.transform.position;
                }
                break;

            case AIState.Dazed:
                Dazed();

                _anim.Play("Idle");

                break;

        }
        if(_canSeePlayer == true){
            playSpottedSound = true;
        }
    }


    private void CheckHealth()
    {
        if (_health <= 0)
        {
            // Kill enemy
            if (_isFinalBoss)
            {
                if(spawnedHammer == false){

                    Vector3 bossPosition = transform.position;
                    bossPosition.y = bossPosition.y - 0.75f;
                    Instantiate(droppedHammer, bossPosition, Quaternion.identity);
                    spawnedHammer = true;
                }
                _bossDead = true;
                Destroy(this.gameObject, 0.2f);

            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < 1.5f)
        {
            _AIState = AIState.Attack;
        }
    }

    private void AttackPlayer()
    {
        _navMeshAgent.enabled = false;

        RotateToPlayer();

        if (_inAttackCooldown) return;

        if (Vector3.Distance(transform.position, _player.transform.position) > 1.5f)
        {
            Debug.LogWarning("Player is too far away");
            _navMeshAgent.enabled = true;
            _AIState = AIState.Chase;
            return;
        }

        Debug.Log("I am not in cooldown");

        _anim.SetTrigger("Attack");

        Instantiate(_attackSphere, _attackSphereLocation.position, Quaternion.identity);

        _inAttackCooldown = true;

        EnemyAttackObject.clip = EnemyAttackClip;
        EnemyAttackObject.Play();

        StartCoroutine(AttackCooldown());
    }
    private void RotateToPlayer()
    {
        var lookPos = _player.transform.position - transform.position;
        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
    }

    // Wanders to a random point
    private void Wander()
    {
        if (_navMeshAgent.remainingDistance >= _navMeshAgent.stoppingDistance) _IAmWaiting = false;

        if (_IAmWaiting == false && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.speed = _navMeshSpeed;
            _IAmWaiting = true;
            StartCoroutine(WaitTimer());
        }
    }

    // Finds a random point on the nav mesh to roam to
    private Vector3 RandomNavMeshLocation()
    {
        // Loop may be performance heavy but ensures
        // that the position is on the nav mesh
        while (true)
        {
            Vector3 targetPosition = Vector3.zero;
            Vector3 randomPosition = Random.insideUnitSphere * _walkRadius;
            randomPosition += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, _walkRadius, 1) && hit.position != _lastPosition)
            {
                targetPosition = hit.position;

                _lastPosition = targetPosition;

                return targetPosition;

            }
        }
    }

    // Guard Chases Player
    private void Chase()
    {
        _navMeshAgent.enabled = true;

        RotateToPlayer();

        if (playSpottedSound == true){
        EnemySpottedObject.clip = EnemyAttackClip;
        EnemyAttackObject.Play();
        playSpottedSound = false;
        }
        _navMeshAgent.SetDestination(_player.transform.position);
        _navMeshAgent.speed = _navMeshAgentSprintSpeed;
    }

    // Guard Searches Players Last Known Position
    private void Search()
    {
        _IAmWaiting = false;
        _navMeshAgent.SetDestination(_lastKnownLocation);
        _navMeshAgent.speed = _navMeshAgentJogSpeed;
    }

    private void Dazed()
    {
        //_navMeshAgent.ResetPath();
        _navMeshAgent.enabled = false;
    }

    // Wait co-routine that halts the guard for a moment after it reaches its destination
    private IEnumerator WaitTimer()
    {
        _IAmWaiting = true;
        yield return new WaitForSeconds(_waitTime);

        // State Roam is the only caller of this function. This is why setting a destination here is acceptable
        if (_AIState == AIState.Roam) _navMeshAgent.SetDestination(RandomNavMeshLocation());
    }

    // Guard Shoots Raycast At Max Range _proximityRange Towards Player To See If Player Is Close Enough To Be Chased
    private IEnumerator ProximityCheck()
    {
        RaycastHit hit;

        var rayOrigin = transform.position;
        var rayTarget = _player.transform.position - transform.position;
        rayOrigin.y += 2f;
        rayTarget.y -= 1;

        if (Physics.Raycast(rayOrigin, rayTarget, out hit, _proximityRange))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                Vector3 toPlayer = (_player.transform.position - transform.position).normalized;

                // FOV check using a dot product. If the dot product of the guards forward vector and the direction to the player is greater than
                // 0.55f, we consider the player to be in the visual FOV of the guard
                if (Vector3.Dot(transform.TransformDirection(Vector3.forward).normalized, toPlayer) > 0.55f)
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
        }
        else
        {
            _canSeePlayer = false;
        }

        // If player is very close to the guard, we assume the guard would notice the players presence and consider them as seeing the player
        if (Vector3.Distance(transform.position, _player.transform.position) < _alwaysSeeRadius)
        {
            _canSeePlayer = true;
        }

        yield return new WaitForSeconds(_proxCheckWaitTime);

        StartCoroutine(ProximityCheck());
    }

    private IEnumerator AttackCooldown()
    {
        if (_isFinalBoss) bossSoundsObject.PlayOneShot(bossSoundsClip);
        yield return new WaitForSeconds(_attackCooldown);

        _inAttackCooldown = false;
        
    }

    private IEnumerator DazedTimer()
    {
        yield return new WaitForSeconds(_dazedFor);

        if (_canSeePlayer)
        {
            _AIState = AIState.Chase;
            _IAmWaiting = false;
        }
        else
        {
            _AIState = AIState.Roam;
            _IAmWaiting = false;
        }

        _navMeshAgent.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DazeSphere"))
        {
            _AIState = AIState.Dazed;
            _rb.velocity = Vector3.zero;
            StartCoroutine(DazedTimer());
        }

        if (other.gameObject.CompareTag("PlayerAttackSphere"))
        {
            TakeDamage(2);
        }
    }

    public void TakeDamage(int damage)
    {
        _anim.SetTrigger("Hit");
        _health -= damage;
        CheckHealth();
        // Play damage sound and any anim

        _AIState = AIState.Chase;
    }

}
