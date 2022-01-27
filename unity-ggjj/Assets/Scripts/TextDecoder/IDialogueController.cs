public interface IDialogueController
{
    GameMode GameMode { set; }
    void StartSubStory(NarrativeScript narrativeScript);
}