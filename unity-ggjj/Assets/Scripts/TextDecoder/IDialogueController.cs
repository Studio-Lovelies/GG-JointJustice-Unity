public interface IDialogueController
{
    GameMode GameMode { set; }
    void StartSubStory(NarrativeScript narrativeScript);
    void SetNewDialogue(NarrativeScript narrativeScript);
}