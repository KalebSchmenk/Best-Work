using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaunchable
{
    public void Launch(Vector3 dir, float power, float stunLength); // Vector 3 dir is the direction to launch in and power is the strenth of the launching
}
