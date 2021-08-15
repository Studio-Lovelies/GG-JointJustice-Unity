using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController
{
    void FadeIn(float seconds);
    void FadeOut(float seconds);
    void ShakeScreen(float intensity);
    void SetBackground(string background);
    void SetCameraPos(int x, int y);
    void PanCamera(float seconds, int xEnd, int yEnd);
}
