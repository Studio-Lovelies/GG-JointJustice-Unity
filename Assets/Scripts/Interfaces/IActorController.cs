public interface IActorController
{
    bool Animating { get; set; }
    
    void SetActiveActor(string actor);
    void SetActiveSpeaker(string actor);
    void ShowActor();
    void HideActor();
    void SetEmotion(string emotion);
}
