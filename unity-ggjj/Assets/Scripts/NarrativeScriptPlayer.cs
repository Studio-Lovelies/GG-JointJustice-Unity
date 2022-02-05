using UnityEngine;

public class NarrativeScriptPlayer : MonoBehaviour
{
    public StoryPlayer StoryPlayer { get; set; }
    public NarrativeScript ActiveNarrativeScript => StoryPlayer.ActiveNarrativeScript;

    private void Awake()
    {
        GetComponent<Game>().NarrativeScriptPlayer = this;
    }
    
    public void Continue()
    {
        StoryPlayer.Continue();
    }

    public void HandleChoice(int choiceIndex)
    {
        StoryPlayer.HandleChoice(choiceIndex);
    }

    public void PressWitness()
    {
        StoryPlayer.HandleChoice(1);
    }
}
