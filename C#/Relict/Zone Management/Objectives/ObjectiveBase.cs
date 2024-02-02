using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBase : MonoBehaviour
{
    [SerializeField] public int order = 1; // Order of objective

    [SerializeField] private string objectiveName; // Objective name

    protected bool isActive = false; // Is this objective active flag

    protected ZoneManager zoneManager; // Current zone manager

    // Virtual func that children will override
    public virtual ObjectiveBase StartObjective()
    {
        return this;
    }

    // On objective complete, inform zone manager
    public void FinishObjective()
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
