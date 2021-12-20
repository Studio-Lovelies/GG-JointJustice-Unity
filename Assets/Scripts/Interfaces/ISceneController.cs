using UnityEngine;

public enum ItemDisplayPosition
{
    Left,
    Right,
    Middle
}

public interface ISceneController
{
    void FadeIn(float seconds);
    void FadeOut(float seconds);
    void ShakeScreen(float intensity, float duration, bool isBlocking);
    void SetScene(string background);
    void SetCameraPos(Vector2Int position);
    void PanCamera(float seconds, Vector2Int finalPosition);
    void PanToActorSlot(int oneBasedSlotIndex, float seconds);
    void JumpToActorSlot(int oneBasedSlotIndex);
    void ShowItem(string item, ItemDisplayPosition position);
    void ShowActor();
    void HideActor();
    void Wait(float seconds);
    void HideItem();
    void PlayAnimation(string animationName);
    void Objection(string actorName);
    void HoldIt(string actorName);
    void TakeThat(string actorName);
    void Shout(string actorName, string shoutName);
    void IssuePenalty();
    void ReloadScene();
}
