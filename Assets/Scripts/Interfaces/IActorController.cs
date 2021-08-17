public interface IActorController
{
    void SetActiveActor(string actor);
    void SetActiveSpeaker(string actor);
    void ShowActor();
    void HideActor();
    void SetEmotion(string emotion);
}
