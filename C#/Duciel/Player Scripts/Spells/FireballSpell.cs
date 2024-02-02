using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireballSpell : MonoBehaviour
{
    public PlayerInputActions _playerInput;
    private InputAction _fireball;

    [SerializeField] private GameObject _testFireballPrefab;
    [SerializeField] private Transform _spellCastLocation;
    [SerializeField] float _cooldownTime = 2.5f;

    private Camera _mainCam;

    private Animator _anim;

    private bool _fireballCooldown = false;
    [SerializeField] GameObject _cooldownOverlay;

    [Header("Player Sounds")]
    [SerializeField] AudioSource playerFireBallCastObject;
    [SerializeField] AudioClip playerFireBallCastClip;

    void Start()
    {
        _mainCam = Camera.main;

        _anim = GetComponent<PlayerController>()._anim;
    }
    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _fireball = _playerInput.Player.Fireball;
        _fireball.Enable();
    }
    private void OnDisable()
    {
        _fireball.Enable();
    }

    void Update()
    {
        if (_fireball.triggered && !_fireballCooldown)
        {
            CastFireball();
        }

        if (_fireballCooldown)
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

    private void CastFireball()
    {
        RaycastHit hit;
        Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            _anim.SetTrigger("Spell");

            var fireball = Instantiate(_testFireballPrefab, _spellCastLocation.position, Quaternion.identity);

            FireballController tempFireballControl = fireball.GetComponent<FireballController>();

            tempFireballControl.Fire(hit.point);
        }

        playerFireBallCastObject.clip = playerFireBallCastClip;
        playerFireBallCastObject.Play();

        StartCoroutine(FireballCooldown());
    }

    private IEnumerator FireballCooldown()
    {
        _fireballCooldown = true;

        yield return new WaitForSeconds(_cooldownTime);

        _fireballCooldown = false;
    }

}
