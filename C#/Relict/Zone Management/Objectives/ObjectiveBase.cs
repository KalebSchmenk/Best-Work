using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBase : MonoBehaviour
{
    [Header("Base Objective Settings")]
    [SerializeField] public int order = 1; // Order of objective

    [SerializeField] private string objectiveName; // Objective name

    public string ObjectiveDescription;

    protected bool isActive = false; // Is this objective active flag

    protected ZoneManager zoneManager; // Current zone manager

    // Virtual func that children will override
    public virtual ObjectiveBase StartObjective()
    {
        return this;
    }

    // Virtual func for if the objective is fauled
    public virtual ObjectiveBase FailedObjective()
    {
        return this;
    }

    // On objective complete, inform zone manager
    public virtual void FinishObjective()
    {
        isActive = false;
        zoneManager.ObjectiveComplete(this);
    }

    public override string ToString()
    {
        return objectiveName;
    }

    protected virtual void Start()
    {
        zoneManager = GameObject.FindGameObjectWithTag("ZoneManager").GetComponent<ZoneManager>();
        if (zoneManager == null )
        {
            Debug.LogError(this + " could not find the zone manager!");
        }
    }
}
