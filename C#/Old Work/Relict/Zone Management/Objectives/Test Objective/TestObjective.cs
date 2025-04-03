using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjective : ObjectiveBase
{
    public override ObjectiveBase StartObjective()
    {
        print("Objective " + this + " was started!");
        isActive = true;
        return this;
    }

    private void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.B))
        {
            FinishObjective();
        }
    }
}
