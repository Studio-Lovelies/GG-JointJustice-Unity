public interface IActorController
{
    bool Animating { get; }
    
    void SetActiveActor(string actor);
    void SetActiveSpeaker(string actor);
    void SetEmotion(string emotion);
}
