public interface IActorController
{ 
    void SetActiveActor(string actor);
    void SetActiveSpeaker(string actor);
    void PlayAnimation(string emotion);
    void StartTalking();
    void StopTalking();
    void OnAnimationDone();
}
