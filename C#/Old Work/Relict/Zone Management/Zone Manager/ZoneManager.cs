using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [NonSerialized] public ObjectiveBase activeObjective; // Current active objective

    public float startNextObjectiveIn = 3.5f;

    private List<ObjectiveBase> objectives = new List<ObjectiveBase>();

    public GameObject chariot;

    private void Start()
    {

        var objectiveObjs = GameObject.FindGameObjectsWithTag("Objective");
        foreach (var gameObj in objectiveObjs)
        {
            objectives.Add(gameObj.GetComponent<ObjectiveBase>());
        }

        if (!objectives.Any())
        {
            Debug.LogWarning(this + " could not find any objectives! If this is expected, disregard");
            return;
        }

        IEnumerable<ObjectiveBase> sortAscendingQuery =
            from obj in objectives
            orderby obj.order
            select obj;

        objectives = sortAscendingQuery.ToList();

        Invoke("StartNextObjective", startNextObjectiveIn);
    }

    public void ObjectiveComplete(ObjectiveBase objective)
    {
        print("Objective " + objective + " completed!");
        objectives.Remove(objective);

        Invoke("StartNextObjective", startNextObjectiveIn);
    }

    public void StartNextObjective()
    {
        if (objectives.Count > 0) 
        {
            print("Starting new objective...");
            activeObjective = objectives[0].StartObjective();

            GameManager.instance.UpdateObjective(activeObjective.ObjectiveDescription);
        }
        else
        {
            print("All objectives completed! Calling chariot...");
            chariot.SetActive(true);
        }
    }
}
