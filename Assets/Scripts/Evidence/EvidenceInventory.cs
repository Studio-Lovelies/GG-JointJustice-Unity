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
            Evidence altEvidence = ObjectDictionary[evidenceName].AltEvidence;
            ObjectDictionary.SubstituteObject(evidenceName, altEvidence);
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
        return ObjectDictionary[index];
    }
}
