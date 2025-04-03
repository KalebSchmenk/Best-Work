using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class SpecialSceneManager : Editor
{
    // Called on compile
    static SpecialSceneManager()
    {
        Debug.Log("Loading Scenes Every Frame");
        EditorApplication.update += FakeUpdate;
    }

    // Called every editor update
    public static void FakeUpdate()
    {
        ProcessScenes();
    }

    // Tells all found special scenes to set their name strings
    public static void ProcessScenes()
    {
        var scriptables = Resources.FindObjectsOfTypeAll<LevelZone>();

        foreach (var item in scriptables)
        {
            if (item.scene != null) item.scene.SetSceneName();
        }
    }
}
