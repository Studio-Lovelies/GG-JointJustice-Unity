public interface IBGSceneList
{
    void InstantiateBGScenes(INarrativeScript narrativeScript);
    void ClearBGScenes();
    BGScene SetScene(SceneAssetName sceneName);
}