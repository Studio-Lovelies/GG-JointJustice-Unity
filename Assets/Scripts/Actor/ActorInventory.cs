public class ActorInventory : ObjectInventory<ActorData, ActorList>, ICourtRecordObjectInventory
{
    /// <summary>
    /// Returns the actor data at the specified index as an ICourtRecordObject to be used in the court record menu.
    /// </summary>
    /// <param name="index">The index of the actor data to get.</param>
    /// <returns>The actor data as an ICourtRecordObject</returns>
    public ICourtRecordObject GetObjectAtIndex(int index)
    {
        return ObjectStorage[index];
    }
}