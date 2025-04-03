using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DESIGN PATTERN: Object Pooling
/// </summary>
public class GenericGameObjectPool : MonoBehaviour
{
    int count;
    GameObject poolObj;
    List<GameObject> objectPool = new List<GameObject>();

    /// <summary>
    /// Creates gameobject pool by instantiating the pooling gameobject INITCOUNT times.
    /// </summary>
    /// <param name="poolObj">Gameobject to pool.</param>
    /// <param name="initCount">The starting pool size.</param>
    public void Init(GameObject poolObj, int initCount = 5)
    {
        this.poolObj = poolObj;

        for (int i = 0; i < initCount; ++i)
        {
            var obj = Instantiate(poolObj);
            Add(obj);
        }
    }

    /// <summary>
    /// Disables gameobject and adds it to pool.
    /// Changes it name to match the pool object for comparing later. (Don't love this but its the simplest solution)
    /// </summary>
    /// <param name="obj">Gameobject to add to pool.</param>
    private void Add(GameObject obj)
    {
        obj.SetActive(false);
        obj.name = poolObj.name + count;
        ++count;
        objectPool.Add(obj);
    }

    /// <summary>
    /// Enables and returns an object from the pool.
    /// </summary>
    /// <returns>Enabled pooled gameobject.</returns>
    public GameObject Take()
    {
        if (objectPool.Count > 0)
        {
            var obj = objectPool[0];
            objectPool.RemoveAt(0);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var obj = Instantiate(poolObj);
            obj.name = poolObj.name + count;
            ++count;
            return obj;
        }
    }

    /// <summary>
    /// Checks if the returning gameobject is of the same prefab type as the pooled object. Adds it to the pool if so.
    /// </summary>
    /// <param name="obj">Gameobject to return.</param>
    public void Return(GameObject obj)
    {
        if (obj.name.Contains(poolObj.name))
        {
            Add(obj);
        }
        else
        {
            Debug.LogWarning("This is not the type of Prefab this object pool is pooling... Ignoring return. (Was the object's name changed?)");
        }
    }
}
