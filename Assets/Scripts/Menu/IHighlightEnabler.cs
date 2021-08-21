using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHighlightEnabler
{
    bool HighlightEnabled { get; }
    void SetHighlighted(bool highlighted);
}
