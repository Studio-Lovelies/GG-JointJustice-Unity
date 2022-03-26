public enum SpeakingType
{
    Speaking,
    Thinking,
    SpeakingWithUnknownName
}

public interface IActorController
{
    void SetActiveActor(string actor);
    void SetActiveSpeaker(string actor, SpeakingType speakingType);
    void SetActiveSpeakerToNarrator();
    void SetPose(string pose, string actorName = null);
    void PlayEmotion(string emotion, string actorName = null);
    void StartTalking();
    void StopTalking();
    void OnAnimationDone();
    void AssignActorToSlot(string actor, int oneBasedSlotIndex);
    void SetVisibility(bool shouldShow, ActorAssetName actorName);
    void SetActiveActorObject(Actor activeSceneActiveActor);
    void OnSceneChanged(BGScene activeScene);
}
