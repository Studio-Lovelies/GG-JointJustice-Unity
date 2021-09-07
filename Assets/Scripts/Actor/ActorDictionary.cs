public class ActorDictionary : ObjectDictionaryInterface<ActorData, ActorList>, ICourtRecordObjectDictionary
{
    /// <summary>
    /// Returns the actor data at the specified index as an ICourtRecordObject to be used in the court record menu.
    /// </summary>
    /// <param name="index">The index of the actor data to get.</param>
    /// <returns>The actor data as an ICourtRecordObject</returns>
    public new ICourtRecordObject GetObjectAtIndex(int index)
    {
        return ObjectsDictionary.GetObjectAtIndex(index);
    }
}