using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCursorController : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTex;
    private CursorMode cursorMode = CursorMode.Auto;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(_cursorTex, Vector2.zero, cursorMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
