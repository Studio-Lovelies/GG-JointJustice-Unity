using System.Collections.Generic;

public interface INarrativeScriptPlaylist
{
    public void InitializeNarrativeScripts();
    
    NarrativeScript GetNextNarrativeScript();
    IEnumerable<NarrativeScript> GetAllNarrativeScripts();
    NarrativeScript GetRandomFailureScript();
}
