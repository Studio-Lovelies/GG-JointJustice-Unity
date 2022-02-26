using System.Collections.Generic;

public interface INarrativeScriptPlaylist
{
    IEnumerable<NarrativeScript> GetAllNarrativeScripts();
    NarrativeScript GetRandomFailureScript();
    NarrativeScript GetNextNarrativeScript();
}
