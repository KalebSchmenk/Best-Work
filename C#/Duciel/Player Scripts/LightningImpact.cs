using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningImpact : MonoBehaviour
{
private void Update() {
    
    Destroy(this.gameObject, 7);
}
}
