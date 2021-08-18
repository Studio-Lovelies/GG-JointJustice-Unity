using System;
using UnityEngine;

/// <summary>
/// /// This is an abstract class that buttons can inherit from in order to
/// implement their own highlighting methods
/// When a user hovers over or selects a button it will be
/// highlighted.
/// </summary>
[Serializable]
public abstract class HightlightableMenuItem : MonoBehaviour, IHightlightableMenuItem
{
    public abstract void SetHighlighted(bool highlighted);
}
