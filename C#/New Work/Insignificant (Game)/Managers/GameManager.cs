using UnityEngine;

/// <summary>
/// DESIGN PATTERNS: Singleton and Observer.
/// Singleton: Only one Game Manager in the game.
/// Observer: Tracks the Game State.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public delegate void TaskComplete();
    public static TaskComplete OnTaskComplete;

    public GameObject winObj;
    public GameObject objective;

    public GameObject Player { get; private set; }

    private int tasksNeeded = 3;
    private int taskCount = 0;

    private void Awake()
    {
        OnTaskComplete += MachineRepaired;
    }

    private void OnDestroy()
    {
        OnTaskComplete -= MachineRepaired;
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Track when a machine is repaired. When all machines are repaired, win game.
    /// </summary>
    private void MachineRepaired()
    {
        ++taskCount;

        if (taskCount >= tasksNeeded)
        {
            // Game over!
            winObj.SetActive(true);
            objective.SetActive(false);
            Time.timeScale = 0;
        }
    }
}
