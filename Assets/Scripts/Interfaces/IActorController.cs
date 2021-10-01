public enum SpeakingType
{
    Speaking,
    Thinking
}

public interface IActorController
{
    void SetActiveActor(string actor);
    void SetActiveSpeaker(string actor);

    void SetPose(string pose, string actorName = null);
    void PlayEmotion(string emotion, string actorName = null);
    void StartTalking();
    void StopTalking();
    void OnAnimationDone();
    void SetSpeakingType(SpeakingType speakingType);
    void AssignActorToSlot(string actor, int oneBasedSlotIndex);
}
