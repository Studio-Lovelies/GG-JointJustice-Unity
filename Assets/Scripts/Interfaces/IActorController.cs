public interface IActorController
{ 
    void SetActiveActor(string actor);
    void SetActiveSpeaker(string actor);

    void SetPose(string pose);
    void PlayEmotion(string emotion);
    void StartTalking();
    void StopTalking();
    void OnAnimationDone();
}
