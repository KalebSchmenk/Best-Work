using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KorosShockwaveLineController : MonoBehaviour
{
    public GameObject objToDrawTo;
    private LineRenderer lr; // Linerenderer ref

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        DrawLineRenderer();
    }

    // Draws lines based on active moving colliders
    private void DrawLineRenderer()
    {
        if (lr == null || objToDrawTo == null) { Destroy(lr); return; }

        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, objToDrawTo.transform.position);
    }
}
