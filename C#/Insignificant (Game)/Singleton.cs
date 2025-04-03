using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// DESIGN PATTERN: Singleton.
/// Derived classes can inherit from this base singleton class to gain singleton functionality.
/// Generic with the T type. Constrained to a MonoBehaviour class type.
/// Assumes we want a Unity Gameobject Singleton.
/// Awake will ALWAYS be called but the object may be destroyed immediately after. Ensure functionality has this in mind.
/// </summary>
/// <typeparam name="T">Class to be made a Singleton</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    public Singleton()
    {
        SceneManager.sceneLoaded += OnLoad;
    }

    ~Singleton()
    {
        SceneManager.sceneLoaded -= OnLoad;
    }

    /// <summary>
    /// When a scene loads and instance is null, init the instance.
    /// </summary>
    /// <param name="scene">Scene load param</param>s
    /// <param name="mode">Scene load mode param</param>
    private void OnLoad(Scene scene, LoadSceneMode mode)
    {
        InitInstance();
    }

    /// <summary>
    /// Look for all known objects of type T.
    /// Set INSTANCE to the last in the list if INSTANCE was null. Else just remove INSTANCE from the list.
    /// Destroy immediate all other objects.
    /// Set INSTANCE to not destroy on load.
    /// Always use INSTANCE reference and not THIS as it is not promised that THIS == INSTANCE.
    /// </summary>
    private void InitInstance()
    {
        var objs = FindObjectsByType<T>(FindObjectsSortMode.InstanceID).ToList<T>();
        objs.Reverse();
        
        if (Instance == null)
        {
            Instance = objs[0];
            objs.RemoveAt(0);
        }
        else
        {
            objs.Remove(Instance);
        }
        
        foreach (var obj in objs)
        {
            Debug.LogWarning($"Another object of type {typeof(T)} was found named {obj.name}. Destroying...");
            DestroyImmediate(obj.gameObject);
        }

        if (Instance != null) DontDestroyOnLoad(Instance.gameObject);
    }
}
