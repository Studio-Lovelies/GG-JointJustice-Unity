using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Contains all different special actions used in code. Order matters, as topmost value is most important, and lowest least important.
///</summary>
public enum WaiterTypes
{
    letter,
    punctuation,
    defaultPunctuation,
    dialog,
    overall,
    defaultValue,
}