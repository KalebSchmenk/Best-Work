using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Level Completion Bools")]
    public static bool _completedCombatLevel = false;
    public static bool _completedPuzzleLevel = false;
    public static bool _completedFinalLevel = false;
    public static bool _hasCompletedTutorial = false;

    [Header("Level Access Blockers")]
    [SerializeField] private GameObject _gateToPuzzle;
    [SerializeField] private GameObject _gateToFinal;

    [SerializeField] private GameObject _puzzleDoorLeft;
    [SerializeField] private GameObject _puzzleDoorRight;

    [SerializeField] private GameObject _finalDoorLeft;
    [SerializeField] private GameObject _finalDoorRight;

    [Header("Spawn After Tutorial Completion")]
    [SerializeField] Transform _positionOfNewSpawn;

    void Start()
    {
        CheckpointManager._currentCheckpoint = null;

        if (SceneManager.GetActiveScene().name == "HUBLevel" && _hasCompletedTutorial == true)
        {
            if (_positionOfNewSpawn == null) return;

            GameObject.FindGameObjectWithTag("Player").transform.position = _positionOfNewSpawn.position;
        }

        GateEmissions();

        if (_puzzleDoorLeft != null && _completedCombatLevel == true)
        {
            _puzzleDoorRight.transform.Rotate(_puzzleDoorRight.transform.rotation.x, _puzzleDoorRight.transform.rotation.y + 90, _puzzleDoorRight.transform.rotation.z);
            _puzzleDoorLeft.transform.Rotate(_puzzleDoorLeft.transform.rotation.x, _puzzleDoorLeft.transform.rotation.y - 90, _puzzleDoorLeft.transform.rotation.z);
        }

        if (_finalDoorLeft != null && _completedPuzzleLevel == true)
        {
            _finalDoorRight.transform.Rotate(_finalDoorRight.transform.rotation.x, _finalDoorRight.transform.rotation.y + 90, _finalDoorRight.transform.rotation.z);
            _finalDoorLeft.transform.Rotate(_finalDoorLeft.transform.rotation.x, _finalDoorLeft.transform.rotation.y - 90, _finalDoorLeft.transform.rotation.z);
        }
    }

    private void GateEmissions()
    {
        if (_gateToPuzzle == null || _gateToFinal == null) return;

        if (!_completedCombatLevel)
        {
            _gateToPuzzle.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        }
        else
        {
            _gateToPuzzle.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        }

        if (!_completedPuzzleLevel)
        {
            _gateToFinal.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        }
        else
        {
            _gateToFinal.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        }
    }
}
