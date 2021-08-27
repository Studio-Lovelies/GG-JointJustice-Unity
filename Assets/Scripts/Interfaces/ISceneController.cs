using UnityEngine;

public enum itemDisplayPosition
{
    Left,
    Right,
    Middle
}

public interface ISceneController
{
    void FadeIn(float seconds);
    void FadeOut(float seconds);
    void ShakeScreen(float intensity);
    void SetScene(string background);
    void SetCameraPos(Vector2Int position);
    void PanCamera(float seconds, Vector2Int finalPosition);
    void ShowItem(string item, itemDisplayPosition position);
    void ShowActor();
    void HideActor();
}
