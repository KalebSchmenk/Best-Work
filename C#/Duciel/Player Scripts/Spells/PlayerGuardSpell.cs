using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGuardSpell : MonoBehaviour
{
    public PlayerInputActions _playerInput;
    private InputAction _guard;

    private Animator _anim;

    [SerializeField] GameObject _guardSphere;

    [SerializeField] private float _cooldownTime = 2.5f;

    private bool _inCooldown = false;
    [SerializeField] GameObject _cooldownOverlay;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();

        _anim = GetComponent<PlayerController>()._anim;
    }

    private void OnEnable()
    {
        _guard = _playerInput.Player.DazeSphere;
        _guard.Enable();
    }
    private void OnDisable()
    {
        _guard.Enable();
    }


    void Update()
    {
        if (_guard.triggered == true && !_inCooldown)
        {
            _inCooldown = true;
            GuardSphere();
        }

        if (_inCooldown)
        {
            if (_cooldownOverlay.activeSelf == false)
            {
                _cooldownOverlay.SetActive(true);
            }
        }
        else
        {
            if (_cooldownOverlay.activeSelf == true)
            {
                _cooldownOverlay.SetActive(false);
            }
        }
    }

    private void GuardSphere()
    {
        _anim.SetTrigger("Spell");
        
        Vector3 upPos = this.transform.position;
        upPos.y += 0.5f;

        Instantiate(_guardSphere, upPos, Quaternion.identity);
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);

        _inCooldown = false;
    }
}
