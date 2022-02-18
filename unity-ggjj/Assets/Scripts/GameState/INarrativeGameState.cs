public interface INarrativeGameState
{
    public IActorController ActorController { get; }
    public IAppearingDialogueController AppearingDialogueController { get; }
    public IObjectStorage ObjectStorage { get; }
    public INarrativeScriptPlayer NarrativeScriptPlayer { get; }
    public IAudioController AudioController { get; }
    public IEvidenceController EvidenceController { get; }
    public ISceneController SceneController { get; }
    public IPenaltyManager PenaltyManager { get; }
}