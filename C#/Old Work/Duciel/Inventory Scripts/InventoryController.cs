using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InventoryController : MonoBehaviour
{
    public PlayerInputActions _playerInput;
    private InputAction _craftHealth;

    private PlayerController _playerScript;

    public static int _mushroomCount = 0;
    public static int _cherryCount = 0;
    public static int _leafCount = 0;
    public static int _pebbleCount = 0;

    [Header("Health Objects")]
    [SerializeField] TMP_Text _potionCountText;
    [SerializeField] TMP_Text _mushroomCountText;
    [SerializeField] TMP_Text _cherryCountText;



    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _craftHealth = _playerInput.Player.CraftHealthPotion;
        _craftHealth.Enable();
    }
    private void OnDisable()
    {
        _craftHealth.Disable();
    }

    void Start()
    {
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    
    void Update()
    {
        UpdateCounts();
        CheckForHealthPotion();
    }

    private void CheckForHealthPotion()
    {
        if (_mushroomCount >= 1 && _cherryCount >= 1)
        {
            if (_craftHealth.triggered && _playerScript.GetHealth() != 100)
            {
                _playerScript.Heal();

                _mushroomCount -= 1;
                _cherryCount -= 1;
            }
        }

        _potionCountText.text = "" + CountHealthPotions();
    }

    private int CountHealthPotions()
    {
        if (_mushroomCount < _cherryCount)
        {
            return _mushroomCount / 1;
        }
        else
        {
            return _cherryCount / 1;
        }
    }

    private void UpdateCounts()
    {
        _mushroomCountText.text = "" + _mushroomCount;
        _cherryCountText.text = "" + _cherryCount;
    }
}
