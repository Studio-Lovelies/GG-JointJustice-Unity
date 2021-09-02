using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Evidence Dictionary", menuName = "Evidence/Evidence Dictionary")]
public class EvidenceDictionary : ScriptableObject
{
    [field: SerializeField, Tooltip("Add all the evidence required for this list here.")]
    public List<Evidence> List { get; private set; }
    
    public Dictionary<string, Evidence> Dictionary { get; private set; }

    private void OnEnable()
    {
        if (Dictionary == null || Dictionary.Count != List.Count)
        {
            Dictionary = new Dictionary<string, Evidence>();

            foreach (var evidence in List)
            {
                // TODO fix this
                Dictionary.Add(evidence.name + Random.RandomRange(0, 1111111), evidence);
            }
        }
    }
}
