using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SpecialScene
{
    [SerializeField] public string sceneName = "Scene"; // Scene name for scene loading
#if UNITY_EDITOR
    public SceneAsset scene; // Editor scene reference
#endif

    // If we are in the editor, set the scene name string to scene reference name
    public void SetSceneName()
    {
#if UNITY_EDITOR
        if (scene == null) return;
        sceneName = scene.name;
#endif
        return;
    }

    public string GetSceneToString()
    {
        return sceneName;
    }
}
