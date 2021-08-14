using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class NewSpokenLineEvent : UnityEvent<string> //TODO: Use a custom struct for the data?
{
}

[System.Serializable]
public class NewActionLineEvent : UnityEvent<string> //TODO: Use a custom struct for the data?
{
}