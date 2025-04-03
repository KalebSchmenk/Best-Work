using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Simple tag component for factories to track a gameobject.
/// </summary>
public class FactoryTag : MonoBehaviour
{
    public string FTag { get; private set; }

    /// <summary>
    /// Changes the factory tag's tag.
    /// </summary>
    /// <param name="tag">New tag.</param>
    public void ChangeTag(string tag)
    {
        this.FTag = tag;
    }

    public override string ToString()
    {
        return FTag;
    }
}

