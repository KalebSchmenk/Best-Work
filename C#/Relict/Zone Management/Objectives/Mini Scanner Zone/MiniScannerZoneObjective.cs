using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniScannerZoneObjective : ObjectiveBase
{
    public List<MiniScannerController> scannerZoneObjectives = new List<MiniScannerController>();

    public override ObjectiveBase StartObjective()
    {
        print("Objective " + this + " was started!");
        isActive = true;

        SetAllScannersAvailable();

        return this;
    }

    public override void FinishObjective()
    {
        base.FinishObjective();

        foreach (var controller in scannerZoneObjectives)
        {
            controller.DisableScanner();
        }
    }

    public void ScannerStarted(MiniScannerController newCurrentScanner)
    {
        foreach(var scanner in scannerZoneObjectives)
        {
            if (!GameObject.ReferenceEquals(scanner.gameObject, newCurrentScanner.gameObject))
            {
                scanner.SetUnavailable();
            }
        }
    }

    public void ScannerFinished(MiniScannerController scanner)
    {
        scannerZoneObjectives.Remove(scanner);

        if (scannerZoneObjectives.Count <= 0)
        {
            FinishObjective();
        }
        else
        {
            SetAllScannersAvailable();
        }
    }

    private void SetAllScannersAvailable()
    {
        foreach (var scanner in scannerZoneObjectives)
        {
            scanner.SetAvailable(this);
        }
    }
}
