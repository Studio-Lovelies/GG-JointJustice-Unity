using System.Linq;
using UnityEngine;

public class EvidenceInventory : ObjectInventory<Evidence, EvidenceList>, ICourtRecordObjectInventory
{
    /// <summary>
    /// Checks if evidence has been added to the evidence inventory
    /// and swaps it with its alternate evidence.
    /// </summary>
    /// <param name="evidenceName">The name of the evidence to substitute.</param>
    public void SubstituteEvidenceWithAlt(string evidenceName)
    {
        try
        {
            Evidence evidence = CurrentObjectList.SingleOrDefault(evidenceInList => evidenceInList.name == evidenceName);
            
            if (evidence == null)
            {
                Debug.LogWarning($"Evidence {evidenceName} has not been added to the evidence inventory.");
                return;
            }
        
            if (IsObjectInList(evidence.AltEvidence.name, CurrentObjectList))
            {
                Debug.LogWarning($"Cannot substitute evidence with alt evidence. Evidence with name {evidenceName} is already in the object inventory.");
                return;
            }
            
            if (evidence.AltEvidence == null)
            {
                Debug.LogWarning($"Cannot substitute evidence with alt evidence. Alt evidence for {evidenceName} is null.");
                return;
            }
        
            CurrentObjectList[CurrentObjectList.IndexOf(evidence)] = evidence.AltEvidence;
        }
        catch
        {
            return;
        }
    }
    
    /// <summary>
    /// Returns the evidence at the specified index as an ICourtRecordObject to be used in the court record menu.
    /// </summary>
    /// <param name="index">The index of the evidence to get.</param>
    /// <returns>The evidence as an ICourtRecordObject</returns>
    public ICourtRecordObject GetObjectAtIndex(int index)
    {
        return this[index];
    }
}
