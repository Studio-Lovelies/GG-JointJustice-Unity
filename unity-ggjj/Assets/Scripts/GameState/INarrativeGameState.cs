public interface INarrativeGameState
{
    IActorController ActorController { get; }
    IAppearingDialogueController AppearingDialogueController { get; }
    IObjectStorage ObjectStorage { get; }
    INarrativeScriptPlayer NarrativeScriptPlayer { get; }
    IAudioController AudioController { get; }
    IEvidenceController EvidenceController { get; }
    ISceneController SceneController { get; }
    IPenaltyManager PenaltyManager { get; }
    INarrativeScriptPlaylist NarrativeScriptPlaylist { get; }
    IActionDecoder ActionDecoder { get; }
    IChoiceMenu ChoiceMenu { get; }
}